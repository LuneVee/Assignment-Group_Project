using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Assignment_Group_Project.Scripts.Objects
{
    public class Product
    {
        //Attributer

        public int ArticleNumber { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int SupplierID { get; set; }


        // Metod som skriver ut summering av produkterna.
        public string GetSummaryProduct()
        {
            return $"{ProductName}, Price: {Price}, Articlenumber: {ArticleNumber}, Amount in stock: {StockQuantity}";
        }

    }
}
