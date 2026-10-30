using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Assignment_Group_Project.Scripts.Objects
{
    public class Product
    {
        //Attributer

        public int ProductArticleNumber { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductStockQuantity { get; set; }
        public string? SupplierName { get; set; }
        public DateTime AddOrderDate { get; set; } 
        // Gör en lista av framtida sökfunktion
        public List<string> tagsList = new List<string>();


        // Metod som skriver ut summering av produkterna.
        public string GetSummaryProduct()
        {
            return $"{ProductName}, Price: {ProductPrice}, Articlenumber: {ProductArticleNumber}, Amount in stock: {ProductStockQuantity}";
        }

    }
}
