using Spectre.Console;
using System;
using System.Collections.Generic;
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


        //Read ``string´´ Input!
        public static void ReadInput()                                      //Read whatever the Input is. Made this because I don't want to type ``User.Input = Console.ReadLine()´´ all the time!
        {
            input = Console.ReadLine();                                         //Read what User entered and set it as User Input!
            while (input.IsWhiteSpace())                                        //While User Input is empty or only space!
            {
                //Print.InvalidCommandBig();
                Console.WriteLine("Empty Input! Type something!");              //Tell that the User has entered a empty Input and that they need to try again!
                input = Console.ReadLine();                                     //Read what User entered and set it as User Input!
            }

        }



        //Read ``int´´ Input!
        public static void ReadInputInt()                                 //Start method to read User Input and check if it can be used as an int variable!
        {

            //Check if the user has entered a whole number!
            string Num = Console.ReadLine();                                      //Read what User entered and set it as Num!
            while (!int.TryParse(Num, out inputInt))                              //While loop if the Input is not able to convert into a number!
            {
                Console.WriteLine("Invalied Input. Need to be a whole number!"); //If Num is not a whole number or have entered other then number characters, print this text!
                Num = Console.ReadLine();                                        //Ask the User to enter a new Input!
            }

            inputInt = int.Parse(Num);                                           //Input is able to become a int variable. Set Num as int_Input!

        }



        //Read ``decimal´´ Input!
        public static void ReadInputDecimal()                                 //Start method to read User Input and check if it can be used as a decimal variable!
        {
            //Check if the user has entered a decimal number!
            string Num = Console.ReadLine();                                        //Read what User entered and set it as Num!
            while (!decimal.TryParse(Num, out inputDecimal))                        //While loop if the Input is not able to convert into a decimal number!
            {
                Console.WriteLine("Invalied Input. Need to be a decimal number!");  //If Num is not a decimal number or have entered other then number characters, print this text!
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





    }
}
