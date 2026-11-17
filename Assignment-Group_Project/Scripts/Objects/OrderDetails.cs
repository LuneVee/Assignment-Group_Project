namespace Assignment_Group_Project.Scripts.Objects
{
// Representerar en enskild order.
public class OrderDetails
{
    // Orderns unika nummer.
    public int OrderNumber { get; set; }

    // Orderns nuvarande status.
    public string OrderStatus { get; set; } = "Packing";

    // Datum då ordern skapades.
    public DateTime OrderDate { get; set; }

    // Datum då ordern togs emot.
    public DateTime? OrderReceivedDate { get; set; }

    // Leverantörens namn.
    public string? SupplierName { get; set; }

    // Håller reda på om leverantören har bekräftat ordern. False för att det ska vara standard när ordern skapas.
    public bool SupplierConfirmed { get; set; } = false;

    // Typ av order, t.ex "Incoming Order" eller "Outgoing Order".
    public string? OrderType { get; set;}

    // Produkterna som ingår i ordern.
    public List<Product> OrderedProductsList { get; set; } = new();
}
}