using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerLib
{
    public class DataElement
    {
        #region - Fields
        private string _category;
        private double _value;

        public static event EventHandler<EventArgs> ValueChanged;
        #endregion

        #region - Constructors
        public DataElement(string category, double value)
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

        public virtual double Value
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
