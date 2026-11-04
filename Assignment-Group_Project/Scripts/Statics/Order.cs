using System;
using System.Collections.Generic;
using Assignment_Group_Project.Scripts.Objects;

public static class Order
{
    // string orderType bedtyder att man kan skriva in en string som parameter när man anropar metoden.
    // Så att man kan använda metoden flera gånger med olika strings.
    private static void CreateOrder(string orderType)
    {
        // Skapar en lista, den sparar produkterna i ordern.
        List<Product> orderedProducts = new();


        // Skapar ett Random objekt som används för att generera ett automatiskt ordernummer.
        Random random = new Random();

        // Detta nummer används som order nummer.
        int orderNumber = random.Next(10000, 99999);

        // Spara datum och tid för ordern.
        DateTime orderDate = DateTime.Now;

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
            Console.Write("Do you want to add another product? (yes/no): ");

            string answerIfStillWantToContinueOrdering = Console.ReadLine() ?? "";

            // StringComparison.OrdinalIgnoreCase läser in med både stora och små bokstäver.
            if (!answerIfStillWantToContinueOrdering.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                addingMoreProducts = false;
            }
        }



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



            // Räknar ut totalsumman för hela ordern.
            decimal totalOrderPrice = 0;



            foreach (Product product in orderedProducts)
            {
                totalOrderPrice += product.ProductPrice * product.ProductStockQuantity;
            }



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
}