namespace DebtsCompass.Domain.TotalList
{
    public class TotalList<T> : List<T>
    {
        public decimal TotalAmountExpenses { get; private set; }
        public decimal TotalAmountIncomes { get; private set; }

        public TotalList(List<T> items, decimal totalExpenses, decimal totalIncomes)
        {
            TotalAmountExpenses = totalExpenses;
            TotalAmountIncomes = totalIncomes;

            AddRange(items);
        }
    }
}
