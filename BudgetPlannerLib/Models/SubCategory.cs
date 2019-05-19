using System;

namespace BudgetPlannerLib.Models
{
    public class SubCategory
    {
        #region - Fields
        private string _name;
        private double _value = 0;
        #endregion

        #region - Constructors
        public SubCategory() { }
        public SubCategory(string subCat)
        {
            Name = subCat;
        }
        #endregion

        #region - Methods
        public static SubCategory FromFields(string[] fields)
        {
            return new SubCategory()
            {
                Name = fields[0],
                Value = 0
            };
        }
        #endregion

        #region - Properties
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }
        #endregion
    }
}
