using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerLib.Models
{
    public class CategoryModel
    {
        #region - Fields
        private static List<SubCategory> _allCategories;
        #endregion

        #region - Constructors

        #endregion

        #region - Methods

        #endregion

        #region - Properties
        public static List<SubCategory> AllIncomeCategories
        {
            get { return _allCategories; }
            set
            {
                _allCategories = value;
            }
        }
        #endregion
    }
}
