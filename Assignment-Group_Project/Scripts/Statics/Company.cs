using Assignment_Group_Project.Scripts.Objects;

namespace Assignment_Group_Project.Scripts.Statics
{
    public static class Company
    {

        // Visar alla Leverantörer! 
        public static void AllSuppliers()
        {
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
            AppData appdata = DataStorage.Load<AppData>("data.json");

            Console.WriteLine("Search for a Supplier by name:");
            string searchName = Console.ReadLine() ?? "";


            // Om dem bara trycker enter utan att skriva någon leverantör, visas alla leverantöreer.
            if (string.IsNullOrWhiteSpace(searchName))
            {
                AllSuppliers();
                return;
            }

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

            // Frågar tills användaren skrivit något.
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

                    appData.Suppliers.Add(newSupplier);
                    DataStorage.Save("data.json", appData);

                    Console.WriteLine("Supplier added!");
                }
    }
}