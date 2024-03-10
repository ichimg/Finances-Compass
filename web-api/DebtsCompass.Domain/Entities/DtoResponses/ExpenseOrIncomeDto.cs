namespace DebtsCompass.Domain.Entities.DtoResponses
{
    public class ExpenseOrIncomeDto
    {
        public string Id { get; set; }  
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
        public bool IsExpense { get; set; }
    }
}
