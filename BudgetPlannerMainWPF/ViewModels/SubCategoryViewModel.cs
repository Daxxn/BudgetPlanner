using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetPlannerLib.Models;
using Caliburn.Micro;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class SubCategoryViewModel : Screen
    {
        #region - Fields
        private BindableCollection<SubCategory> _incomeCategories;
        private BindableCollection<SubCategory> _expenseCategories;
        #endregion

        #region - Constructors
        public SubCategoryViewModel()
        {

        }
        #endregion

        #region - Methods

        #endregion

        #region - Properties
        public BindableCollection<SubCategory> IncomeCategories
        {
            get { return _incomeCategories; }
            set
            {
                _incomeCategories = value;
                NotifyOfPropertyChange(() => IncomeCategories);
            }
        }

        public BindableCollection<SubCategory> ExpenseCategories
        {
            get { return _expenseCategories; }
            set
            {
                _expenseCategories = value;
                NotifyOfPropertyChange(() => ExpenseCategories);
            }
        }
        #endregion
    }
}
