using System;

namespace BudgetPlannerLib.Models
{
    public class SubCategory
    {
        #region - Fields
        private string _categoryName;
        #endregion

        #region - Constructors
        public SubCategory() { }
        public SubCategory(string subCat)
        {
            CategoryName = subCat;
        }
        #endregion

        #region - Methods

        #endregion

        #region - Properties
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
            }
        }
        #endregion
    }
}
