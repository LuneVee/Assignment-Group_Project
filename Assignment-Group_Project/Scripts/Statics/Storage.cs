using Assignment_Group_Project.Scripts.Objects;
using Assignment_Group_Project.Scripts.Utilities;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Assignment_Group_Project.Scripts.Statics
{
    static class Storage
    {

        //static List<Product> ProductList = new List<Product>()
        //{
        //    // Example product in List!
        //    new Product()
        //    {
        //        ProductArticleNumber = 1,
        //        ProductName = "Product 1",
        //        ProductPrice = 10.99m,
        //        ProductStockQuantity = 100,
        //        SupplierName = "Supplier A",
        //        AddOrderDate = DateTime.Now.AddDays(-5),
        //        tagsList = new List<string> { "tag1", "tag2", "etsetra" },
        //    },

        //};

        //Load from 'Products.json' file!

        private static List<Product> Products = DataStorage.Load<List<Product>>("Products.json");
        private static List<string> ProductHistory = DataStorage.Load<List<string>>("ProductHistory.json");

        private static void Load()
        {
            Products = DataStorage.Load<List<Product>>("Products.json");
            ProductHistory = DataStorage.Load<List<string>>("ProductHistory.json");
        }

        //Storage Menu version 0.1
        public static void StorageMenu()
        {
            Console.Clear();

            //Create Menu!

            Write.StorageMenu();

            var StorageMenu = AnsiConsole
                .Prompt(new SelectionPrompt<string>()
                .AddChoices
                ("Search Product", "Add Product to Storage List", "Update Product In Storage", "Remove Product from Storage List", "Storage History", "Back to Main Menu"));

            switch (StorageMenu)
            {
                case "Search Product":
                    SearchProductInStorage();
                    break;

                case "Add Product to Storage List":
                    AddProduct();
                    break;

                case "Update Product In Storage":
                    UpdateProductInStorage();
                    break;

                case "Remove Product from Storage List":
                    RemoveProductFromStorage();
                    break;

                case "Storage History":
                    CheckStorageHistory();
                    break;

                case "Back to Main Menu":
                    return;

            }
        }//Back To Main Menu = Finished Running Task!


        //Create Product version 0.1
        public static void AddProduct()
        {

            //Load Products
            Load();

            var Products = appData.Products;

            bool LoopTask = true;

            while (LoopTask == true)
            {

                //
                var table = new Table();

                table.AddColumn("Product     ");                                                            //Product Name!
                table.AddColumn("Artical ");                                                                //Artical Number!
                table.AddColumn("Quantity");                                                                //Stock Quantity!
                table.AddColumn("Price       ");                                                            //Price Per Product!
                table.AddColumn("Company     ");                                                            //Company Suppliers!

                Console.Clear();

                //For index 0 to 9 in Products, add to table row!
                for (int i = 0; i < Math.Min(Products.Count, 10); i++)
                {
                    table.AddRow(Products[i].ProductName.ToString(), Products[i].ProductArticleNumber.ToString(), Products[i].ProductStockQuantity.ToString(), Products[i].ProductPrice.ToString(), Products[i].SupplierName.ToString());
                }

                Write.CreatingProduct();

                //
                User.cursorPosition = new Vector2(Console.CursorLeft, Console.CursorTop);
                Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));
                AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));

                //
                Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));
                AnsiConsole.MarkupLine("[blue] Create Artical Number\n then Create Product![/]\n");
                try
                {
                    User.inputInt = int.Parse(User.ReadLineAsIntRange("[cyan]  Enter a Artical Number[/]:", 7));
                }
                catch
                {
                    return;
                }


                Product? Existing_Artical_Number = Products.Find(a => a.ProductArticleNumber == User.inputInt);

                //If Article Number already exist!
                if (Existing_Artical_Number != null)
                {
                    Console.SetCursorPosition(((int)Console.WindowWidth - 70), 4);

                    //
                    for (int i = 0; i < 30; i++)
                    {
                        try
                        {
                            for (int s = 0; s < 70; s++)
                            {
                                AnsiConsole.Markup(" ");
                            }
                            Console.SetCursorPosition(((int)Console.WindowWidth - 70), Console.CursorTop + 1);
                        }
                        catch
                        {
                            break;
                        }
                    }

                    Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 1));

                    //Clear table!
                    table = new Table();

                    table.AddColumn("Product     ");                                                            //Product Name!
                    table.AddColumn("Artical ");                                                                //Artical Number!
                    table.AddColumn("Quantity");                                                                //Stock Quantity!
                    table.AddColumn("Price       ");                                                            //Price Per Product!
                    table.AddColumn("Company     ");                                                            //Company Suppliers!


                    //Add Product and show its info!
                    table
                        .AddRow(Existing_Artical_Number.ProductName.ToString(),
                        Existing_Artical_Number.ProductArticleNumber.ToString(),
                        Existing_Artical_Number.ProductStockQuantity.ToString(),
                        Existing_Artical_Number.ProductPrice.ToString(),
                        Existing_Artical_Number.SupplierName.ToString());

                    //Create and write Table Window on the right side!
                    AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));


                    Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 3));

                    AnsiConsole.MarkupLine("[yellow]  Artical number already exist![/]\n");

                    AnsiConsole.MarkupLine("[blue]  Is this the product that you wanted to create?[/]");

                    User.AskYesOrNo();

                    switch (User.inputChoice)
                    {
                        default:
                            AnsiConsole.MarkupLine("[red]  ERROR! Something went wrong!");
                            break;

                        case "Yes":
                            Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 5));
                            for (int i = 0; i < 48; i++)
                            {
                                AnsiConsole.Markup(" ");
                            }
                            Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 5));
                            AnsiConsole.MarkupLine("[blue]  Would you like to update the product info?[/]");

                            User.AskYesOrNo();

                            switch (User.inputChoice)
                            {

                                case "Yes":
                                    UpdateProductInStorage();
                                    LoopTask = false;
                                    break;

                                case "No":
                                    Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 5));
                                    for (int i = 0; i < 48; i++)
                                    {
                                        AnsiConsole.Markup(" ");
                                    }
                                    Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 5));
                                    AnsiConsole.MarkupLine("[yellow] Cancelling Process!\n Please chose another [/] [cyan]Artical Number![/]");
                                    Console.ReadKey();
                                    break;
                            }
                            break;

                        case "No":
                            AnsiConsole.MarkupLine("[yellow] Cancelling Process!\n Please chose another [/] [cyan]Artical Number![/]");
                            Console.ReadKey();
                            break;
                    }

                }

                //If Article Number does not exist, Create new Product!
                else
                {
                    CreateNewProduct();
                    LoopTask = false;
                }
            }
        }


        //Create Product To Storage version 0.1
        public static void CreateNewProduct()
        {
            //Load Products
            DataStorage dataStorage = new DataStorage();
            AppData appData = dataStorage.Load();

            var Products = appData.Products;

            AddToStorageHistory(User.inputInt.ToString(), "Creating");

            //Create and set defualt values on Creating_Product_For_List!
            Product Creating_Product_For_List = new Product();                                              //Create as a Product ref!

            Creating_Product_For_List.ProductName = "";                                                            //Set Product Name!
            Creating_Product_For_List.ProductArticleNumber = User.inputInt;                                        //Set Product Artical Number!
            Creating_Product_For_List.ProductStockQuantity = 0;                                                    //Set Product Stock Quantity!
            Creating_Product_For_List.ProductPrice = 0;                                                            //Set Product Price!
            Creating_Product_For_List.SupplierName = "";                                                        //Set Product Supplier!

            //Dictionary!
            Dictionary<string, bool> Product_Info_That_Is_Created = new Dictionary<string, bool>()          //A Dictionary of Conditions!
            {

                ["Name"] = false,
                ["Quantity"] = false,
                ["Price"] = false,
                ["Company"] = false,

            };

            //Create Product!
            while (true)
            {
                Console.Clear();                                                                            //Clear Console!

                Write.CreatingProduct();                                                                    //Create Product Title Text!

                //Clear and create new Table!
                var table = new Table();                                                                    //Create table. If table already exist so will this overwrite the old one!

                table.AddColumn("Product     ");                                                            //Product Name!
                table.AddColumn("Artical ");                                                                //Artical Number!
                table.AddColumn("Quantity");                                                                //Stock Quantity!
                table.AddColumn("Price       ");                                                            //Price Per Product!
                table.AddColumn("Company     ");                                                            //Company Suppliers!

                table.                                                                                      //
                    AddRow(Creating_Product_For_List.ProductName.ToString(),                                       //
                    Creating_Product_For_List.ProductArticleNumber.ToString(),                                     //
                    Creating_Product_For_List.ProductStockQuantity.ToString(),                                     //
                    Creating_Product_For_List.ProductPrice.ToString(),                                             //
                    Creating_Product_For_List.SupplierName.ToString());                                         //

                //Create and write Table Window on the right side!
                AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));                             //Create and write Table Window on the right side!

                //Print out All tags from Creating_Product_For_List!
                foreach (string Tag in Creating_Product_For_List.tagsList)                                  //
                {

                    Console.SetCursorPosition(Console.WindowWidth - 64, Console.CursorTop++ + 1);           //
                    AnsiConsole.Markup($"{Tag}, ");                                                         //

                }

                //Set Cursor Position!
                Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 4));  //

                var Create_Product_Options = AnsiConsole                                                    //
                    .Prompt(new SelectionPrompt<string>()                                                   //
                    .AddChoices                                                                             //
                    ("Product Name", "Product Price", "Product Stock Quantity", "Product Supplier Name", "Add Tag", "Confirm Product", "Cancel Product"));

                //
                switch (Create_Product_Options)                                                             //
                {
                    case "Product Name":                                                                    //
                        User.ReadInput();                                                                   //Read User Input!
                        Creating_Product_For_List.ProductName = User.input;                                        //Set Product_Info_That_Is_Created Product Name to the same as User Input! 
                        Product_Info_That_Is_Created["Name"] = true;                                        //Set Condition to true!
                        break;

                    case "Product Price":                                                                   //
                        User.ReadInputInt();                                                               //
                        Creating_Product_For_List.ProductPrice = User.inputInt;                                    //
                        Product_Info_That_Is_Created["Price"] = true;                                       //
                        break;

                    case "Product Stock Quantity":                                                          //
                        User.ReadInputInt();                                                               //
                        Creating_Product_For_List.ProductStockQuantity = User.inputInt;                            //
                        Product_Info_That_Is_Created["Quantity"] = true;                                    //
                        break;

                    case "Product Supplier Name":                                                           //
                        User.ReadInput();                                                                   //
                        Creating_Product_For_List.SupplierName = User.input;                                    //
                        Product_Info_That_Is_Created["Company"] = true;                                     //
                        break;

                    case "Add Tag":                                                                         //
                        User.ReadInput();                                                                   //
                        Creating_Product_For_List.tagsList.Add(User.input);                                 //
                        break;

                    case "Confirm Product":                                                                 //

                        //Check if all Conditions are met!
                        int Valid_Bools = 0;                                                                //                   

                        foreach (bool Valid in Product_Info_That_Is_Created.Values)                         //
                        {
                            if (Valid == true)                                                              //
                            {
                                Valid_Bools++;                                                              //
                            }
                        }

                        //If Conditions are met!
                        if (Valid_Bools >= Product_Info_That_Is_Created.Count)                              //
                        {
                            Products.Add(Creating_Product_For_List);                                        //

                            //
                            AnsiConsole.MarkupLine("[green]Product has now been added![/]");                //

                            // Sparar all data och den nya produkten i data.json.
                            dataStorage.Save(appData);

                            AddToStorageHistory(Creating_Product_For_List.ProductName, "Added");                   //
                            Console.ReadKey();                                                              //
                            return;
                        }
                        //If Conditions are NOT met!
                        else
                        {
                            Console.SetCursorPosition                                                       //
                                (((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 3));
                            AnsiConsole.MarkupLine("[red] Not Valid![/]");                                  //
                            Console.ReadKey();                                                              //
                        }
                        break;

                    case "Cancel Product":                                                                  //
                        AddToStorageHistory                                                                 //
                            (Creating_Product_For_List.ProductArticleNumber.ToString(), "Canceled");

                        // Sparar all data och den nya produkten i data.json.
                        dataStorage.Save(appData);

                        AnsiConsole.MarkupLine("[yellow] Creating Product has been Canceled![/]");          //
                        Console.ReadKey();                                                                  //
                        return;

                }
            }
        }//Finished Running Task!


        //Test Update Product In Storage version 0.1
        public static void UpdateProductInStorage()
        {
            Load();

            AnsiConsole.MarkupLine("[blue]What Product Would You Like To Update?[/]");
            List<string> choices = new List<string>();
            choices.Add("Search: ");
            choices = Products.FindAll(item => item != null).Select(item => item.ProductName).ToList();
            choices.Add("Back to Storage Menu");

            string UpdateOptions = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(choices));

            if (UpdateOptions == "Back to Storage Menu")
            {
                StorageMenu();
            }
            else
            {
                Product productToUpdate = Products.Find(item => item.ProductName == UpdateOptions);

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

                    // Sparar all data och den nya produkten i data.json.
                    DataStorage.Save("ProductList.json", Products);

                    //
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
            foreach (var product in Products)                                                                   //Loop through each product in the Products list and add its name to the choices list!
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
                Product productToRemove = Products.Find(item => item.ProductName == ChoosenProduct);            //Find the product in the Products list that matches the chosen product name!
                if (productToRemove != null)                                                                    //Check if the product to remove is not null!
                {
                    AnsiConsole.MarkupLine                                                                      //Ask the user for confirmation before removing the product!
                        ($"[blue]Are you sure you want to remove '{productToRemove.ProductName}' from storage?[/]");

                    User.AskYesOrNo();                                                                          //Call the AskYesOrNo method to get the user's confirmation!

                    switch (User.inputChoice)                                                                   //Check the user's confirmation choice!
                    {
                        //If the user confirms the removal, remove the product from the Products list and display a success message!
                        case "Yes":

                            Products.Remove(productToRemove);                                                   //Remove the product from the Products list!
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

            //Load Products
            Load();

            //
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

                    foreach (var item in Products)
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
                    List<Product> Find_Item_With_The_Exact_Atrical_Number_As_User_Input = Products.FindAll(product => product.ProductArticleNumber.ToString() == User.input);

                    if (Products.Exists(item => item.ProductArticleNumber.ToString() == User.input))
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
                        List<Product> Find_Items_With_The_Exact_Name_As_User_Inpus = Products.FindAll(product => product.ProductName.ToLower().Contains(User.input.ToLower())).ToList();

                        if (Products.Exists(item => item.ProductName.ToLower() == User.input.ToLower()))
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
                            List<Product> Find_Items_That_Contains_Input = Products.FindAll(item => item.ProductName.ToLower().Contains(User.input.ToLower())).ToList();

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
                                List<Product> Find_Items_That_Contains_Input_In_Tags = Products.FindAll(item => item.tagsList.Exists(tag => tag.ToLower().Contains(User.input.ToLower()))).ToList();

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


        //
        public static void AddToStorageHistory(string What_Product_Is_It, string What_Condition_It_Was_Added)
        {

            //Load Products
            Load();

            DateTime When_This_Got_Added_To_History = DateTime.Now;



            string PushText = ($"{When_This_Got_Added_To_History} | {What_Product_Is_It} | {What_Condition_It_Was_Added}");

            ProductHistory.Add(PushText);
            DataStorage.Save("ProductHistory.json", Products);



        }


        //
        public static void CheckStorageHistory()
        {
            while (true)
            {

                //Load Products
                Load();

                Console.Clear();

                //Write.ProductHistory();

                List<string> History = ProductHistory.AsEnumerable().Reverse().ToList();                               //

                var table = new Table();

                table.AddColumn("Storage History!");

                int foreachTime = 0;
                foreach (string History_Action in History)
                {
                    if (foreachTime <= 10)
                    {
                        table.AddRow(History_Action);
                    }
                    else
                    {
                        break;
                    }

                }

                //Create and write Table Window on the right side!
                AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));

                Console.SetCursorPosition(0, 4);

                var Option = AnsiConsole
                    .Prompt(new SelectionPrompt<string>().AddChoices("Search: ", "Back To Storage Menu"));

                switch (Option)
                {
                    case "Search: ":


                        break;

                    case "Back To Storage Menu":
                        return;

                }
            }
        }

    }
}
