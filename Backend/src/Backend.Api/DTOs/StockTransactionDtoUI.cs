namespace Backend.Api.Dtos
{
    public class StockTransactionDtoIU
    {
        public int Quantity { get; set; }
        public StockTransactionType TransactionType { get; set; }
        public int ProductId { get; set; }
    }
}