using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetPlannerLib.Models
{
    public class Expense : DataElement
    {
        #region - Fields
        private static int NumOfExpenses { get; set; } = 0;
        private int _idNum;

        private static List<SubCategory> _allExpenseCategories = new List<SubCategory>();
        private SubCategory _selectedCategory;
        #endregion

        #region - Constructors
        public Expense() : base() { }
        public Expense(string categ, double val, int id) : base(categ, val)
        {
            Category = categ;
            Value = val;
            IdNumber = id;
            NumOfExpenses++;
        }
        public Expense(string subCat, string categ, double val, int id) : base(categ, val)
        {
            SelectedCategory = new SubCategory(subCat);
            Category = categ;
            Value = val;
            IdNumber = id;
            NumOfExpenses++;
        }
        #endregion

        #region - Methods
        public static Expense FromFields(string[] fields)
        {
            return new Expense()
            {
                IdNumber = Int32.Parse(fields[0]),
                Category = fields[1],
                Value = Double.Parse(fields[2])
            };
        }
        #endregion

        #region - Properties
        public int IdNumber
        {
            get { return _idNum; }
            set
            {
                _idNum = value;
            }
        }

        public static List<SubCategory> AllExpenseCategories
        {
            get { return _allExpenseCategories; }
            set
            {
                _allExpenseCategories = value;
            }
        }

        public SubCategory SelectedCategory
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
