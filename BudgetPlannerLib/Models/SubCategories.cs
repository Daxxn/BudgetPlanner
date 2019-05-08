using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetPlannerLib
{
    public static class SubCategories
    {
        #region - Fields
        private static List<string> _incomeCatList = new List<string>();
        private static List<string> _expenseCatList = new List<string>();

        private static string _selectedCategory;
        #endregion

        #region - Methods

        #endregion

        #region - Properties

        public static List<string> IncomeCategoryList
        {
            get { return _incomeCatList; }
            set
            {
                _incomeCatList = value;
            }
        }

        public static List<string> ExpenseCategoryList
        {
            get { return _expenseCatList; }
            set
            {
                _expenseCatList = value;
            }
        }

        public static string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
            }
        }
        #endregion
    }
}
