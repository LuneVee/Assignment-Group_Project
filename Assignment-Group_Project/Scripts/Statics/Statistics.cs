using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Spectre.Console;
using Assignment_Group_Project.Scripts.Objects;

namespace Assignment_Group_Project.Scripts.Statics
{
    static class Statistics
    {
        public static void ShowOrderStatistics()
        {
            AppData appData = DataStorage.Load<AppData>("data.json");
            
            var orders = appData.OrderHistory.Concat(appData.Orders).ToList();

            string selectedOrderType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select order type:")
                    .AddChoices( "Outgoing Order", "Incoming Order" ));

            string selectedPeriod = AnsiConsole.Prompt(
                
                new SelectionPrompt<string>()
                    .Title("Select a period:")
                    .AddChoices("Day", "Week", "Month", "Quarter", "Half Year", "Year", "All time" ));
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

                    Console.ReadKey();
        }
        private static string GetPeriodLabel(DateTime date, string period)
        {
            switch (period)
            {
                case "Day":
                return date.ToString("yyyy-MM-dd");   
                
                case "Week":
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
