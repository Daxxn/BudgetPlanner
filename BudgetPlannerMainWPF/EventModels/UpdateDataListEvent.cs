using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlannerLib.Models;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.EventModels
{
    public class UpdateDataListEvent
    {
        #region - Fields
        public BindableCollection<SubCategory> IncomeCategories { get; private set; }
        public BindableCollection<SubCategory> ExpenseCategories { get; private set; }
        #endregion

        #region - Constructors
        public UpdateDataListEvent() { }
        public UpdateDataListEvent(BindableCollection<SubCategory> incomeCats, BindableCollection<SubCategory> expenseCats)
        {
            IncomeCategories = incomeCats;
            ExpenseCategories = expenseCats;
        }
        #endregion
    }
}
