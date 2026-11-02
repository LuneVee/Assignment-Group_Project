using Assignment_Group_Project.Scripts.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_Group_Project.Scripts.Statics
{
    static class Storage
    {

        static List<Product> ProductList = new List<Product>()
        {
            // Example product in List!
            new Product()
            {
                ProductArticleNumber = 1,
                ProductName = "Product 1",
                ProductPrice = 10.99m,
                ProductStockQuantity = 100,
                SupplierName = "Supplier A",
                AddOrderDate = DateTime.Now.AddDays(-5),
                tagsList = new List<string> { "tag1", "tag2", "etsetra" },
            },

        };

    }
}
