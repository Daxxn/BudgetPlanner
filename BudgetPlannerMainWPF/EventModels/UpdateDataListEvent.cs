using BudgetPlannerLib.Models;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.EventModels
{
    public class UpdateDataListEvent
    {
        #region - Fields
        public BindableCollection<Category> IncomeCategories { get; private set; }
        public BindableCollection<Category> ExpenseCategories { get; private set; }
        #endregion

        #region - Constructors
        public UpdateDataListEvent() { }
        public UpdateDataListEvent(BindableCollection<Category> incomeCats, BindableCollection<Category> expenseCats)
        {
            IncomeCategories = incomeCats;
            ExpenseCategories = expenseCats;
        }
        #endregion
    }
}