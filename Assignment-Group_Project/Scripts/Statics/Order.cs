using System;
using System.Collections.Generic;
using System.Linq;
using Assignment_Group_Project.Scripts.Objects;
using Spectre.Console;
using Assignment_Group_Project.Scripts.Utilities;

public static class Order
{

    // string orderType bedtyder att man kan skriva in en string som parameter när man anropar metoden.
    // Så att man kan använda metoden flera gånger med olika strings.
    private static void CreateOrder(string orderType)
    {
        AnsiConsole.Clear();

        // Hämtar data från JSON-filen.
        DataStorage dataStorage = new DataStorage();
        AppData appData = dataStorage.Load();

        // Lista som sparar produkterna i ordern.
        List<Product> orderedProductsList = new();

        // Skapar ett automatiskt ordernummer.
        Random random = new Random();
        int orderNumber = random.Next(10000, 99999);

        // Sparar datum och tid för ordern.
        DateTime orderDate = DateTime.Now;

        // Skapar ett objekt för den nya ordern.
        OrderDetails newOrder = new OrderDetails
        {
            OrderNumber = orderNumber,
            OrderDate = orderDate,
            OrderType = orderType,
        };

        AnsiConsole.Write(new Rule($"Starting {Markup.Escape(orderType)}"));

        bool addingMoreProducts = true;

        while (addingMoreProducts)
        {
            Product newProduct = new();

            // Frågar efter produktens namn med Spectre.Console.
            newProduct.ProductName = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the name of the product:")
                    .AllowEmpty());

            if (string.IsNullOrWhiteSpace(newProduct.ProductName))
            {
                ShowError("Product name cannot be empty");
                continue;
            }

            // Kontrollerar att produkten finns i lagret för utgående ordrar.
            Product? existingProduct = null;

            if (orderType == "Outgoing Order")
            {
                existingProduct = appData.Products.FirstOrDefault(product =>
                    product.ProductName != null &&
                    product.ProductName.Equals(
                        newProduct.ProductName,
                        StringComparison.OrdinalIgnoreCase));

                if (existingProduct == null)
                {
                    ShowError("Product does not exist in storage.");
                    continue;
                }

                // Hämtar artikelnumret från produkten i lagret.
                newProduct.ProductArticleNumber = existingProduct.ProductArticleNumber;
            }

            // Frågar efter antal.
            string amountInput = AnsiConsole.Prompt(
                new TextPrompt<string>("Amount of product:"));

            if (!int.TryParse(amountInput, out int amount) || amount <= 0)
            {
                ShowError("Invalid amount. Enter number bigger than 0.");
                continue;
            }

            newProduct.OrderAmount = amount;

            if (orderType == "Outgoing Order")
            {
                // Räknar hur många exemplar av samma produkt som redan finns i ordern.
                int alreadyOrdered = orderedProductsList
                    .Where(product => product.ProductArticleNumber == existingProduct!.ProductArticleNumber)
                    .Sum(product => product.OrderAmount);

                // Räknar ut hur många exemplar som fortfarande är tillgängliga.
                int availableQuantity = existingProduct!.ProductStockQuantity - alreadyOrdered;

                // Kontrollerar att lagret räcker.
                if (amount > availableQuantity)
                {
                    ShowError($"Not enough products in stock. Available quantity: {availableQuantity}");
                    continue;
                }
            }

            // Frågar efter pris per produkt.
            string priceInput = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter price per product:"));

            if (!decimal.TryParse(priceInput, out decimal price) || price <= 0)
            {
                ShowError("Invalid price.");
                continue;
            }

            newProduct.ProductPrice = price;

            // Lägger till produkten i orderlistan.
            orderedProductsList.Add(newProduct);

            AnsiConsole.MarkupLine("[green]Product added![/]");

            // Frågar om användaren vill lägga till fler produkter.
            AnsiConsole.MarkupLine("Do you want to add another product?");

            User.AskYesOrNo();

            // Kontrollerar användarens svar.
            if (User.inputChoice?.ToLower() != "yes")
            {
                addingMoreProducts = false;
            }
        }

        // Kopplar produktlistan till orderobjektet.
        newOrder.OrderedProductsList = orderedProductsList;

        if (orderedProductsList.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Order must contain at least one product.[/]");
            return;
        }

        // Leverantörens namn behövs endast för inkommande order.
        string supplierName = "";

        if (orderType == "Incoming Order")
        {
            supplierName = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter supplier company name:")
                    .AllowEmpty());

            if (string.IsNullOrWhiteSpace(supplierName))
            {
                ShowError("Supplier name cannot be empty.");
                return;
            }
        }

        // Sparar leverantörens namn i orderobjektet.
        newOrder.SupplierName = supplierName;

        // Minskar lagersaldot för varje produkt i en utgående order.
        if (orderType == "Outgoing Order")
        {
            foreach (Product orderedProduct in orderedProductsList)
            {
                // Hittar den riktiga produkten i lagerlistan.
                Product? storageProduct = appData.Products.FirstOrDefault(product =>
                    product.ProductArticleNumber == orderedProduct.ProductArticleNumber);

                if (storageProduct != null)
                {
                    storageProduct.ProductStockQuantity -= orderedProduct.OrderAmount;
                }
            }
        }

        // Lägger till den nya ordern i listan med alla order.
        appData.Orders.Add(newOrder);

        // Sparar all data.
        dataStorage.Save(appData);

        // Räknar ut totalsumman för hela ordern.
        decimal totalOrderPrice = orderedProductsList.Sum(
            product => product.ProductPrice * product.OrderAmount);

        // Visar kvittot med Spectre.Console.
        AnsiConsole.Write(new Rule("[green]ORDER RECEIPT[/]"));

        AnsiConsole.MarkupLine($"Order type: {Markup.Escape(orderType)}");
        AnsiConsole.MarkupLine($"Order number: {orderNumber}");
        AnsiConsole.MarkupLine($"Order date: {orderDate:yyyy-MM-dd HH:mm}");

        if (orderType == "Incoming Order")
        {
            AnsiConsole.MarkupLine($"Supplier: {Markup.Escape(supplierName)}");
        }

        // Skapar en tabell för produkterna i ordern.
        Table receiptTable = new Table();

        receiptTable.AddColumn("Product");
        receiptTable.AddColumn("Amount");
        receiptTable.AddColumn("Price per product");
        receiptTable.AddColumn("Total");

        foreach (Product product in orderedProductsList)
        {
            receiptTable.AddRow(
                Markup.Escape(product.ProductName ?? ""),
                product.OrderAmount.ToString(),
                $"{product.ProductPrice:F2} kr",
                $"{product.ProductPrice * product.OrderAmount:F2} kr");
        }

        AnsiConsole.Write(receiptTable);

        AnsiConsole.MarkupLine(
            $"[bold green]TOTAL: {totalOrderPrice:F2} kr[/]");

        AnsiConsole.Write(new Rule());
    }


        


    public static void AddOutgoingOrder()
    {
        CreateOrder("Outgoing Order");
    }




    public static void AddIncomingOrder()
    {
        CreateOrder("Incoming Order");
    }







    // Metod för att uppdatera statusen på en befintlig order.
    public static void UpdateOrder()
    {
        AnsiConsole.Clear();

        // Hämtar sparad data från JSON-filen.
        DataStorage dataStorage = new DataStorage();
        AppData appData = dataStorage.Load();

        // Hämtar alla aktiva ordrar, både Incoming och Outgoing.
        List<OrderDetails> allOrdersList = appData.Orders;

        // Kontrollerar om det finns några aktiva ordrar.
        if (allOrdersList.Count == 0)
        {
            AnsiConsole.MarkupLine("No orders found.");
            return;
        }

        // Bestämmer hur många ordrar som ska visas per sida.
        int pageSize = 10;

        // Håller reda på vilken sida användaren befinner sig på.
        int currentPage = 0;

        // Variabel som sparar den order användaren väljer.
        OrderDetails? orderToUpdate = null;

        // Fortsätter visa sidor tills användaren har valt en order.
        while (orderToUpdate == null)
        {
            // Räknar ut hur många ordrar som ska hoppas över.
            int startIndex = currentPage * pageSize;

            // Hämtar max 10 ordrar för den aktuella sidan.
            List<OrderDetails> pageOrders = allOrdersList.Skip(startIndex).Take(pageSize).ToList();

            // Skapar menyval för sidan.
            List<string> choices = pageOrders.Select(order => $"Order #{order.OrderNumber} - {order.OrderType} - {order.OrderStatus}").ToList();

            choices.Add("Go Back");

            // Lägger till navigeringsalternativ om det finns fler sidor.
            if (currentPage > 0)
            {
                choices.Add("Previous page");
            }

            if (startIndex + pageSize < allOrdersList.Count)
            {
                choices.Add("Next page");
            }

            // Visar ordrarna och navigeringsalternativen.
            string selectedChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title($"Choose an order (Page {currentPage + 1}):")
            .AddChoices(choices));


            if (selectedChoice == "Next page")
            {
                currentPage++;
            }

            else if (selectedChoice == "Previous page")
            {
                currentPage--;
            }

            else if (selectedChoice == "Go Back")
            {
                return;
            }

            else
            {
                int selectedIndex = choices.IndexOf(selectedChoice);
                orderToUpdate = pageOrders[selectedIndex];
            }
        }

        // Visar vilken order användaren har valt.
        AnsiConsole.MarkupLine($"Order found, Current status: {orderToUpdate.OrderStatus}");



        // Skapar en lista med tillåtna statusändringar.
        List<string> availableStatuses = new();

        // Kontrollerar orderns nuvarande status.
        switch (orderToUpdate.OrderStatus)
        {
            case "Packing":
                availableStatuses.Add("Shipped");
                availableStatuses.Add("Cancelled");
            break;

            case "Shipped":
                availableStatuses.Add("Delivered");
            break;

            case "Delivered":
            case "Cancelled":
                // Dessa ordrar kan inte få en ny status men användaren ska fortfarande kunna ta bort dem.
            break;
        }

        // Lägger till möjligheten att ta bort ordern eller gå tillbaka.
        availableStatuses.Add("Delete Order");
        availableStatuses.Add("Register Supplier Confirmation");
        availableStatuses.Add("Go Back");

        // Visar menyn med statusändringar och borttagning.
        string selectedOption = AnsiConsole.Prompt(
        new SelectionPrompt<string>().Title("Choose what you want to do:").AddChoices(availableStatuses));


        if (selectedOption == "Go Back")
        {
            return;
        }



        // Kontrollerar om användaren vill registrera en leverantörsbekräftelse.
        if (selectedOption == "Register Supplier Confirmation")
        {
            // Kontrollerar att det är en Incoming Order.
            if (orderToUpdate.OrderType != "Incoming Order")
            {
                ShowError("Supplier confirmation is only available for incoming orders.");
                return;
            }

            // Kontrollerar om ordern redan har blivit bekräftad.
            if (orderToUpdate.SupplierConfirmed)
            {
                ShowError("This order has already been confirmed.");
                return;
            }

            AnsiConsole.MarkupLine("Have you received an order confirmation from the supplier?");
            // Frågar användaren om bekräftelsen har mottagits.
            User.AskYesOrNo();

            if (User.inputChoice == "Yes")
            {
                // Registrerar att leverantören har bekräftat ordern.
                orderToUpdate.SupplierConfirmed = true;

                // Sparar ändringen i JSON-filen.
                dataStorage.Save(appData);

                AnsiConsole.MarkupLine("[green]Supplier confirmation successfully registered![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("No changes were made.");
            }

            return;
        }



        // Kontrollerar om användaren har valt att ta bort ordern.
        if (selectedOption == "Delete Order")
        {
            AnsiConsole.MarkupLine($"Are you sure you want to delete order #{orderToUpdate.OrderNumber}?");

            // Frågar användaren om borttagningen ska bekräftas.
            User.AskYesOrNo();

            if (User.inputChoice == "Yes")
            {
                // Flyttar ordern från aktiva ordrar till orderhistoriken.
                appData.Orders.Remove(orderToUpdate);
                appData.OrderHistory.Add(orderToUpdate);

                // Sparar ändringarna.
                dataStorage.Save(appData);

                AnsiConsole.MarkupLine("[green]Order successfully deleted & moved to order history![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("Order was not deleted.");
            }
            return;
        }

        // Uppdaterar orderns status.
        orderToUpdate.OrderStatus = selectedOption;

        // Om ordern har statusen deliverd flyttas den till historik listan.
        if (selectedOption == "Delivered")
        {
            orderToUpdate.OrderReceivedDate = DateTime.Now;

            appData.Orders.Remove(orderToUpdate);
            appData.OrderHistory.Add(orderToUpdate);

            AnsiConsole.MarkupLine("[green]Order moved to order history![/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]Order status updated to: {Markup.Escape(selectedOption)}[/]");
        }

        // Sparar ändringarna i JSON-filen.
        dataStorage.Save(appData);
    }








    public static void CheckOrders()
    {
        AnsiConsole.Clear();

        // Hämtar sparad data från JSON-filen.
        DataStorage dataStorage = new DataStorage();
        AppData appData = dataStorage.Load();


        // Låter användaren välja vilken typ av order som ska visas.
        string selectedOrderType = AnsiConsole.Prompt(
        // SelectionPromt gör en meny.    
        new SelectionPrompt<string>().Title("Which orders would you like to view?").AddChoices("Incoming Order", "Outgoing Order"));

        // Kontrollerar om det finns några order.
        if (appData.Orders.Count == 0)
        {
            AnsiConsole.MarkupLine("No orders found.");
            return;
        }

        // Filtrerar orderlistan utifrån användarens val.
        // säger att den ska gå igenom alla order i appData.Orders och kolla om order.OrderType == selectedOrderType.
        List<OrderDetails> selectedOrdersList = appData.Orders.Where(order => order.OrderType == selectedOrderType).ToList();


        // Kontrollerar om det finns order av den valda typen.
        if (selectedOrdersList.Count == 0)
        {
            AnsiConsole.MarkupLine("No orders found for this order type.");
            return;
        }


        // Låter användaren välja vilken order som ska visas.
        OrderDetails selectedOrder = AnsiConsole.Prompt(new SelectionPrompt<OrderDetails>()
        .Title("Choose an order:")
        //.UseConverter bestämmer hur varje order ska visas i menyn. Här visas ordernummer och status.
        .UseConverter(order => $"Order #{order.OrderNumber} - {order.OrderStatus}")
        //.AddChoices(selectedOrders) lägger till alla order i den filtrerade listan som valbara alternativ.
        .AddChoices(selectedOrdersList));

        
        // Visar detaljer för den valda ordern.
        // Skapar en panel med information om den valda ordern.
        var orderInfo = new List<string>
        {
            $"Order Number: {selectedOrder.OrderNumber}",
            $"Order Type: {Markup.Escape(selectedOrder.OrderType ?? "-")}",
            $"Order Status: {Markup.Escape(selectedOrder.OrderStatus)}",
            $"Order Date: {selectedOrder.OrderDate:yyyy-MM-dd HH:mm}"
        };

        // Visar leveranstörens namn om det finns.
        if (!string.IsNullOrEmpty(selectedOrder.SupplierName))
        {
            orderInfo.Add($"Supplier: {Markup.Escape(selectedOrder.SupplierName)}");
        }

        // Visar om leverantören har bekräftat en Incoming Order.
        if (selectedOrder.OrderType == "Incoming Order")
        {
            if (selectedOrder.SupplierConfirmed)
            {
                orderInfo.Add("[green]Supplier Confirmation: Confirmed[/]");
            }
            else
            {
                orderInfo.Add("Supplier Confirmation: Awaiting confirmation");
            }
        }

        // Skapar en panel som visar orderinformationen.
        var orderPanel = new Panel(string.Join("\n", orderInfo))
            .Header("ORDER DETAILS")
            .Border(BoxBorder.Rounded);

        AnsiConsole.Write(orderPanel);

        // Skapar en tabell för produkterna i ordern.
        Table productTable = new Table();

        productTable.Border(TableBorder.Rounded);
        productTable.AddColumn("Product Name");
        productTable.AddColumn("Amount");
        productTable.AddColumn("Price per Product");
        productTable.AddColumn("Total");

        foreach (Product product in selectedOrder.OrderedProductsList)
        {
            // F2 gör att priset visas med två decimaler.
            productTable.AddRow(
                Markup.Escape(product.ProductName ?? "-"),
                product.OrderAmount.ToString(),
                $"{product.ProductPrice:F2} kr",
                $"{product.ProductPrice * product.OrderAmount:F2} kr"
            );
        }

        AnsiConsole.Write(productTable);




        // Skapa menyvalen som användaren kan välja mellan.
        List<string> choices = new List<string>();

        // Visa bara Mark as Delivered om ordern har skickats.
        if (selectedOrder.OrderStatus == "Shipped")
        {
            choices.Add("Mark as Delivered");
        }

        // Användaren ska alltid kunna gå tillbaka.
        choices.Add("Go Back");

        
        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What would you like to do?")
            .AddChoices(choices));


        switch (choice)
        {
            case "Mark as Delivered":

                // Kontrollerar om ordern har skickats.
                if (selectedOrder.OrderStatus == "Shipped")
                {
                    selectedOrder.OrderStatus = "Delivered";
                    selectedOrder.OrderReceivedDate = DateTime.Now;

                    // Flyttar ordern till historiken.
                    appData.Orders.Remove(selectedOrder);
                    appData.OrderHistory.Add(selectedOrder);

                    dataStorage.Save(appData);

                    AnsiConsole.MarkupLine("[green]Order successfully moved to order history![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Only shipped orders can be marked as delivered.[/]");
                }
                break;

            case "Go Back":
            return;
        }
    }









    public static void OrderMenu()
    {
        bool orderMenuIsRunning = true;

        while (orderMenuIsRunning)
        {
            Console.Clear();

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Order Menu")
                    .AddChoices(
                        "Check Orders",
                        "Check Order History",
                        "Add Outgoing Order",
                        "Add Incoming Order",
                        "Update Order",
                        "Back To Main Menu"
                    ));

            switch (choice)
            {
                case "Check Orders":
                    CheckOrders();
                    break;

                case "Check Order History":
                    CheckOrderHistory();
                    break;

                case "Add Outgoing Order":
                    AddOutgoingOrder();
                    break;

                case "Add Incoming Order":
                    AddIncomingOrder();
                    break;

                case "Update Order":
                    UpdateOrder();
                    break;

                case "Back To Main Menu":
                    orderMenuIsRunning = false;
                    break;
            }

            if (orderMenuIsRunning)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }








    public static void CheckOrderHistory()
    {
        AnsiConsole.Clear();

        // Hämtar data från json och sparar det i appData.
        DataStorage dataStorage = new DataStorage();
        AppData appData = dataStorage.Load();

        // Gör en lista som innehåller alla ordrar i orderhistoriken.
        List<OrderDetails> orderHistoryList = appData.OrderHistory;

        // Kontrollera om det finns några tidigare ordrar.
        if (orderHistoryList.Count == 0)
        {
            AnsiConsole.MarkupLine("No orders found in order history.");
            return;
        }


        // Låter användaren välja vilken order den vill titta mer på.
        OrderDetails selectedOrder = AnsiConsole.Prompt(new SelectionPrompt<OrderDetails>()
            .Title("Choose an order:")
            //UseConverter bestämmer hur varje order ska visas i menyn. Här visas ordernummer och status.
            .UseConverter(order => $"Order #{order.OrderNumber} - {order.OrderStatus}")
            .AddChoices(orderHistoryList));


        // Visar information om ordern.
        var orderInfo = new List<string>
        {
            $"Order Number: {selectedOrder.OrderNumber}",
            $"Order Type: {Markup.Escape(selectedOrder.OrderType ?? "")}",
            $"Order Status: {Markup.Escape(selectedOrder.OrderStatus)}",
            $"Order Date: {selectedOrder.OrderDate:yyyy-MM-dd HH:mm}"
        };

        // Visar mottagningsdatum om det finns.
        if (selectedOrder.OrderReceivedDate != null)
        {
            orderInfo.Add($"Received Date: {selectedOrder.OrderReceivedDate:yyyy-MM-dd HH:mm}");
        }

        // Visar leverantör om det finns.
        if (!string.IsNullOrEmpty(selectedOrder.SupplierName))
        {
            orderInfo.Add($"Supplier: {Markup.Escape(selectedOrder.SupplierName)}");
        }

        AnsiConsole.Write(new Rule("[green]ORDER HISTORY[/]"));

        // Skapar en panel som visar orderinformationen.
        var orderPanel = new Panel(string.Join("\n", orderInfo))
            .Header("ORDER DETAILS")
            .Border(BoxBorder.Rounded);

        AnsiConsole.Write(orderPanel);

        // Skapar en tabell för produkterna i ordern.
        Table productTable = new Table();

        productTable.AddColumn("Product Name");
        productTable.AddColumn("Amount");
        productTable.AddColumn("Price per Product");
        productTable.AddColumn("Total");

        foreach (Product product in selectedOrder.OrderedProductsList)
        {
            productTable.AddRow(
                Markup.Escape(product.ProductName ?? "-"),
                product.OrderAmount.ToString(),
                $"{product.ProductPrice:F2} kr",
                $"{product.ProductPrice * product.OrderAmount:F2} kr"
            );
        }

        AnsiConsole.Write(productTable);
    }









    // Skriver ut error i röd text.
    public static void ShowError (string message)
    {
        AnsiConsole.MarkupLine($"[red]{Markup.Escape(message)}[/]");
    }

}