namespace Assignment_Group_Project.Scripts.Objects
{
    public class AppData
    {
        
        public List<OrderDetails> Orders { get; set; } = new();
        public List<OrderDetails> OrderHistory { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();
    }
}