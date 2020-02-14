using BudgetPlannerLib.Models;

namespace FileManagerLibrary
{
    public class CategoryFile : FileModel
    {
        public string CategoryName { get; set; }
        public Category[] IncomeCategories { get; set; }
        public Category[] ExpenseCategories { get; set; }

        public CategoryFile() : base() { }
        public CategoryFile(string path) : base(path) { }
        public CategoryFile(string path, Category[] incomeCategories, Category[] expenseCategories) : base(path)
        {
            IncomeCategories = incomeCategories;
            ExpenseCategories = expenseCategories;
        }
    }
}