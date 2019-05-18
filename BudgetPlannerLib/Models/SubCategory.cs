using System;

namespace BudgetPlannerLib.Models
{
    public class SubCategory
    {
        #region - Fields
        private string _name;
        private double _value;
        #endregion

        #region - Constructors
        public SubCategory() { }
        public SubCategory(string subCat)
        {
            Name = subCat;
        }

        // not really usable...
        public SubCategory(string name, double value)
        {
            Name = name;
            Value = value;
        }
        #endregion

        #region - Methods

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
