namespace Assignment_Group_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            // Menyn fortsätter visas tills användaren väljer att avsluta.
            while (running)
            {
                Console.WriteLine("\n===== ORDER MENU =====");
                Console.WriteLine("1. Add outgoing order");
                Console.WriteLine("2. Add incoming order");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine() ?? "";

                // Kontrollerar vilket alternativ användaren har valt.
                switch (choice)
                {
                    case "1":
                        Order.AddOutgoingOrder();
                        break;

                    case "2":
                        Order.AddIncomingOrder();
                        break;

                    case "3":
                        running = false;
                        Console.WriteLine("Program closed.");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}