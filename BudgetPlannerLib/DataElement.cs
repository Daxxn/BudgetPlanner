using System;

namespace BudgetPlannerLib
{
    public class DataElement
    {
        #region - Fields
        private string _category;
        private decimal _value;

        public static event EventHandler<EventArgs> ValueChanged;
        #endregion

        #region - Constructors
        public DataElement() { }
        public DataElement(string category, decimal value)
        {
            Category = category;
            Value = value;
        }
        #endregion

        #region - Methods
        public static void Exit()
        {
            ValueChanged = null;
        }
        #endregion

        #region - Properties
        public virtual string Category
        {
            get { return _category; }
            set
            {
                _category = value;
            }
        }

        public virtual decimal Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }
        #endregion
    }
}
