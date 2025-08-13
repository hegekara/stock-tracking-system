namespace Backend.Api.Models;

public enum StockTransactionType
{
    Inbound,
    Outbound
}

public class StockTransaction
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public int Quantity { get; set; }
    public StockTransactionType TransactionType { get; set; }
    public bool? Deleted { get; set; } = false;
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}
