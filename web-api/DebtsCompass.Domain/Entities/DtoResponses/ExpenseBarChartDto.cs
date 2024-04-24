namespace DebtsCompass.Domain.Entities.DtoResponses
{
    public class ExpenseBarChartDto
    {
        public int Month { get; set; }
        public string Category { get; set; }
        public decimal TotalAmount { get; set; }

        public ExpenseBarChartDto(int month, string category, decimal totalAmount)
        {
            Month = month;
            Category = category;
            TotalAmount = totalAmount;
        }
    }
}
