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
    public bool? Deleted { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = null!;
}
