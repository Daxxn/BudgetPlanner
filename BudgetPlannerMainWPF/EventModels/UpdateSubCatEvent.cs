using System.Collections.Generic;
using BudgetPlannerLib.Models;

namespace BudgetPlannerMainWPF.EventModels
{
    public class UpdateSubCatEvent
    {
        #region - Fields
        public List<Category> IncomeSubs { get; private set; }
        public List<Category> ExpenseSubs { get; private set; }
        #endregion

        #region - Constructors
        public UpdateSubCatEvent() { }
        public UpdateSubCatEvent(List<Category> income, List<Category> expense)
        {
            IncomeSubs = income;
            ExpenseSubs = expense;
        }
        #endregion
    }
}