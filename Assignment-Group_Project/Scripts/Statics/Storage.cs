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

        //Load from 'Products.json' file!
        //private static List<Product> Products = Load<Product>("data.json");
        private static List<Product> Products = new List<Product>();
        //private static List<ProductHistory> ProductHistoryRef = Load<ProductHistory>("data.json");
        private static List<ProductHistory> ProductHistoryRef = new List<ProductHistory>();

        //Load
        //private static List<T> Load<T>(string json_File_Name)
        //{
        //    var Variable = DataStorage.Load<List<T>>(json_File_Name);
        //    return Variable;

        //}

        //Storage Menu version 0.1
        public static void StorageMenu()
        {
            while (true)
            {

                Console.Clear();

                //Create Menu!

                Write.StorageMenu();

                var StorageMenu = AnsiConsole
                    .Prompt(new SelectionPrompt<string>()
                    .AddChoices
                    ("Search Product",
                    "Add Product to Storage List",
                    "Update Product In Storage",
                    "Remove Product from Storage List",
                    "Storage History",
                    "Back to Main Menu"));

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

            }
        }//Back To Main Menu = Finished Running Task!


        //Create Product version 0.1
        public static void AddProduct()
        {

            //Load Products
            AppData appData = DataStorage.Load<AppData>("data.json");
            var Products = appData.Products;

            bool LoopTask = true;

            while (LoopTask == true)
            {

                //
                var table = new Table();

                table.AddColumn("Product     ");                                                            //Product Name!
                table.AddColumn("Artical ");                                                                //Artical Number!
                //table.AddColumn("Quantity");                                                                //Stock Quantity!
                //table.AddColumn("Price       ");                                                            //Price Per Product!
                table.AddColumn("Company     ");                                                            //Company Suppliers!

                Console.Clear();

                //For index 0 to 9 in Products, add to table row!
                for (int i = 0; i < Math.Min(Products.Count, 10); i++)
                {
                    table.AddRow(
                        Products[i].ProductName!.ToString(),
                        Products[i].ProductArticleNumber.ToString(),
                        //Products[i].ProductStockQuantity.ToString(),
                        //Products[i].ProductPrice.ToString(),
                        Products[i].SupplierName!.ToString()
                        );
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
                    User.inputInt = int.Parse(User.ReadLineAsIntRange("[cyan]  Enter a Artical Number[/]:", 7)!);
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

                    ClearRightSideOfWindow();

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
                        .AddRow(Existing_Artical_Number.ProductName!.ToString(),
                        Existing_Artical_Number.ProductArticleNumber.ToString(),
                        Existing_Artical_Number.ProductStockQuantity.ToString(),
                        Existing_Artical_Number.ProductPrice.ToString(),
                        Existing_Artical_Number.SupplierName!.ToString());

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
            AppData appData = DataStorage.Load<AppData>("data.json");
            var Products = appData.Products;

            //Create and set defualt values on Creating_Product_For_List!
            Product Creating_Product_For_List = new Product();                                              //Create as a Product ref!

            Creating_Product_For_List.ProductName = "";                                                     //Set Product Name!
            Creating_Product_For_List.ProductArticleNumber = User.inputInt;                                 //Set Product Artical Number!
            Creating_Product_For_List.ProductStockQuantity = 0;                                             //Set Product Stock Quantity!
            Creating_Product_For_List.ProductPrice = 0;                                                     //Set Product Price!
            Creating_Product_For_List.SupplierName = "";                                                    //Set Product Supplier!

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
                    AddRow(Creating_Product_For_List.ProductName.ToString(),                                //
                    Creating_Product_For_List.ProductArticleNumber.ToString("D7"),                          //
                    Creating_Product_For_List.ProductStockQuantity.ToString(),                              //
                    Creating_Product_For_List.ProductPrice.ToString(),                                      //
                    Creating_Product_For_List.SupplierName.ToString());                                     //

                //Create and write Table Window on the right side!
                AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));                             //Create and write Table Window on the right side!

                //Print out All tags from Creating_Product_For_List!
                foreach (string Tag in Creating_Product_For_List.tagsList)                                  //
                {

                    Console.SetCursorPosition(Console.WindowWidth - 64, Console.CursorTop++ + 1);           //
                    AnsiConsole.Markup($"{Tag}, ");                                                         //

                }

                //Set Cursor Position!
                Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 5));  //

                var Create_Product_Options = AnsiConsole                                                    //
                    .Prompt(new SelectionPrompt<string>()                                                   //
                    .AddChoices                                                                             //
                    ("Product Name", "Product Price", "Product Stock Quantity", "Product Supplier Name", "Add Tag", "Remove Tag", "Confirm Product", "Cancel Product"));

                //
                switch (Create_Product_Options)                                                             //
                {
                    //Product Name!
                    case "Product Name":                                                                    //
                        User.input = User.ReadLineRange("[cyan]  Set Name:[/]", 30);                        //Read User Input!
                        try
                        {
                            if (User.input == null)
                            {
                                break;
                            }
                            Creating_Product_For_List.ProductName = User.input;                             //Set Product_Info_That_Is_Created Product Name to the same as User Input! 
                            Product_Info_That_Is_Created["Name"] = true;                                    //Set Condition to true!
                        }
                        catch
                        {
                            AnsiConsole.MarkupLine("[red]  Invalid input![/]");                             //Write this text if input is invalid!
                            Product_Info_That_Is_Created["Name"] = false;                                   //Set Condition to false!
                            Console.ReadKey();                                                              //
                        }
                        break;

                    //Product Price!
                    case "Product Price":                                                                   //
                        User.input = User.ReadLineAsDecimalRange("[cyan]  Set Price:[/]", 20);              //
                        try
                        {
                            if (User.input == null)
                            {
                                break;
                            }
                            Creating_Product_For_List.ProductPrice = decimal.Parse(User.input);             //
                            Product_Info_That_Is_Created["Price"] = true;                                   //
                        }
                        catch
                        {
                            AnsiConsole.MarkupLine("[red]  Invalid input![/]");                             //
                            Product_Info_That_Is_Created["Price"] = false;                                  //
                            Console.ReadKey();
                        }
                        break;

                    //Product Stock Quantity!
                    case "Product Stock Quantity":                                                          //
                        int.TryParse(User.ReadLineAsIntRange("[cyan]  Set Quantity:[/]", 20), out User.inputInt); //
                        try
                        {
                            if (User.inputInt == null)
                            {
                                break;
                            }
                            Creating_Product_For_List.ProductStockQuantity = User.inputInt;                 //
                            Product_Info_That_Is_Created["Quantity"] = true;                                //
                        }
                        catch
                        {
                            AnsiConsole.MarkupLine("[red]  Invalid input![/]");                             //
                            Product_Info_That_Is_Created["Quantity"] = false;                               //
                            Console.ReadKey();
                        }
                        break;

                    //Product Supplier Name!
                    case "Product Supplier Name":                                                           //
                        User.input = User.ReadLineRange("[cyan]  Set Company Name:[/]", 30);                          //Read User Input!
                        try
                        {
                            if (User.input == null)
                            {
                                break;
                            }
                            Creating_Product_For_List.SupplierName = User.input;                            //Set Product_Info_That_Is_Created Product Name to the same as User Input! 
                            Product_Info_That_Is_Created["Company"] = true;                                 //Set Condition to true!
                        }
                        catch
                        {
                            AnsiConsole.MarkupLine("[red]  Invalid input![/]");                             //Write this text if input is invalid!
                            Product_Info_That_Is_Created["Company"] = false;                                //Set Condition to false!
                            Console.ReadKey();
                        }
                        break;

                    //Add Tag!
                    case "Add Tag":                                                                         //
                        User.input = User.ReadLineRange("[cyan]  Add Tag:[/]", 30);                         //
                        if (User.input == null)
                        {
                            break;
                        }
                        Creating_Product_For_List.tagsList.Add(User.input);                                 //
                        break;

                    //Remove Tag!
                    case "Remove Tag":

                        //
                        List<string> Active_Tags = new List<string>();

                        foreach (string Tag in Creating_Product_For_List.tagsList)
                        {

                            Active_Tags.Add(Tag);

                        }

                        // Skapar en panel som visar produkterna.
                        var TagPanel = new Panel(string.Join("\n", Active_Tags.AsEnumerable().Reverse())).Header(" TAGS # ").Border(BoxBorder.Rounded);

                        //Create and write Table Window on the right side!
                        Console.SetCursorPosition(0, 4);
                        AnsiConsole.Write(new Align(TagPanel, HorizontalAlignment.Right));
                        Console.SetCursorPosition(0, 4);

                        Active_Tags.Add("Back");

                        Create_Product_Options = AnsiConsole
                            .Prompt(new SelectionPrompt<string>()
                            .AddChoices
                            (Active_Tags));

                        //
                        switch (Create_Product_Options)
                        {
                            //
                            case "Back":
                                break;

                            default:
                                Active_Tags.Remove(Active_Tags.Last());
                                int choice = Active_Tags.FindIndex(tag => tag == Create_Product_Options);
                                Active_Tags.Remove(Active_Tags[choice]);
                                Creating_Product_For_List.tagsList = Active_Tags;
                                break;
                        }

                        break;


                    //Confirm Product!
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

                            // Save into .json!
                            appData.Products = Products;
                            DataStorage.Save("data.json", appData);


                            AddToStorageHistory                                                             //
                                (Creating_Product_For_List.ProductName,
                                Creating_Product_For_List.ProductArticleNumber,
                                Creating_Product_For_List.SupplierName,
                                ProductHistory.Status.Added);

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

                    //Cancel Product!
                    case "Cancel Product":                                                                  //

                        Creating_Product_For_List.ProductName = "";                                         //
                        Creating_Product_For_List.SupplierName = "";                                        //

                        AddToStorageHistory                                                                 //
                            (Creating_Product_For_List.ProductName,
                            Creating_Product_For_List.ProductArticleNumber,
                            Creating_Product_For_List.SupplierName,
                            ProductHistory.Status.Cancelled);

                        // Save into .json!
                        appData.Products = Products;
                        DataStorage.Save("data.json", appData);

                        AnsiConsole.MarkupLine("[yellow] Creating Product has been Canceled![/]");          //
                        Console.ReadKey();                                                                  //
                        return;

                }
            }
        }//Finished Running Task!


        //Test Update Product In Storage version 0.1
        public static void UpdateProductInStorage()
        {
            //Load Products
            AppData appData = DataStorage.Load<AppData>("data.json");
            var Products = appData.Products;

            Console.Clear();

            Write.UpdateProduct();

            CreateProductTabelUI();

            User.cursorPosition = new Vector2(1, 3);

            //
            Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y + 1));

            AnsiConsole.MarkupLine("[blue] What Product Would You Like To Update?[/]\n");
            List<string> choices = new List<string>();
            //choices.Add("Search: ");
            foreach (var choice in Products)
            {

                choices.Add($"{choice.ProductName}");

            }

            //choices = Products.FindAll(item => item != null).Select(item => item.Name).ToList();
            choices.Add("Back");

            string UpdateOptions = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(choices));

            if (UpdateOptions == "Back")
            {
                return;
            }
            else
            {
                Product productToUpdate = Products.Find(item => item.ProductName == UpdateOptions)!;
                if (productToUpdate != null)
                {

                    void CreateTable()
                    {
                        //
                        Console.Clear();

                        //
                        Write.UpdateProduct();

                        //
                        var table = new Table();

                        table.AddColumn("Product     ");                                                            //Product Name!
                        table.AddColumn("Artical ");                                                                //Artical Number!
                        table.AddColumn("Quantity");                                                                //Stock Quantity!
                        table.AddColumn("Price       ");                                                            //Price Per Product!
                        table.AddColumn("Company     ");                                                            //Company Suppliers!

                        //
                        table
                            .AddRow(productToUpdate.ProductName!.ToString(),
                            productToUpdate.ProductArticleNumber.ToString(),
                            productToUpdate.ProductStockQuantity.ToString(),
                            productToUpdate.ProductPrice.ToString(),
                            productToUpdate.SupplierName!.ToString());

                        //
                        User.cursorPosition = new Vector2(Console.CursorLeft, Console.CursorTop);
                        Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));
                        AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));

                    }

                    bool RunUpdateProduct = true;
                    while (RunUpdateProduct == true)
                    {

                        CreateTable();

                        //
                        Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));

                        string UpdateValue = AnsiConsole
                            .Prompt(new SelectionPrompt<string>()
                            .AddChoices("Name", "Price", "Quantity", "Confirm", "Back To Storage Page"));

                        switch (UpdateValue)
                        {
                            case "Name":
                                User.input = productToUpdate.ProductName;
                                try
                                {
                                    //
                                    CreateTable();
                                    Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));
                                    User.input = User.ReadLineRange("[cyan]  Name: [/]", 60);
                                    if (User.input == null)
                                    {
                                        User.input = productToUpdate.ProductName;
                                    }
                                    else
                                    {
                                        productToUpdate.ProductName = User.input;
                                    }

                                }
                                catch
                                {

                                }
                                break;

                            case "Artical Number":
                                User.inputInt = productToUpdate.ProductArticleNumber;
                                try
                                {
                                    //
                                    CreateTable();
                                    Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));
                                    User.inputInt = int.Parse(User.ReadLineAsIntRange("[cyan]  Artical number: [/]", 7)!);
                                    productToUpdate.ProductArticleNumber = User.inputInt;
                                }
                                catch
                                {

                                }
                                break;

                            case "Price":
                                User.inputDecimal = productToUpdate.ProductPrice;
                                try
                                {
                                    //
                                    CreateTable();
                                    Console.SetCursorPosition(
                                        ((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));
                                    User.inputDecimal = decimal.Parse(User.ReadLineAsDecimalRange("[cyan]  Price: [/]", 9)!);
                                    productToUpdate.ProductPrice = User.inputDecimal;
                                }
                                catch
                                {

                                }
                                break;

                            case "Quantity":
                                User.inputInt = productToUpdate.ProductStockQuantity;
                                try
                                {
                                    //
                                    CreateTable();
                                    Console.SetCursorPosition(
                                        ((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));
                                    User.inputInt = int.Parse(User.ReadLineAsIntRange("[cyan]  Quantity: [/]", 9)!);
                                    productToUpdate.ProductStockQuantity = User.inputInt;
                                }
                                catch
                                {

                                }
                                break;

                            //Not yet in use!
                            case "Size":


                                break;

                            //Not yet in use!
                            case "Add Tags":

                                break;

                            //Not yet in use!
                            case "Remove Tags":

                                break;

                            case "Confirm":

                                int Item = Products                                                             //
                                    .FindIndex(product => product.ProductArticleNumber == productToUpdate.ProductArticleNumber);
                                Products[Item] = productToUpdate;                                               //

                                // Save into .json!
                                appData.Products = Products;                                                    //
                                DataStorage.Save("data.json", appData);                                         //

                                AddToStorageHistory(                                                            //
                                    Products[Item].ProductName!,
                                    Products[Item].ProductArticleNumber,
                                    Products[Item].SupplierName!,
                                    ProductHistory.Status.Updated);

                                return;

                            case "Back To Storage Page":
                                RunUpdateProduct = false;
                                break;

                        }
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Product not found![/]");
                    Console.ReadKey();
                }

                return;
            }


        }//Finished Running Task!


        //Test Remove Product From Storage version 0.1
        public static void RemoveProductFromStorage()
        {

            //Load Products
            AppData appData = DataStorage.Load<AppData>("data.json");                                           //
            var Products = appData.Products;                                                                    //

            //Creating Menu!
            Console.Clear();                                                                                    //Clear Console Window!

            Write.RemoveProduct();                                                                              //Create RemoveProduct ASCII Text!

            AnsiConsole.MarkupLine                                                                              //Display a message to the user asking which product they would like to remove!
                ("[blue]What Product Would You Like To Remove?[/]");

            // Create a list of product names for the selection prompt!
            List<string> choices = new List<string>();                                                          //Create a List<string> to create a list of "choices"!

            /*choices.Add("Search:");*/                                                                             //Add "Search" as the first option to the list of choices!
            foreach (var product in Products)                                                                   //Loop through each product in the Products list and add its name to the choices list!
            {
                if (product != null)                                                                            //Check if the product is not null before adding its name to the choices list!
                {
                    choices.Add(product.ProductName!);                                                           //Add the product name to the choices list!
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
                Product productToRemove = Products.Find(item => item.ProductName == ChoosenProduct)!;            //Find the product in the Products list that matches the chosen product name!
                if (productToRemove != null)                                                                    //Check if the product to remove is not null!
                {
                    AnsiConsole.MarkupLine                                                                      //Ask the user for confirmation before removing the product!
                        ($"[blue]Are you sure you want to remove '{productToRemove.ProductName}' from storage?[/]");

                    User.AskYesOrNo();                                                                          //Call the AskYesOrNo method to get the user's confirmation!

                    switch (User.inputChoice)                                                                   //Check the user's confirmation choice!
                    {
                        //If the user confirms the removal, remove the product from the Products list and display a success message!
                        case "Yes":

                            //Remove Item!
                            Products.Remove(productToRemove);                                                   //Remove the product from the Products list!
                            appData.Products = Products;                                                        //Set appData's Products List Value as methods Products value!

                            //Save Changes!
                            DataStorage.Save("data.json", appData);                                             //

                            AddToStorageHistory(                                                                //
                                productToRemove.ProductName!,
                                productToRemove.ProductArticleNumber,
                                productToRemove.SupplierName!,
                                ProductHistory.Status.Removed);

                            //Finale line bofore finishing method!
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
            AppData appData = DataStorage.Load<AppData>("data.json");
            var Products = appData.Products;

            //
            Write.StorageMenu();

            var table = new Table();

            table.AddColumn("Product Name");
            table.AddColumn("Article Number");
            //table.AddColumn("Stock Quantity");
            //table.AddColumn("Price Per Product");
            table.AddColumn("Suppliers");

            foreach (var item in Products)
            {

                table.AddRow(item.ProductName!.ToString(), item.ProductArticleNumber.ToString(), /*item.ProductStockQuantity.ToString(),*//* item.ProductPrice.ToString(),*/ item.SupplierName!.ToString());

            }
            Console.SetCursorPosition(0, 4);
            AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));
            Console.SetCursorPosition(0, 4);


            List<string> choices = new List<string>();
            choices.Add("Search:");
            //foreach (var choice in Products)
            //{

            //    choices.Add($"{choice.ProductName}");

            //}

            //choices = Products.FindAll(item => item != null).Select(item => item.Name).ToList();
            choices.Add("Back To Storage");

            User.inputChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(choices));

            //
            //User.inputChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            //    .AddChoices("Search:", "Back To Storage"));

            switch (User.inputChoice)
            {
                case "Search:":

                    //
                    Console.Clear();

                    Write.StorageMenu();

                    Console.SetCursorPosition(0, 4);
                    AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));
                    Console.SetCursorPosition(0, 4);

                    //Create search text and read input from user!
                    User.input = User.ReadLineRange("[cyan]  Search: [/]", 40);
                    if (User.input == null)
                    {
                        break;
                    }

                    table = new Table();

                    table.AddColumn("Name                ");
                    table.AddColumn("Article ");
                    table.AddColumn("Quantity");
                    table.AddColumn("Price     ");
                    table.AddColumn("Suppliers           ");

                    //Check 1: Search if Input matches ArticleNumber exactly!
                    List<Product> Find_Item_With_The_Exact_Atrical_Number_As_User_Input = Products.FindAll(product => product.ProductArticleNumber.ToString() == User.input);

                    if (Products.Exists(item => item.ProductArticleNumber.ToString() == User.input))
                    {
                        Console.WriteLine("Found product by exact match with Article Number!");
                        foreach (var product in Find_Item_With_The_Exact_Atrical_Number_As_User_Input)
                        {
                            table.AddRow(product.ProductArticleNumber.ToString(), product.ProductName!.ToString(), product.ProductStockQuantity.ToString(), product.ProductPrice.ToString(), product.SupplierName!.ToString());
                        }

                        ClearRightSideOfWindow();

                        Console.SetCursorPosition(0, 4);
                        AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));
                        Console.SetCursorPosition(0, 4);
                    }
                    else
                    {
                        //Check 2: Search if Input matches ProductName exactly!
                        List<Product> Find_Items_With_The_Exact_Name_As_User_Inpus = Products.FindAll(product => product.ProductName!.ToLower().Contains(User.input.ToLower())).ToList();

                        if (Products.Exists(item => item.ProductName!.ToLower() == User.input.ToLower()))
                        {
                            //Console.WriteLine("Found product by exact match with Product Name!");

                            foreach (var product in Find_Items_With_The_Exact_Name_As_User_Inpus)
                            {
                                table.AddRow(
                                    product.ProductName!.ToString(),
                                    product.ProductArticleNumber.ToString(),
                                    product.ProductStockQuantity.ToString(),
                                    product.ProductPrice.ToString(),
                                    product.SupplierName!.ToString());
                            }

                            ClearRightSideOfWindow();

                            Console.SetCursorPosition(0, 4);
                            AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));
                            Console.SetCursorPosition(0, 4);
                        }

                        else
                        {

                            //Check 3: Search if Input matches name of Product partially!
                            List<Product> Find_Items_That_Contains_Input = Products.FindAll(item => item.ProductName!.ToLower().Contains(User.input.ToLower())).ToList();

                            if (Find_Items_That_Contains_Input.Count > 0)
                            {
                                //Console.WriteLine("Found product by partial match!");

                                foreach (var product in Find_Items_That_Contains_Input)
                                {
                                    table.AddRow(
                                        product.ProductName!.ToString(),
                                        product.ProductArticleNumber.ToString(),
                                        product.ProductStockQuantity.ToString(),
                                        product.ProductPrice.ToString(),
                                        product.SupplierName!.ToString());
                                }

                                ClearRightSideOfWindow();

                                Console.SetCursorPosition(0, 4);
                                AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));
                                Console.SetCursorPosition(0, 4);
                            }

                            else
                            {

                                //Check 4: Search if Input matches any of the Product Tags!
                                List<Product> Find_Items_That_Contains_Input_In_Tags = Products
                                    .FindAll(item => item.tagsList.Exists(tag => tag.ToLower().Contains(User.input.ToLower()))).ToList();

                                if (Find_Items_That_Contains_Input_In_Tags.Count > 0)
                                {
                                    //Console.WriteLine("Found product by tag match!");
                                    foreach (var product in Find_Items_That_Contains_Input_In_Tags)
                                    {
                                        table.AddRow(product.ProductName!.ToString(),
                                            product.ProductArticleNumber.ToString(),
                                            product.ProductStockQuantity.ToString(),
                                            product.ProductPrice.ToString(),
                                            product.SupplierName!.ToString());
                                    }

                                    ClearRightSideOfWindow();

                                    Console.SetCursorPosition(0, 4);
                                    AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));
                                    Console.SetCursorPosition(0, 4);

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

                case "Back To Storage":
                    return;
            }
        }//Finished Running Task!


        //
        public static void AddToStorageHistory(string What_Product_Is_It, int What_Artical_Number, string What_Company, ProductHistory.Status What_Condition_It_Is_In)
        {

            //Load Products
            AppData appData = DataStorage.Load<AppData>("data.json");
            ProductHistoryRef = appData.ProductHistory;
            ProductHistory NewHistory = new ProductHistory();

            NewHistory.Year = DateTime.Now.Year;
            NewHistory.Month = DateTime.Now.Month;
            NewHistory.Day = DateTime.Now.Day;
            NewHistory.Name = What_Product_Is_It;
            NewHistory.ArticalNumber = What_Artical_Number;
            NewHistory.Company = What_Company;
            NewHistory.Time = new ProductHistory.Vector3C(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            NewHistory.status = What_Condition_It_Is_In;

            ProductHistoryRef.Add(NewHistory);

            appData.ProductHistory = ProductHistoryRef;

            DataStorage.Save("data.json", appData);

        }


        //
        public static void CheckStorageHistory()
        {
            while (true)
            {

                //Load Products
                AppData appData = DataStorage.Load<AppData>("data.json");
                ProductHistoryRef = appData.ProductHistory;

                Console.Clear();



                //Write.ProductHistory();

                //List<ProductHistory> History = ProductHistoryRef.AsEnumerable().Reverse().ToList();                               //

                List<string> HistoryList = new List<string>();

                //
                int maxValue = 0;
                int minValue = 0;

                try
                {

                    //
                    maxValue = ProductHistoryRef.Count;

                    for (int i = minValue; i < maxValue; i++)
                    {
                        if (ProductHistoryRef[i].Name == null)
                        {
                            break;
                        }

                        try
                        {
                            HistoryList.Add
                            ($"{ProductHistoryRef[i].ArticalNumber.ToString("D7")} | " +
                             $"{ProductHistoryRef[i].Year.ToString()}/" +
                             $"{ProductHistoryRef[i].Month.ToString()}/" +
                             $"{ProductHistoryRef[i].Day.ToString()}   | " +
                             $"{ProductHistoryRef[i].Time.X.ToString("D2")}:" +
                             $"{ProductHistoryRef[i].Time.Y.ToString("D2")}:" +
                             $"{ProductHistoryRef[i].Time.Z.ToString("D2")} | " +
                             $"{ProductHistoryRef[i].Name!.ToString()} | " +
                             $"{ProductHistoryRef[i].Company!.ToString()} | " +
                             $"{ProductHistoryRef[i].status.ToString()}");
                        }
                        catch
                        {
                            break;
                        }

                    }

                }
                catch
                {
                    HistoryList = new List<string>() { "History is Empty!" };
                }



                // Skapar en panel som visar produkterna.
                var HistoryPanel = new Panel(string.Join("\n", HistoryList.AsEnumerable().Reverse())).Header("HISTORY").Border(BoxBorder.Rounded);

                //Create Title Text!
                Write.ProductHistory();

                //Create and write Table Window on the right side!
                Console.SetCursorPosition(0, 4);
                AnsiConsole.Write(new Align(HistoryPanel, HorizontalAlignment.Right));

                Console.SetCursorPosition(0, 4);

                string Option = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices( "Back To Storage"));

                switch (Option)
                {
                    case "Search:":

                        AnsiConsole.Markup("[cyan]  Search: [/]");
                        User.input = User.ReadLine();
                        try
                        {
                            //
                            List<ProductHistory> Find_All_In_Year = new List<ProductHistory>();
                            try
                            {
                                Find_All_In_Year = ProductHistoryRef.FindAll(item => item.Year == int.Parse(User.input!));
                            }
                            catch
                            { }
                            if (Find_All_In_Year.Count == 0)
                            {
                                Find_All_In_Year = ProductHistoryRef;
                            }

                            //
                            List<ProductHistory> Find_All_In_Month = new List<ProductHistory>();
                            try
                            {
                                Find_All_In_Month = Find_All_In_Year.FindAll(item => item.Month == int.Parse(User.input!));
                            }
                            catch
                            { }
                            if (Find_All_In_Month.Count == 0)
                            {
                                Find_All_In_Month = Find_All_In_Year;
                            }

                            //
                            List<ProductHistory> Find_All_In_Day = new List<ProductHistory>();
                            try
                            {
                                Find_All_In_Day = Find_All_In_Month.FindAll(item => item.Day == int.Parse(User.input!));
                            }
                            catch
                            { }
                            if (Find_All_In_Day.Count == 0)
                            {
                                Find_All_In_Day = Find_All_In_Month;
                            }

                            //
                            List<ProductHistory> Find_All_In_Time = Find_All_In_Day.FindAll(item => item.Time.ToString() == User.input);
                            if (Find_All_In_Time.Count == 0)
                            {
                                Find_All_In_Time = Find_All_In_Day;
                            }

                            //
                            List<ProductHistory> Find_All_In_Name = Find_All_In_Time.FindAll(item => item.Name!.ToString() == User.input);
                            if (Find_All_In_Name.Count == 0)
                            {
                                Find_All_In_Name = Find_All_In_Time;
                            }

                            //
                            List<ProductHistory> Find_All_In_Company = Find_All_In_Name.FindAll(item => item.Company!.ToString() == User.input);
                            if (Find_All_In_Company.Count == 0)
                            {
                                Find_All_In_Company = Find_All_In_Name;
                            }

                            //
                            List<ProductHistory> Find_All_In_Status = Find_All_In_Company.FindAll(item => item.status.ToString() == User.input);
                            if (Find_All_In_Status.Count == 0)
                            {
                                Find_All_In_Status = Find_All_In_Company;
                            }

                            //
                            List<ProductHistory> Find_All_In_Artical_Number = Find_All_In_Company.FindAll(item => item.status.ToString() == User.input);
                            try
                            {
                                Find_All_In_Artical_Number = Find_All_In_Status.FindAll(item => item.status.ToString() == User.input);
                            }
                            catch
                            { }
                            if (Find_All_In_Artical_Number.Count == 0)
                            {
                                Find_All_In_Artical_Number = Find_All_In_Status;
                            }


                            var All_Found_Items = Find_All_In_Artical_Number;

                            ClearRightSideOfWindow();

                            HistoryList.Clear();

                            maxValue = All_Found_Items.Count();

                            try
                            {

                                for (int i = minValue; i < maxValue; i++)
                                {
                                    if (All_Found_Items[i].Name == null)
                                    {

                                    }

                                    try
                                    {
                                        HistoryList.Add
                                        ($"{ProductHistoryRef[i].ArticalNumber.ToString("D7")} | " +
                                         $"{ProductHistoryRef[i].Year.ToString()}/" +
                                         $"{ProductHistoryRef[i].Month.ToString()}/" +
                                         $"{ProductHistoryRef[i].Day.ToString()}   | " +
                                         $"{ProductHistoryRef[i].Time.X.ToString("D2")}:" +
                                         $"{ProductHistoryRef[i].Time.Y.ToString("D2")}:" +
                                         $"{ProductHistoryRef[i].Time.Z.ToString("D2")} | " +
                                         $"{ProductHistoryRef[i].Name!.ToString()} | " +
                                         $"{ProductHistoryRef[i].Company!.ToString()} | " +
                                         $"{ProductHistoryRef[i].status.ToString()}");
                                    }
                                    catch
                                    {
                                        break;
                                    }

                                }

                            }
                            catch
                            {
                                HistoryList = new List<string>() { "History is Empty!" };
                            }



                            // Skapar en panel som visar produkterna.
                            HistoryPanel = new Panel(string.Join("\n", HistoryList.AsEnumerable().Reverse())).Header("HISTORY").Border(BoxBorder.Rounded);


                            //Create and write Table Window on the right side!
                            Console.SetCursorPosition(0, 4);
                            AnsiConsole.Write(new Align(HistoryPanel, HorizontalAlignment.Right));
                            Console.SetCursorPosition(0, 4);



                        }
                        catch
                        {
                            AnsiConsole.MarkupLine("Could not run Search Method!");
                        }
                        Console.ReadKey();

                        break;

                    case "Back To Storage":
                        return;

                }
            }
        }

        //
        private static void MakeProductOptionsSelectionPrompt()
        {

            //Create StorageHistory Options!
            List<string> choices = new List<string>();                                                  //Create a List of strings!
            choices.Add("Search: ");                                                                    //Add a string into the 'choices' List!
            choices = Products.FindAll(item => item != null).Select(item => item.ProductName).ToList()!; //Add all Products 
            choices.Add("Back to Storage Menu");

            string Option = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(choices));

        }

        //
        private static void ClearRightSideOfWindow()
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

        }

        //
        private static void CreateProductTabelUI()
        {

            //Load Products
            AppData appData = DataStorage.Load<AppData>("data.json");
            var Products = appData.Products;

            //
            var table = new Table();

            table.AddColumn("Product     ");                                                            //Product Name!
            table.AddColumn("Artical ");                                                                //Artical Number!
            //table.AddColumn("Quantity");                                                                //Stock Quantity!
            //table.AddColumn("Price       ");                                                            //Price Per Product!
            table.AddColumn("Company     ");                                                            //Company Suppliers!

            //For index 0 to 9 in Products, add to table row!
            for (int i = 0; i < Math.Min(Products.Count, 10); i++)
            {
                table.AddRow(
                    Products[i].ProductName!.ToString(),
                    Products[i].ProductArticleNumber.ToString(),
                    //Products[i].ProductStockQuantity.ToString(),
                    //Products[i].ProductPrice.ToString(), 
                    Products[i].SupplierName!.ToString());
            }

            //
            User.cursorPosition = new Vector2(Console.CursorLeft, Console.CursorTop);
            Console.SetCursorPosition(((int)User.cursorPosition.X), ((int)User.cursorPosition.Y));
            AnsiConsole.Write(new Align(table, HorizontalAlignment.Right));

        }

    }
}
