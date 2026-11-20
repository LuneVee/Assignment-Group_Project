using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Spectre.Console;
using Assignment_Group_Project.Scripts.Objects;

namespace Assignment_Group_Project.Scripts.Statics
{

    // Visar statistik över ordrar, grupperade på en period (dag, vecka, månad osv.)
    static class Statistics
    {

        // Visar antal ordrar och totalbelopp per period för vald ordertyp.
        public static void ShowOrderStatistics()
        {

            // Hämtar all sparad data från data.json.
            AppData appData = DataStorage.Load<AppData>("data.json");
            

            // Concat lägger ihop orderhistoriken och de aktiva ordrarna i en lista,
            // så att både levererade och pågående ordrar räknas med.
            var orders = appData.OrderHistory.Concat(appData.Orders).ToList();


            // Användaren väljer om utgående eller inkommande ordrar ska visas.
            // De två texterna måste stämma exakt med OrderType i ordrarna.
            string selectedOrderType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select order type:")
                    .AddChoices(new[] { "Outgoing Order", "Incoming Order" }));

            string selectedPeriod = AnsiConsole.Prompt(
                
                new SelectionPrompt<string>()
                    .Title("Select a period:")
                    .AddChoices(new[] { "Day", "Week", "Month", "Quarter", "Half Year", "Year", "All time" }));

                    // Behåller bara ordrar av vald typ som inte är avbrutna.
                    // && betyder "och", så båda villkoren måste stämma.
                    var filteredOrders = orders
                    .Where(order => order.OrderType == selectedOrderType && order.OrderStatus != "Cancelled")
                    .ToList();
                    if (filteredOrders.Count == 0)
                {
                    Console.WriteLine("No orders found.");
                    return;
                }

                var groups = filteredOrders.GroupBy(order => GetPeriodLabel(order.OrderDate, selectedPeriod))
                .Select(group => new
                {
                    TotalAmount = group.Sum(order => order.OrderedProductsList.Sum(product => product.ProductPrice * product.OrderAmount)),
                     Period = group.Key,
                    TotalOrders = group.Count()
         })
                     .OrderBy(group => group.Period)
                    .ToList();

                    foreach (var group in groups)
                    {
                        Console.WriteLine($"Period: {group.Period}, Total Amount: {group.TotalAmount:C}, Total Orders: {group.TotalOrders}");
                    }
        }

        // Ger en etikett för ett datum, beroende på vilken period som valts.
        // Ordrar med samma etikett hamnar i samma grupp.
        private static string GetPeriodLabel(DateTime date, string period)
        {
            switch (period)
            {
                case "Day":
                return date.ToString("yyyy-MM-dd");   
                
                case "Week":
                // DayOfWeek räknar söndag som 0, 
                // Formeln ger måndag 0, tisdag 1 ... söndag 6
                int daysSinceMonday = ((int)date.DayOfWeek + 6) % 7;
                var weekStart = date.AddDays(-daysSinceMonday);
                var weekEnd = weekStart.AddDays(6);
                return $"{weekStart:yyyy-MM-dd} - {weekEnd:yyyy-MM-dd}";

                case "Month":
                return date.ToString("yyyy-MM");

                case "Year":
                return date.ToString("yyyy");

                case "Quarter":
                int quarter = (date.Month - 1) / 3 + 1;
                return $"Q{quarter} {date.Year}";

                case "Half Year":
                int halfYear = (date.Month - 1) / 6 + 1;
                return $"H{halfYear} {date.Year}";

                case "All time":
                return "All time";

                default:
                return "";
            }
}
    }
}
