using Assignment_Group_Project.Scripts.Objects;
using Assignment_Group_Project.Scripts.Utilities;
using Spectre.Console;
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

        //Storage Menu version 0.1
        public static void StorageMenu()
        {
            var StorageMenu = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("Search Product", "Add Product to Storage List", "Back to Main Menu"));

            switch (StorageMenu)
            {
                case "Search Product":
                    SearchProductInStorage();
                    break;

                case "Add Product to Storage List":
                    AddProductToStorage();
                    break;

                case "Back to Main Menu":

                    break;

            }

        }



        //Create Product version 0.1
        public static void AddProductToStorage()
        {
            var table = new Table();

            table.AddColumn("Product Name");
            table.AddColumn("Article Number");
            table.AddColumn("Stock Quantity");
            table.AddColumn("Price Per Product");
            table.AddColumn("Suppliers");

            AnsiConsole.MarkupLine("[blue]Create Product[/]");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Product Artical Number: ");
            Console.ResetColor();

            User.ReadInputInt();

            Product Existing_Artical_Number = ProductList.Find(a => a.ProductArticleNumber == User.inputInt);

            //If Article Number already exist!
            if (Existing_Artical_Number != null)
            {

                AnsiConsole.MarkupLine("[yellow]Artical number already exist![/]");
                //Console.SetCursorPosition(40, 0);
                table.AddRow(Existing_Artical_Number.ProductName.ToString(), Existing_Artical_Number.ProductArticleNumber.ToString(), Existing_Artical_Number.ProductStockQuantity.ToString(), Existing_Artical_Number.ProductPrice.ToString(), Existing_Artical_Number.SupplierName.ToString());
                AnsiConsole.Write(table);

                AnsiConsole.MarkupLine("[blue]Is this the same product that you wanted to create?[/]");

                User.AskYesOrNo();

                switch (User.inputChoice)
                {
                    default:
                        AnsiConsole.MarkupLine("[red]ERROR! Something went wrong!");
                        break;

                    case "Yes":
                        AnsiConsole.MarkupLine("[blue]Would you like to update the product info?[/]");

                        User.AskYesOrNo();

                        switch (User.inputChoice)
                        {

                            case "Yes":
                                UpdateProductInStorage();
                                break;

                            case "No":
                                Console.WriteLine("Cancelling Process!");
                                Console.ReadKey();
                                StorageMenu();
                                break;
                        }

                        Console.ReadKey();

                        break;

                    case "No":
                        Console.WriteLine("Cancelling Process!");
                        Console.ReadKey();
                        StorageMenu();
                        break;
                }

            }

            //If Article Number does not exist!
            else
            {

                Product Creating_Product_For_List = new Product();

                Creating_Product_For_List.ProductArticleNumber = User.inputInt;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Product Name: ");
                Console.ResetColor();
                User.ReadInput();
                Creating_Product_For_List.ProductName = User.input;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Product Price: ");
                Console.ResetColor();
                User.ReadInputDecimal();
                Creating_Product_For_List.ProductPrice = User.inputDecimal;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Product Stock Quantity: ");
                Console.ResetColor();
                User.ReadInputInt();
                Creating_Product_For_List.ProductStockQuantity = User.inputInt;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Product Supplier Name: ");
                Console.ResetColor();
                User.ReadInput();
                Creating_Product_For_List.SupplierName = User.input;

                while (User.input != "esc")
                {

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Product Tags (separate with space): ");
                    Console.ResetColor();
                    User.ReadInput();

                    if (User.input == "esc")
                    {
                        ProductList.Add(Creating_Product_For_List);
                        Console.WriteLine("Product has now been added.");

                        table.AddRow(Creating_Product_For_List.ProductName.ToString(), Creating_Product_For_List.ProductArticleNumber.ToString(), Creating_Product_For_List.ProductStockQuantity.ToString(), Creating_Product_For_List.ProductPrice.ToString(), Creating_Product_For_List.SupplierName.ToString());

                        Console.ReadKey();
                    }
                    else
                    {

                        List<string> Tags = User.input.Split(' ').Select(tag => tag.Trim()).ToList();

                        foreach (string tag in Tags)
                        {
                            Creating_Product_For_List.tagsList.Add(tag);
                            Console.Write($"{tag}  ");
                        }


                    }

                }

            }

        }

        //Test Update Product In Storage version 0.1
        public static void UpdateProductInStorage()
        {

            AnsiConsole.MarkupLine("[blue]What Product Would You Like To Update?[/]");
            List<string> choices = new List<string>();
            choices = ProductList.FindAll(item => item != null).Select(item => item.ProductName).ToList();

            string UpdateOptions = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(choices));


        }


        //Test Remove Product From Storage version 0.1



        // Test Search in Storage Function version 0.1
        public static void SearchProductInStorage()
        {
            AnsiConsole.MarkupLine("[blue]Test Storage[/]");

            User.inputChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .AddChoices("Search For Item", "Exit Program"));

            switch (User.inputChoice)
            {
                case "Search For Item":

                    // Implementation for searching for an item

                    //
                    var table = new Table();

                    table.AddColumn("Product Name");
                    table.AddColumn("Article Number");
                    table.AddColumn("Stock Quantity");
                    table.AddColumn("Price Per Product");
                    table.AddColumn("Suppliers");

                    foreach (var item in ProductList)
                    {

                        table.AddRow(item.ProductName.ToString(), item.ProductArticleNumber.ToString(), item.ProductStockQuantity.ToString(), item.ProductPrice.ToString(), item.SupplierName.ToString());

                    }
                    AnsiConsole.Write(table);

                    //Create search text and read input from user!
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Search: ");
                    Console.ResetColor();
                    User.ReadInput();

                    table = new Table();

                    table.AddColumn("Product Name");
                    table.AddColumn("Article Number");
                    table.AddColumn("Stock Quantity");
                    table.AddColumn("Price Per Product");
                    table.AddColumn("Suppliers");

                    //Check 1: Search if Input matches ArticleNumber exactly!
                    List<Product> Find_Item_With_The_Exact_Atrical_Number_As_User_Input = ProductList.FindAll(product => product.ProductArticleNumber.ToString() == User.input);

                    if (ProductList.Exists(item => item.ProductArticleNumber.ToString() == User.input))
                    {
                        Console.WriteLine("Found product by exact match with Article Number!");
                        foreach (var product in Find_Item_With_The_Exact_Atrical_Number_As_User_Input)
                        {
                            table.AddRow(product.ProductName.ToString(), product.ProductArticleNumber.ToString(), product.ProductStockQuantity.ToString(), product.ProductPrice.ToString(), product.SupplierName.ToString());
                        }
                        AnsiConsole.Write(table);
                    }
                    else
                    {
                        //Check 2: Search if Input matches ProductName exactly!
                        List<Product> Find_Items_With_The_Exact_Name_As_User_Inpus = ProductList.FindAll(product => product.ProductName.ToLower().Contains(User.input.ToLower())).ToList();

                        if (ProductList.Exists(item => item.ProductName.ToLower() == User.input.ToLower()))
                        {
                            Console.WriteLine("Found product by exact match with Product Name!");

                            foreach (var product in Find_Items_With_The_Exact_Name_As_User_Inpus)
                            {
                                table.AddRow(product.ProductName.ToString(), product.ProductArticleNumber.ToString(), product.ProductStockQuantity.ToString(), product.ProductPrice.ToString(), product.SupplierName.ToString());
                            }
                            AnsiConsole.Write(table);
                        }

                        else
                        {

                            //Check 3: Search if Input matches name of Product partially!
                            List<Product> Find_Items_That_Contains_Input = ProductList.FindAll(item => item.ProductName.ToLower().Contains(User.input.ToLower())).ToList();

                            if (Find_Items_That_Contains_Input.Count > 0)
                            {
                                Console.WriteLine("Found product by partial match!");

                                foreach (var product in Find_Items_That_Contains_Input)
                                {
                                    table.AddRow(product.ProductName.ToString(), product.ProductArticleNumber.ToString(), product.ProductStockQuantity.ToString(), product.ProductPrice.ToString(), product.SupplierName.ToString());
                                }
                                AnsiConsole.Write(table);
                            }

                            else
                            {

                                //Check 4: Search if Input matches any of the Product Tags!
                                List<Product> Find_Items_That_Contains_Input_In_Tags = ProductList.FindAll(item => item.tagsList.Exists(tag => tag.ToLower().Contains(User.input.ToLower()))).ToList();

                                if (Find_Items_That_Contains_Input_In_Tags.Count > 0)
                                {
                                    Console.WriteLine("Found product by tag match!");
                                    foreach (var product in Find_Items_That_Contains_Input_In_Tags)
                                    {
                                        table.AddRow(product.ProductName.ToString(), product.ProductArticleNumber.ToString(), product.ProductStockQuantity.ToString(), product.ProductPrice.ToString(), product.SupplierName.ToString());
                                    }
                                    AnsiConsole.Write(table);

                                }
                                // If no products were found!
                                else
                                {
                                    Console.WriteLine("No products found matching the search term.");
                                }

                            }

                        }

                    }
                    Console.ReadKey();
                    break;

                case "Exit Program":
                    return;
            }
        }

    }
}
