using Spectre.Console;
using System;
using System.Collections.Generic;
using Assignment_Group_Project.Scripts.Utilities;
using System.Text;
using Assignment_Group_Project.Scripts.Statics;

namespace Assignment_Group_Project.Scripts
{
    static class MainMenu
    {
        // Variables!
        static bool runProgram = true;

        //Create and Run main menu! -<This is currently only a placeholder for the main menu>!
        public static void RunMenu()
        {
            while (runProgram == true)
            {

                AnsiConsole.MarkupLine("[blue]Product Manager System[/]");
                User.inputChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] { "Storage", "Orders", "Statistics", "Suppliers", "End Program" }));


                switch (User.inputChoice)
                {

                    case "Storage":
                        Storage.StorageMenu();
                        break;

                    case "Orders":
                    Order.OrderMenu();
                        break;

                    case "Statistics":
                        Statistics.ShowOrderStatistics();
                        break;
                    case "Suppliers":
                        CompanyMenu();
                        break;
                    case "End Program":
                        runProgram = false;
                        break;
                    default:
                        AnsiConsole.MarkupLine("[red]Invalid choice. Please try again.[/]");
                        break;

                }
            }
        }


                    private static void CompanyMenu()
                    {
                    bool running = true;

                     while (running)
                {
                    string choice = AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                    .Title("Supplier Menu")
                    .AddChoices(
                    "Show All Suppliers",
                    "Search Supplier",
                    "Add Supplier",
                    "Back to Main Menu"
                ));

                 switch (choice)
                {
                     case "Show All Suppliers":
                        Company.AllSuppliers();
                         break;

                    case "Search Supplier":
                        Company.Search();
                        break;
                    
                    case "Add Supplier":
                        Company.AddSupplier();
                        break;
                   
                    case "Back to Main Menu":
                    running = false;
                    break;
                }

                 if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    AnsiConsole.Clear();
                    
        }
    }
}

    }
}
