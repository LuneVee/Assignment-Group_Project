using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_Group_Project.Scripts.Objects
{
    public class Supplier
    {

        public int SupplierId {get; set;}
        public string? Name {get; set;}
        public string? Email {get; set;}
        public string? Company {get; set;}
        public string? PhoneNumber {get; set;}



        public string GetSummarySupplier()
        {
            return $"Supplier ID: {SupplierId}, Name: {Name}, Email: {Email}, Company: {Company}, Phone Number: {PhoneNumber}";
        }

    }
}
