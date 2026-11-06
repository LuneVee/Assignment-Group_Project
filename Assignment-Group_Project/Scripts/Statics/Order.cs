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
        List<Product> orderedProducts = new();


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

        Console.WriteLine("Starting Order.");

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
            orderedProducts.Add(newProduct);


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
            newOrder.OrderedProducts = orderedProducts;





            if (orderedProducts.Count == 0)
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
            decimal totalOrderPrice = orderedProducts.Sum(product => product.ProductPrice * product.ProductStockQuantity);



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

            

            foreach (Product product in orderedProducts)
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


        Console.Write("Enter order number: ");

        if (!int.TryParse(Console.ReadLine(), out int orderNumber))
        {
            Console.WriteLine("Invalid order number.");
            return;
        }



        // Söker efter en order som matchar det inskrivna ordernumret.
        // FirstOrDefault letar efter den order som matchar ordernumret.
        OrderDetails? orderToUpdate = appData.Orders.FirstOrDefault(order => order.OrderNumber == orderNumber);

        // Kontrollerar om ordern finns.
        if (orderToUpdate == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

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
                Console.WriteLine("This order cannot be updated anymore.");
            return;
        }

        // Visar en meny med de statusar som är tillåtna.
        // new SelectionPrompt<string>() Skapar en meny där användaren kan välja ett alternativ.
        // .Title sätter titeln på menyn.
        // .AddChoices(availableStatuses) Lägger till alternativen från listan availableStatuses i menyn.
        string newStatus = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Choose a new order status:").AddChoices(availableStatuses));

        // Uppdaterar orderns status.
        orderToUpdate.OrderStatus = newStatus;

        // Om ordern har statusen deliverd flyttas den till historik listan.
        if (newStatus == "Delivered")
        {
            orderToUpdate.OrderReceivedDate = DateTime.Now;

            appData.Orders.Remove(orderToUpdate);
            appData.OrderHistory.Add(orderToUpdate);

            Console.WriteLine("Order moved to order history!");
        }
        else
        {
            Console.WriteLine($"Order status updated to: {newStatus}");
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
        List<OrderDetails> selectedOrders = appData.Orders.Where(order => order.OrderType == selectedOrderType).ToList();


        // Kontrollerar om det finns order av den valda typen.
        if (selectedOrders.Count == 0)
        {
            Console.WriteLine("No orders found for this order type.");
            return;
        }


        // Visar alla order av den valda typen.
        foreach (OrderDetails order in selectedOrders)
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
        .AddChoices(selectedOrders));


        // Visar detaljer för den valda ordern.
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Order Number: {selectedOrder.OrderNumber}");
        Console.WriteLine($"Order Type: {selectedOrder.OrderType}");
        Console.WriteLine($"Order Status: {selectedOrder.OrderStatus}");
        Console.WriteLine($"Order Date: {selectedOrder.OrderDate:yyyy-MM-dd HH:mm}");

        if (!string.IsNullOrEmpty(selectedOrder.SupplierName))
        {
            Console.WriteLine($"Supplier: {selectedOrder.SupplierName}");
        }

        Console.WriteLine("Ordered Products:");

        foreach (Product product in selectedOrder.OrderedProducts)
        {
            // F2 gör att priset visas med två decimaler.
            Console.WriteLine($" - Product Name: {product.ProductName}, Amount: {product.ProductStockQuantity}, Price per Product: {product.ProductPrice:F2} kr");
        }
        Console.WriteLine("-----------------------------------");



        string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title("What would you like to do?")
        .AddChoices("Mark as Delivered", "Go Back"));

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

    // Typ av order, t.ex "Incoming Order" eller "Outgoing Order".
    public string? OrderType { get; set;}

    // Produkterna som ingår i ordern.
    public List<Product> OrderedProducts { get; set; } = new();
}
}