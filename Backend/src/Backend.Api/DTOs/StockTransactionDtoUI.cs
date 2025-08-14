namespace Backend.Api.Dtos
{
    public class StockTransactionDtoIU
    {
        public DateTime TransactionDate { get; set; }
        public int Quantity { get; set; }
        public StockTransactionType TransactionType { get; set; }
        public int ProductId { get; set; }
    }
}