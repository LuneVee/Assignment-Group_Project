using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_Group_Project.Scripts.Utilities
{
    static class User
    {

        public static string? input { get; set; }
        public static int inputInt;

 //Read ``string´´ Input!
        public static void ReadInput()                                     //Read whatever the Input is. Made this because I don't want to type ``User.Input = Console.ReadLine()´´ all the time!
        {
            input = Console.ReadLine();                                           //Read what User entered and set it as User Input!
            while (input.IsWhiteSpace())                                          //While User Input is empty or only space!
            {
                //Print.InvalidCommandBig();
                Console.WriteLine("Empty Input! Type something!");               //Tell that the User has entered a empty Input and that they need to try again!
                input = Console.ReadLine();                                      //Read what User entered and set it as User Input!
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

            inputInt = int.Parse(Num);                                          //Input is able to become a int variable. Set Num as int_Input!

        }



    }
}
