namespace Backend.Api.Dtos
{
    public class StockTransactionDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public StockTransactionType TransactionType { get; set; }
        public string ProductName { get; set; } = null!;
    }
}
