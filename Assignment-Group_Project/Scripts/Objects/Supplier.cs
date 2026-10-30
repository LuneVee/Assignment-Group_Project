using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_Group_Project.Scripts.Objects
{
    public class Supplier
    {

    
        public string? SupplierCompanyName {get; set;}
        public string? Email {get; set;}
        public string? PhoneNumber {get; set;}



        public string GetSummarySupplier()
        {
            return $"Supplier company name: {SupplierCompanyName}, Email: {Email}, Phone Number: {PhoneNumber}";
        }

    }
}
