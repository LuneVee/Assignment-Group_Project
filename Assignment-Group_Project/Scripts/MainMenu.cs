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

            StartScreen();

            while (runProgram == true)
            {

                Console.Clear();

                Write.MainMenu();

                AnsiConsole.MarkupLine("[blue]Product Manager System[/]");
                User.inputChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] { "Storage", "Orders", "Statistics", "Suppliers", "End Program" }));


                switch (User.inputChoice)
                {

                    case "Storage":
                        Loading();
                        Storage.StorageMenu();
                        break;

                    case "Orders":
                        Loading();
                    Order.OrderMenu();
                        break;

                    case "Statistics":
                        Loading();
                        Statistics.ShowOrderStatistics();
                        break;
                    case "Suppliers":
                        Loading();
                        Company.CompanyMenu();
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



        private static void StartScreen()
        {

            Write.TeamName();
            Loading();

        }


        private static void Loading()
        {
            Random random = new Random();
            double RanNumTask;
            int RanNumThread;

            Console.SetCursorPosition(5, 4);

            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    // Create loading bar
                    var task = ctx.AddTask("[green]Loading...[/]", maxValue: 100);

                    // Increase progress until 100%
                    while (!ctx.IsFinished)
                    {
                        RanNumTask = random.Next(0, 4);
                        RanNumThread = random.Next(1, 2);

                        task.Increment(RanNumTask);
                        Thread.Sleep(RanNumThread);
                    }
                });

        }




    }
}
