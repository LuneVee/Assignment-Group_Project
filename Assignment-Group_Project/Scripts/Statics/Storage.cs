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
            var StorageMenu = AnsiConsole
                .Prompt(new SelectionPrompt<string>()
                .AddChoices("Search Product", "Add Product to Storage List", "Update Product In Storage", "Remove Product from Storage List", "Back to Main Menu"));

            switch (StorageMenu)
            {
                case "Search Product":
                    SearchProductInStorage();
                    break;

                case "Add Product to Storage List":
                    AddProductToStorage();
                    break;

                case "Update Product In Storage":
                    UpdateProductInStorage();
                    break;

                case "Remove Product from Storage List":
                    RemoveProductFromStorage();
                    break;

                case "Back to Main Menu":
                    MainMenu.RunMenu();
                    break;

            }

        }//Back To Main Menu = Finished Running Task!


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

        }//Finished Running Task!


        //Test Update Product In Storage version 0.1
        public static void UpdateProductInStorage()
        {

            AnsiConsole.MarkupLine("[blue]What Product Would You Like To Update?[/]");
            List<string> choices = new List<string>();
            choices.Add("Search: ");
            choices = ProductList.FindAll(item => item != null).Select(item => item.ProductName).ToList();
            choices.Add("Back to Storage Menu");

            string UpdateOptions = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(choices));

            if (UpdateOptions == "Back to Storage Menu")
            {
                StorageMenu();
            }
            else
            {
                Product productToUpdate = ProductList.Find(item => item.ProductName == UpdateOptions);

                if (productToUpdate != null)
                {
                    AnsiConsole.MarkupLine($"[blue]Updating Product: {productToUpdate.ProductName}[/]");
                    Console.WriteLine("Enter new values (leave blank to keep current value):");
                    
                    Console.Write($"Product Name ({productToUpdate.ProductName}): ");
                    User.ReadInput();  
                    productToUpdate.ProductName = User.input;

                    Console.Write($"Product Price ({productToUpdate.ProductPrice}): ");
                    User.ReadInputDecimal();
                    productToUpdate.ProductPrice = User.inputDecimal;

                    Console.Write($"Product Stock Quantity ({productToUpdate.ProductStockQuantity}): "); 
                    User.ReadInputInt();
                    productToUpdate.ProductStockQuantity = User.inputInt;

                    Console.Write($"Product Supplier Name ({productToUpdate.SupplierName}): ");
                    User.ReadInput();
                    productToUpdate.SupplierName = User.input;

                    AnsiConsole.MarkupLine("[green]Product updated successfully![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Product not found![/]");
                }
            }


        }//Finished Running Task!


        //Test Remove Product From Storage version 0.1
        public static void RemoveProductFromStorage()
        {

            AnsiConsole.MarkupLine                                                                              //Display a message to the user asking which product they would like to remove!
                ("[blue]What Product Would You Like To Remove?[/]");

            // Create a list of product names for the selection prompt!
            List<string> choices = new List<string>();

            choices.Add("Search:");                                                                             //Add "Search" as the first option to the list of choices!
            foreach (var product in ProductList)                                                                   //Loop through each product in the Products list and add its name to the choices list!
            {
                if (product != null)                                                                            //Check if the product is not null before adding its name to the choices list!
                {
                    choices.Add(product.ProductName);                                                           //Add the product name to the choices list!
                }
            }
            choices.Add("Back To Main Menu");                                                                   //Add "Back To Main Menu" as the last option to the list of choices!


            //Create a selection prompt for the user to choose a product to remove!
            var ChoosenProduct = AnsiConsole
                .Prompt(new SelectionPrompt<string>()
                .AddChoices(choices));                                                                          //Add the choices list to the selection prompt!

            if (ChoosenProduct == "Back To Main Menu")                                                          //Check if the user chose "Back To Main Menu"!
            {
                return;                                                                                         //If the user chose "Back To Main Menu", exit the method and return to the main menu!

            }
            else
            {
                Product productToRemove = ProductList.Find(item => item.ProductName == ChoosenProduct);            //Find the product in the Products list that matches the chosen product name!
                if (productToRemove != null)                                                                    //Check if the product to remove is not null!
                {
                    AnsiConsole.MarkupLine                                                                      //Ask the user for confirmation before removing the product!
                        ($"[blue]Are you sure you want to remove '{productToRemove.ProductName}' from storage?[/]");

                    User.AskYesOrNo();                                                                          //Call the AskYesOrNo method to get the user's confirmation!

                    switch (User.inputChoice)                                                                   //Check the user's confirmation choice!
                    {
                        //If the user confirms the removal, remove the product from the Products list and display a success message!
                        case "Yes":

                            ProductList.Remove(productToRemove);                                                   //Remove the product from the Products list!
                            AnsiConsole.MarkupLine                                                              //Display a success message to the user!
                                ($"[green]Product '{productToRemove.ProductName}' removed successfully![/]");
                            Console.ReadKey();                                                                  //Wait for the user to press a key before continuing!
                            break;                                                                              //Exit the switch statement after removing the product!

                        //If the user cancels the removal, display a cancellation message and return to the main menu!
                        case "No":
                            AnsiConsole.MarkupLine                                                              //Display a message to the user indicating that the removal process has been cancelled!
                                ("[yellow]Product removal cancelled.[/]");
                            Console.ReadKey();                                                                  //Wait for the user to press a key before continuing!
                            return;                                                                             //Exit the method and return to the main menu!
                    }

                }
                //If the product to remove is null, display an error message to the user!
                else
                {
                    AnsiConsole.MarkupLine                                                                      //If the product to remove is null, display an error message to the user!
                        ("[red]Product not found![/]");
                    Console.ReadKey();                                                                          //Wait for the user to press a key before continuing!
                }

            }
        }//Finished Running Task!


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
        }//Finished Running Task!


    }
}
