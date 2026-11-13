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
        Console.Clear();

        // Hämtar data från data.json och sparar det i appData.
        DataStorage dataStorage = new DataStorage();
        AppData appData = dataStorage.Load();

        // Skapar en lista, den sparar produkterna i ordern.
        List<Product> orderedProductsList = new();


        // Skapar ett Random objekt som används för att generera ett automatiskt ordernummer.
        Random random = new Random();

        // Detta nummer används som order nummer.
        int orderNumber = random.Next(10000, 99999);

        // Spara datum och tid för ordern.
        DateTime orderDate = DateTime.Now;

        // Skapar ett objekt som innehåller info om den nya ordern.
        OrderDetails newOrder = new OrderDetails
        {
            OrderNumber = orderNumber,
            OrderDate = orderDate,
            OrderType = orderType,
        };

        Console.WriteLine($"Starting {orderType}.");

        bool addingMoreProducts = true;

        while (addingMoreProducts)
        {
            Product newProduct = new();

            // ?? "" hanterar null.
            Console.Write("Enter the name of the product: ");
            newProduct.ProductName = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(newProduct.ProductName))
            {
                Console.WriteLine("Product name cannot be empty");
                continue;
            }

            // Antal av produkt
            Console.Write("Amount of product: ");
            // Om det inte är siffor skriver den ut invalid amount.
            // int parse kontrollerar om det är siffror och om det är mindre än 0.
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Enter number bigger then 0.");
                continue;
            }
            // TILLFÄLLIG lösning för att spara antalet i ordern, detta ändras med kopplingen till storage senare.
            // Behöver skilja antalet i ordern och antalet i lagret.
            newProduct.ProductStockQuantity = amount;



            // Frågar efter pris per produkt.
            Console.Write("Enter price per product: ");

            // Om det inte är siffor eller om det är mindre än 0, så skrivs detta ut.
            // out decimal price skapar en variable som heter price.
            if (!decimal.TryParse(Console.ReadLine(), out decimal price)|| price <= 0)
            {
                Console.WriteLine("Invalid price.");
                continue;
            }
            newProduct.ProductPrice = price;


            // Lägger till produkten i orderlistan.
            orderedProductsList.Add(newProduct);


            Console.WriteLine("Product added!");



            // Frågar om användaren vill lägga till fler produkter.
            Console.WriteLine("Do you want to add another product?");
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
                Console.WriteLine("Order must contain at least one product.");
                return;
            }



            // Leverantörens namn behövs endast för inkommande order.
            string supplierName = "";

            if (orderType == "Incoming Order")
            {
                Console.Write("Enter supplier company name: ");
                supplierName = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(supplierName))
                {
                    Console.WriteLine("Supplier name cannot be empty.");
                    return;
                }
            }
            // Sparar leverantörens namn i orderobjektet.
            newOrder.SupplierName = supplierName;

            // Lägger till den nya ordern i listan med alla order.
            appData.Orders.Add(newOrder);

            // Sparar all data och den den nya ordern i data.json.
            dataStorage.Save(appData);



            // Räknar ut totalsumman för hela ordern.
            decimal totalOrderPrice = orderedProductsList.Sum(product => product.ProductPrice * product.ProductStockQuantity);



            // Skriver ut orderns kvitto.
            Console.WriteLine("\n========== ORDER RECEIPT ==========");
            Console.WriteLine($"Order type: {orderType}");
            Console.WriteLine($"Order number: {orderNumber}");
            Console.WriteLine($"Order date: {orderDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine("-----------------------------------");

            // Visar leverantörens namn endast för inkommande order.
            if (orderType == "Incoming Order")
            {
                Console.WriteLine($"Supplier: {supplierName}");
            }

            

            foreach (Product product in orderedProductsList)
            {
                Console.WriteLine($"Product: {product.ProductName}");
                Console.WriteLine($"Amount: {product.ProductStockQuantity}");
                Console.WriteLine($"Price per product: {product.ProductPrice:F2} kr");
                Console.WriteLine($"Total: {product.ProductPrice * product.ProductStockQuantity:F2} kr");
                Console.WriteLine("-----------------------------------");
            }

                Console.WriteLine($"TOTAL: {totalOrderPrice:F2} kr");
                Console.WriteLine("===================================");
            
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
        Console.Clear();

        // Hämtar sparad data från JSON-filen.
        DataStorage dataStorage = new DataStorage();
        AppData appData = dataStorage.Load();

        // Hämtar alla aktiva ordrar, både Incoming och Outgoing.
        List<OrderDetails> allOrdersList = appData.Orders;

        // Kontrollerar om det finns några aktiva ordrar.
        if (allOrdersList.Count == 0)
        {
            Console.WriteLine("No orders found.");
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
        Console.WriteLine($"Order found, Current status: {orderToUpdate.OrderStatus}");



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
                Console.WriteLine("Supplier confirmation is only available for incoming orders.");
                return;
            }

            // Kontrollerar om ordern redan har blivit bekräftad.
            if (orderToUpdate.SupplierConfirmed)
            {
                Console.WriteLine("This order has already been confirmed.");
                return;
            }

            Console.WriteLine("Have you received an order confirmation from the supplier?");

            // Frågar användaren om bekräftelsen har mottagits.
            User.AskYesOrNo();

            if (User.inputChoice == "Yes")
            {
                // Registrerar att leverantören har bekräftat ordern.
                orderToUpdate.SupplierConfirmed = true;

                // Sparar ändringen i JSON-filen.
                dataStorage.Save(appData);

                Console.WriteLine("Supplier confirmation successfully registered!");
            }
            else
            {
                Console.WriteLine("No changes were made.");
            }

            return;
        }



        // Kontrollerar om användaren har valt att ta bort ordern.
        if (selectedOption == "Delete Order")
        {
            Console.WriteLine($"Are you sure you want to delete order #{orderToUpdate.OrderNumber}?");

            // Frågar användaren om borttagningen ska bekräftas.
            User.AskYesOrNo();

            if (User.inputChoice == "Yes")
            {
                // Flyttar ordern från aktiva ordrar till orderhistoriken.
                appData.Orders.Remove(orderToUpdate);
                appData.OrderHistory.Add(orderToUpdate);

                // Sparar ändringarna.
                dataStorage.Save(appData);

                Console.WriteLine("Order successfully deleted & moved to order history!");
            }
            else
            {
                Console.WriteLine("Order was not deleted.");
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

            Console.WriteLine("Order moved to order history!");
        }
        else
        {
            Console.WriteLine($"Order status updated to: {selectedOption}");
        }

        // Sparar ändringarna i JSON-filen.
        dataStorage.Save(appData);
    }








    public static void CheckOrders()
    {
        Console.Clear();

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
            Console.WriteLine("No orders found.");
            return;
        }

        // Filtrerar orderlistan utifrån användarens val.
        // säger att den ska gå igenom alla order i appData.Orders och kolla om order.OrderType == selectedOrderType.
        List<OrderDetails> selectedOrdersList = appData.Orders.Where(order => order.OrderType == selectedOrderType).ToList();


        // Kontrollerar om det finns order av den valda typen.
        if (selectedOrdersList.Count == 0)
        {
            Console.WriteLine("No orders found for this order type.");
            return;
        }


        // Visar alla order av den valda typen.
        foreach (OrderDetails order in selectedOrdersList)
        {
            Console.WriteLine($"Order Number: {order.OrderNumber}");
            Console.WriteLine($"Order Status: {order.OrderStatus}");
            Console.WriteLine($"Order Date: {order.OrderDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine("-----------------------------------");
        }

        // Låter användaren välja vilken order som ska visas.
        OrderDetails selectedOrder = AnsiConsole.Prompt(new SelectionPrompt<OrderDetails>()
        .Title("Choose an order:")
        //.UseConverter bestämmer hur varje order ska visas i menyn. Här visas ordernummer och status.
        .UseConverter(order => $"Order #{order.OrderNumber} - {order.OrderStatus}")
        //.AddChoices(selectedOrders) lägger till alla order i den filtrerade listan som valbara alternativ.
        .AddChoices(selectedOrdersList));

        Console.Clear();
        // Visar detaljer för den valda ordern.
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Order Number: {selectedOrder.OrderNumber}");
        Console.WriteLine($"Order Type: {selectedOrder.OrderType}");
        Console.WriteLine($"Order Status: {selectedOrder.OrderStatus}");
        Console.WriteLine($"Order Date: {selectedOrder.OrderDate:yyyy-MM-dd HH:mm}");

        // Visar leveranstörens namn om det finns.
        if (!string.IsNullOrEmpty(selectedOrder.SupplierName))
        {
            Console.WriteLine($"Supplier: {selectedOrder.SupplierName}");
        }


        // Visar om leverantören har bekräftat en Incoming Order.
        if (selectedOrder.OrderType == "Incoming Order")
        {
            if (selectedOrder.SupplierConfirmed)
            {
                Console.WriteLine("Supplier Confirmation: Confirmed");
            }
            else
            {
                Console.WriteLine("Supplier Confirmation: Awaiting confirmation");
            }
        }




        Console.WriteLine("Ordered Products:");

        foreach (Product product in selectedOrder.OrderedProductsList)
        {
            // F2 gör att priset visas med två decimaler.
            Console.WriteLine($" - Product Name: {product.ProductName}, Amount: {product.ProductStockQuantity}, Price per Product: {product.ProductPrice:F2} kr");
        }
        Console.WriteLine("-----------------------------------");



        // Skapa menyvalen som användaren kan välja mellan.
        List<string> choices = new List<string>();

        // Visa bara Mark as Delivered om ordern har skickats.
        if (selectedOrder.OrderStatus == "Shipped")
        {
            choices.Add("Mark as Delivered");
        }

        // Användaren ska alltid kunna gå tillbaka.
        choices.Add("Go Back");

        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(choices));

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

                    Console.WriteLine("Order successfully moved to order history!");
                }
                else
                {
                    Console.WriteLine("Only shipped orders can be marked as delivered.");
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
        Console.Clear();

        // Hämtar data från json och sparar det i appData.
        DataStorage dataStorage = new DataStorage();
        AppData appData = dataStorage.Load();

        // Gör en lista som innehåller alla ordrar i orderhistoriken.
        List<OrderDetails> orderHistoryList = appData.OrderHistory;

        // Kontrollera om det finns några tidigare ordrar.
        if (orderHistoryList.Count == 0)
        {
            Console.WriteLine("No orders found in order history.");
            return;
        }


        // Låter användaren välja vilken order den vill titta mer på.
        OrderDetails selectedOrder = AnsiConsole.Prompt(new SelectionPrompt<OrderDetails>()
            .Title("Choose an order:")
            //UseConverter bestämmer hur varje order ska visas i menyn. Här visas ordernummer och status.
            .UseConverter(order => $"Order #{order.OrderNumber} - {order.OrderStatus}")
            .AddChoices(orderHistoryList));


        // Visar mottagningsdatum om det finns.
        if (selectedOrder.OrderReceivedDate != null)
        {
            Console.WriteLine($"Received Date: {selectedOrder.OrderReceivedDate:yyyy-MM-dd HH:mm}");
        }

        // Visar leverantör om det finns.
        if (!string.IsNullOrEmpty(selectedOrder.SupplierName))
        {
            Console.WriteLine($"Supplier: {selectedOrder.SupplierName}");
        }


        foreach (Product product in selectedOrder.OrderedProductsList)
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"Order Number: {selectedOrder.OrderNumber}");
            Console.WriteLine($"Order Type: {selectedOrder.OrderType}");
            Console.WriteLine($"Order Status: {selectedOrder.OrderStatus}");
            Console.WriteLine($"Order Date: {selectedOrder.OrderDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Product Name: {product.ProductName}");
            Console.WriteLine($"Amount: {product.ProductStockQuantity}");
            Console.WriteLine($"Price per Product: {product.ProductPrice:F2} kr");
            Console.WriteLine($"Total: {product.ProductPrice * product.ProductStockQuantity:F2} kr");
            Console.WriteLine("-----------------------------------");
        }
        
    }
}














namespace Assignment_Group_Project.Scripts.Objects
{
// Representerar en enskild order.
public class OrderDetails
{
    // Orderns unika nummer.
    public int OrderNumber { get; set; }

    // Orderns nuvarande status.
    public string OrderStatus { get; set; } = "Packing";

    // Datum då ordern skapades.
    public DateTime OrderDate { get; set; }

    // Datum då ordern togs emot.
    public DateTime? OrderReceivedDate { get; set; }

    // Leverantörens namn.
    public string? SupplierName { get; set; }

    // Håller reda på om leverantören har bekräftat ordern. False för att det ska vara standard när ordern skapas.
    public bool SupplierConfirmed { get; set; } = false;

    // Typ av order, t.ex "Incoming Order" eller "Outgoing Order".
    public string? OrderType { get; set;}

    // Produkterna som ingår i ordern.
    public List<Product> OrderedProductsList { get; set; } = new();
}
}