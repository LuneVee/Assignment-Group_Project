using System;
using System.Collections.Generic;
using System.Text;
using Assignment_Group_Project.Scripts.Objects;

namespace Assignment_Group_Project.Scripts.Statics
{
    public static class OutgoingOrder
    {
        // public static List<Product> productList = new ();
    
    //Metoder
    public static void AddOutgoingOrder()
    {
        // Skapar objekt av Product.
        Product outgoingnewproduct = new Product();



        // Product
        // ?? "" hanterar null/inget
        // om använaren bara trycker enter blir svaret en tom string.
        Console.Write("Enter the name of the product: ");
        outgoingnewproduct.ProductName = Console.ReadLine() ?? "";



        // LÄGG TILL ANTAL*PRIS
        // Antal av produkt
        Console.Write("Amount of product: ");
        // Om det inte är siffor skriver den ut invalid amount.
        //
        // IMPLEMNETERA TRY CATCH för olika senarion.
        //
        if (!int.TryParse(Console.ReadLine(), out int amount))
        {
            Console.WriteLine("Invalid amount.");
            return;
        }
        outgoingnewproduct.ProductPrice *= amount;
        Console.WriteLine($"{amount}");




        Console.Write("Enter the product price: ");
        // konverterar string till decimal och det får inte vara mindre än 0.
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
        {
            Console.WriteLine("Invalid price.");
            return;
        }




        //Datum
        Console.Write("What date is it today? Type YYYY-MM-DD:");
        // Konverterar string till datum. Om omvandlingen misslyckas skrivs detta ut.
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime userInputDateTime))
        {
            Console.WriteLine("Ivalid date.");
            return;
        }
        // Om datumet är före dagens datum körs detta.
        if(userInputDateTime.Date > DateTime.Today)
        {
            Console.WriteLine("Todays date can not be in the future.");
            return;
        }
        outgoingnewproduct.AddOrderDate = userInputDateTime;





        if (amount > outgoingnewproduct.ProductStockQuantity)
        {
            Console.WriteLine("The amount you choose is not avaible in stock.");
            return;
        }

        outgoingnewproduct.ProductStockQuantity -= amount;
        Console.WriteLine($"{amount} {outgoingnewproduct.ProductName} has been added.");

    }
}



// public static class IncommingOrder
//     {
//         static Product incommingnewproduct = new Product();

//         Console.WriteLine("What?");

//         System.Console.WriteLine("");
    }

