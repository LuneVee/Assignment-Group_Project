namespace Assignment_Group_Project.Scripts.Objects
{
    public class AppData
    {
        // TODO: Ta bort // på raderna nedan när det finns en vanlig (icke-static)
        // klass för en enskild order. Byt ut "Order" mot klassens riktiga namn.
        public List<OrderDetails> Orders { get; set; } = new();
        public List<OrderDetails> OrderHistory { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();
    }
}