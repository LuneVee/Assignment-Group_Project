using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Assignment_Group_Project.Scripts.Utilities
{
    static class User
    {

        //Variables!
        public static string? input { get; set; }
        public static string? inputChoice { get; set; }
        public static int inputInt;
        public static decimal inputDecimal;

        public static Vector2 cursorPosition = new Vector2();


        //Read ``string´´ Input!
        public static void ReadInput()                                      //Read whatever the Input is. Made this because I don't want to type ``User.Input = Console.ReadLine()´´ all the time!
        {
            input = Console.ReadLine();                                         //Read what User entered and set it as User Input!
            while (input.IsWhiteSpace())                                        //While User Input is empty or only space!
            {
                //Print.InvalidCommandBig();
                AnsiConsole.MarkupLine("Empty Input! Type something!");         //Tell that the User has entered a empty Input and that they need to try again!
                input = Console.ReadLine();                                     //Read what User entered and set it as User Input!
            }

        }



        //Read ``int´´ Input!
        public static void ReadInputInt()                                 //Start method to read User Input and check if it can be used as an int variable!
        {

            //Check if the user has entered a whole number!
            string Num = Console.ReadLine();                                    //Read what User entered and set it as Num!
            while (!int.TryParse(Num, out inputInt))                            //While loop if the Input is not able to convert into a number!
            {
                AnsiConsole.MarkupLine                                          //If Num is not a whole number or have entered other then number characters, print this text!
                    ("Invalied Input. Need to be a whole number!");
                Num = Console.ReadLine();                                       //Ask the User to enter a new Input!
            }

            inputInt = int.Parse(Num);                                          //Input is able to become a int variable. Set Num as int_Input!

        }



        //Read ``decimal´´ Input!
        public static void ReadInputDecimal()                                 //Start method to read User Input and check if it can be used as a decimal variable!
        {
            //Check if the user has entered a decimal number!
            string Num = Console.ReadLine();                                        //Read what User entered and set it as Num!
            while (!decimal.TryParse(Num, out inputDecimal))                        //While loop if the Input is not able to convert into a decimal number!
            {
                AnsiConsole.MarkupLine                                              //If Num is not a decimal number or have entered other then number characters, print this text!
                    ("Invalied Input. Need to be a decimal number!");
                Num = Console.ReadLine();                                           //Ask the User to enter a new Input!
            }
            inputDecimal = decimal.Parse(Num);                                      //Input is able to become a decimal variable. Set Num as decimal_Input!
        }



        //Ask User to between Yes or No with a selection prompt!
        public static void AskYesOrNo()
        {
            //Create a selection prompt for the user to choose between "Yes" and "No"
            var YesNo = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .AddChoices("Yes", "No"));                                      //Add choices to YesNo selection prompt.

            switch (YesNo)                                                      //Read what the User has selected with selection prompt!
            {
                case "Yes":                                                     //If the User has selected "Yes" set User InputChoice to "Yes"!
                    inputChoice = "Yes";
                    break;

                case "No":                                                      //If the User has selected "No" set User InputChoice to "No"!
                    inputChoice = "No";
                    break;

            }
        }



        //Read User Key input!
        public static string? ReadLine()
        {

            string ReadLineInput = "";

            string inputLine = "";

            while (true)
            {

                ConsoleKeyInfo key = Console.ReadKey(true);

                // ESC = Leave method
                if (key.Key == ConsoleKey.Escape)
                {
                    return null;
                }

                // ENTER = Confirm input
                if (key.Key == ConsoleKey.Enter)
                {
                    AnsiConsole.WriteLine();
                    return inputLine;
                }

                // BACKSPACE = Delete character
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (inputLine.Length > 0)                                                       //Check how many char inputLine have!
                    {
                        inputLine = inputLine[..^1];                                                //inputLine = itself - last char that it have! So it just remove it last char.
                        AnsiConsole.Write("\b" + " " + "\b");                                       //Move back 1 char and write over it, then move back 1 char again!  \b == move back 1 char on current console line. " " == write " " as next char on console line!
                    }
                }
                // If User typ on the keyboard!
                else if (!char.IsControl(key.KeyChar))
                {
                    inputLine += key.KeyChar;                                                       //Add the Key that was pressed as a char to inputLine!
                    AnsiConsole.Write(key.KeyChar);                                                 //Write the Keyboard input that just where made!
                }
            }
        }



        //Read User Digit Key input!
        public static string? ReadLineInt()
        {

            string ReadLineInput = "";

            string inputLine = "";

            while (true)
            {

                ConsoleKeyInfo key = Console.ReadKey(true);

                // ESC = Leave method
                if (key.Key == ConsoleKey.Escape)
                {
                    return null;
                }

                // ENTER = Confirm input
                if (key.Key == ConsoleKey.Enter)
                {
                    AnsiConsole.WriteLine();
                    return inputLine;
                }

                // BACKSPACE = Delete character
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (inputLine.Length > 0)                                                       //Check how many char inputLine have!
                    {
                        inputLine = inputLine[..^1];                                                //inputLine = itself - last char that it have! So it just remove it last char.
                        AnsiConsole.Write("\b" + " " + "\b");                                       //Move back 1 char and write over it, then move back 1 char again!  \b == move back 1 char on current console line. " " == write " " as next char on console line!
                    }
                }
                // If User typ on the keyboard!
                else if (char.IsDigit(key.KeyChar))
                {
                    inputLine += key.KeyChar;                                                       //Add the Key that was pressed as a char to inputLine!
                    AnsiConsole.Write(key.KeyChar);                                                 //Write the Keyboard input that just where made!
                }
            }
        }



        //Read User Input with a max char amount!
        public static string? ReadLineRange(string Text_At_The_Start_Of_The_Line, int Max_Char_For_Line)
        {

            string inputLine = "";

            AnsiConsole.Markup($"{Text_At_The_Start_Of_The_Line} ");

            while (true)
            {

                ConsoleKeyInfo key = Console.ReadKey(true);                                         //

                // ESC
                if (key.Key == ConsoleKey.Escape)                                                   //If User press the ESC key!
                {
                    int CharAmount = Text_At_The_Start_Of_The_Line.Length + inputLine.Length;       //

                    //
                    while (CharAmount >= 0)                                                         //
                    {
                        AnsiConsole.Write("\b" + " " + "\b");                                       //Move back 1 char and write over it, then move back 1 char again!  \b == move back 1 char on current console line. " " == write " " as next char on console line!
                        CharAmount -= 1;                                                            //
                    }
                    return null;
                }

                // ENTER
                if (key.Key == ConsoleKey.Enter)                                                    //
                {
                    Console.WriteLine();
                    return inputLine;
                }

                // BACKSPACE = Delete character
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (inputLine.Length > 0)                                                       //Check how many char inputLine have!
                    {
                        inputLine = inputLine[..^1];                                                //inputLine = itself - last char that it have! So it just remove it last char.
                        Console.Write("\b" + " " + "\b");                                           //Move back 1 char and write over it, then move back 1 char again!  \b == move back 1 char on current console line. " " == write " " as next char on console line!
                    }
                }

                // If User typ on the keyboard!
                else if (!char.IsControl(key.KeyChar))
                {
                    // If NOT reached Max Char!
                    if (inputLine.Length < Max_Char_For_Line)
                    {
                        inputLine += key.KeyChar;                                                   //Add the Key that was pressed as a char to inputLine!
                        Console.Write(key.KeyChar);                                                 //Write the Keyboard input that just where made!
                    }
                }
            }
        }



        //Read User Digit Input with a max char amount!
        public static string? ReadLineAsIntRange(string Text_At_The_Start_Of_The_Line, int Max_Char_For_Line)
        {

            string inputLine = "";
            int inputAsInt;

            AnsiConsole.Markup($"{Text_At_The_Start_Of_The_Line} ");

            while (true)
            {

                ConsoleKeyInfo key = Console.ReadKey(true);
                Vector2 CursorPosition = new Vector2(Console.CursorLeft, Console.CursorTop);

                // ESC
                if (key.Key == ConsoleKey.Escape)                                                   //If User press the ESC key!
                {
                    int CharAmount = Text_At_The_Start_Of_The_Line.Length + inputLine.Length;       //

                    //
                    while (CharAmount >= 0)                                                         //
                    {
                        Console.Write("\b" + " " + "\b");                                           //Move back 1 char and write over it, then move back 1 char again!  \b == move back 1 char on current console line. " " == write " " as next char on console line!
                        CharAmount -= 1;                                                            //
                    }
                    return null;
                }

                // ENTER
                if (key.Key == ConsoleKey.Enter)                                                    //
                {
                    try
                    {
                        AnsiConsole.WriteLine();
                        inputAsInt = int.Parse(inputLine);
                        return inputLine;
                    }
                    catch
                    {
                        Console.SetCursorPosition(((int)CursorPosition.X), ((int)CursorPosition.Y));
                    }

                }

                // BACKSPACE = Delete character
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (inputLine.Length > 0)                                                       //Check how many char inputLine have!
                    {
                        inputLine = inputLine[..^1];                                                //inputLine = itself - last char that it have! So it just remove it last char.
                        AnsiConsole.Markup("\b" + " " + "\b");                                      //Move back 1 char and write over it, then move back 1 char again!  \b == move back 1 char on current console line. " " == write " " as next char on console line!
                    }
                }

                // If User typ on the keyboard!
                else if (char.IsDigit(key.KeyChar))
                {
                    // If NOT reached Max Char!
                    if (inputLine.Length < Max_Char_For_Line)
                    {
                        inputLine += key.KeyChar;                                                   //Add the Key that was pressed as a char to inputLine!
                        Console.Write(key.KeyChar);                                                 //Write the Keyboard input that just where made!
                    }
                }
            }
        }




    }

}
