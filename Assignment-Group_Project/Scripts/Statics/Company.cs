using Assignment_Group_Project.Scripts.Objects;

namespace Assignment_Group_Project.Scripts.Statics
{
    public static class Company
    {

        // Visar alla Leverantörer! 
        public static void AllSuppliers()
        {
            AppData appData = new DataStorage().Load();

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
            AppData appdata = new DataStorage().Load();

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
    }
}