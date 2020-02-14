using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetPlannerLib.Models
{
    public class Income : BaseColumn, IEquals
    {
        #region - Fields
        private static List<Category> _allIncomeCategories = new List<Category>();
        private Category _selectedCategory = new Category();
        #endregion

        #region - Constructors
        public Income() : base() { }
        public Income(string name, decimal amount, uint id) : base(id, name, amount) { }
        public Income(string categoryName, string name, decimal amount, uint id) : base(id, name, amount, categoryName) { }
        #endregion

        #region - Methods
        /// <summary>
        /// Reads string on file open.
        /// </summary>
        /// <param name="fields">OpenFile - String array from the FileControl.OpenFile Parser</param>
        /// <returns></returns>
        public static Income FromFields(string[] fields)
        {
            return new Income()
            {
                IDNumber = UInt32.Parse(fields[0]),
                Name = fields[1],
                Amount = Decimal.Parse(fields[2], System.Globalization.NumberStyles.Currency),
                SelectedCategory = new Category(fields[3])
            };
        }

        /// <summary>
        /// Clears the SubCategories.
        /// </summary>
        public static void ClearData()
        {
            AllIncomeCategories.Clear();
        }

        //public override bool Equals(Object obj)
        //{
        //    var a = obj as Income;
        //    return a != null &&
        //        this.IdNumber == a.IdNumber &&
        //        this.Name == a.Name &&
        //        this.Amount == a.Amount &&
        //        this.SelectedCategory == a.SelectedCategory;
        //}
        #endregion

        #region - Properties
        public static List<Category> AllIncomeCategories
        {
            get { return _allIncomeCategories; }
            set
            {
                _allIncomeCategories = value;
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
