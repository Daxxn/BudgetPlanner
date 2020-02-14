using BudgetPlannerLib.Models;

namespace FileManagerLibrary
{
    public class BudgetFile : FileModel
    {
        public string BudgetName { get; set; } = "No Name";
        public Income[] IncomeData { get; set; }
        public Expense[] ExpenseData { get; set; }

        public BudgetFile() : base() { }
        public BudgetFile(string path) : base(path) { }
        public BudgetFile(string path, Income[] income, Expense[] expense) : base(path)
        {
            IncomeData = income;
            ExpenseData = expense;
        }
    }
}