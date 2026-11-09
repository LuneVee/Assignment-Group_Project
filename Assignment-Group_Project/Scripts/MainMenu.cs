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
                    .AddChoices(new[] { "Storage", "Orders", "End Program",}));


                switch (User.inputChoice)
                {

                    case "Storage":
                        Storage.StorageMenu();
                        break;

                    case "Orders":
                    Order.OrderMenu();
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
    }
}
