using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetPlannerLib.Models
{
    public class Expense : BaseColumn, IEquals
    {
        #region - Fields
        private static List<Category> _allExpenseCategories = new List<Category>();
        private Category _selectedCategory = new Category();
        #endregion

        #region - Constructors
        public Expense() : base() { }
        public Expense(string name, decimal amount, uint id) : base(id, name, amount) { }
        public Expense(string categoryName, string name, decimal amount, uint id) : base(id, name, amount, categoryName) { }
        #endregion

        #region - Methods
        public static Expense FromFields(string[] fields)
        {
            return new Expense()
            {
                IDNumber = UInt32.Parse(fields[0]),
                Name = fields[1],
                Amount = Decimal.Parse(fields[2], System.Globalization.NumberStyles.Currency),
                SelectedCategory = new Category(fields[3])
            };
        }

        public static void ClearData()
        {
            AllExpenseCategories.Clear();
        }

        //public override Boolean Equals(Object obj)
        //{
        //    var a = obj as Expense;
        //    return a != null &&
        //        this.IdNumber == a.IdNumber &&
        //        this.Category == a.Category &&
        //        this.Value == a.Value &&
        //        this.SelectedCategory == a.SelectedCategory;
        //}
        #endregion

        #region - Properties

        public static List<Category> AllExpenseCategories
        {
            get { return _allExpenseCategories; }
            set
            {
                _allExpenseCategories = value;
            }
        }

        public Category SelectedCategory
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
