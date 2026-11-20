using Assignment_Group_Project.Scripts.Objects;
using Spectre.Console;

namespace Assignment_Group_Project.Scripts.Statics
{
    // Hanterar leverantörer: visa alla, söka, lägga till och menyn för dem.
    public static class Company
    {

        // visar alla leverantör. 
        public static void AllSuppliers()
        {

            // Hämtar all sparad data från data.json.
            AppData appData = DataStorage.Load<AppData>("data.json");

            if (appData.Suppliers.Count == 0)
            {
                Console.WriteLine("No Suppliers Found.");
                return;
            }

            foreach (Supplier supplier in appData.Suppliers)
            {
                Console.WriteLine(supplier.GetSummarySupplier());
            }
        }      
        
        // Sök på företagetsnamn 
        public static void Search()
        {

            // Hämtar all sparad data från data.json till appdata.
            AppData appdata = DataStorage.Load<AppData>("data.json");

            Console.WriteLine("Search for a Supplier by name:");

             // ReadLine kan ge null om inget kan läsas, så ?? "" gör att vi
             // får en tom text i stället för null.
            string searchName = Console.ReadLine() ?? "";


            // Om dem bara trycker enter utan att skriva någon leverantör, visas alla leverantöreer.
            if (string.IsNullOrWhiteSpace(searchName))
            {
                AllSuppliers();
                return;
            }


            // Where plockar ut de leverantörer där villkoret stämmer.
            // s är en leverantör i listan, och villkoret kollas för varje s.
            List<Supplier> matches = appdata.Suppliers
                .Where(s => s.SupplierCompanyName != null && 
                 s.SupplierCompanyName.Contains(searchName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matches.Count == 0)
            {
                Console.WriteLine("No suppliers found.");
                return;
            }

            foreach (Supplier supplier in matches)
            {
                Console.WriteLine(supplier.GetSummarySupplier());
            }
        }

            // Frågar tills användaren skrivit något. Används för alla tre fälten
            // så att samma loop inte behöver skrivas tre gånger.
                    private static string AskNotEmpty(string question)
                    {
                        while (true)
                        {
                            Console.Write(question);
                            string answer = Console.ReadLine() ?? "";

                            if (!string.IsNullOrWhiteSpace(answer))
                            {
                                return answer.Trim();
                            }

                            Console.WriteLine("This field cannot be empty. Try again.");
                        }
                    }

                // Lägger till en ny leverantör.
                public static void AddSupplier()
                {

                    // Hämtar all sparad data från data.json.
                    AppData appData = DataStorage.Load<AppData>("data.json");

                    string companyName = AskNotEmpty("Company name: ");

                    // Nekar om namnet redan finns
                    bool alreadyExists = appData.Suppliers.Any(s =>
                        s.SupplierCompanyName != null &&
                        s.SupplierCompanyName.Equals(companyName, StringComparison.OrdinalIgnoreCase));

                    if (alreadyExists)
                    {
                        Console.WriteLine("A supplier with that name already exists.");
                        return;
                    }

                    string email = AskNotEmpty("Email: ");
                    string phoneNumber = AskNotEmpty("Phone number: ");

                    Supplier newSupplier = new Supplier
                    {
                        SupplierCompanyName = companyName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    };

                    // Lägger leverantören i listan och sparar direkt i data.json.
                    appData.Suppliers.Add(newSupplier);
                    DataStorage.Save("data.json", appData);

                    Console.WriteLine("Supplier added!");
                }

                // Menyn för leverantörer. Körs tills användaren väljer att gå tillbaka.
                   public static void CompanyMenu()
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