using System;

namespace BudgetPlannerLib.Models
{
    public class BaseColumn
    {
        #region - Fields
        private uint _IdNumber;
        private string _name;
        private decimal _amount;
        private Category _category;

        public static event EventHandler<EventArgs> ValueChanged;
        #endregion

        #region - Constructors
        public BaseColumn() { }
        public BaseColumn(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
        public BaseColumn(uint idNum, string name, decimal amount)
        {
            IDNumber = idNum;
            Name = name;
            Amount = amount;
        }
        public BaseColumn(uint idNum, string name, decimal amount, string categoryName)
        {
            IDNumber = idNum;
            Name = name;
            Amount = amount;
            Category = new Category(categoryName);
        }
        #endregion

        #region - Methods
        public static void Exit()
        {
            ValueChanged = null;
        }
        #endregion

        #region - Properties
        public uint IDNumber
        {
            get { return _IdNumber; }
            set
            {
                _IdNumber = value;
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                ValueChanged?.Invoke(this, new EventArgs());
            }
        }

        public Category Category
        {
            get { return _category; }
            set
            {
                _category = value;
            }
        }
        #endregion
    }
}
