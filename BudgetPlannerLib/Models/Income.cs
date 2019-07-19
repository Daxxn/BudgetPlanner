using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetPlannerLib.Models
{
    public class Income : DataElement, IEquals
    {
        #region - Fields
        private static int NumOfIncomes { get; set; } = 0;
        private int _idNum;

        private static List<SubCategory> _allIncomeCategories = new List<SubCategory>();
        private SubCategory _selectedCategory;
        #endregion

        #region - Constructors
        public Income() : base() { }
        public Income(string categ, decimal val, int id) : base(categ, val)
        {
            Category = categ;
            Value = val;
            IdNumber = id;
        }
        public Income(string subCat, string categ, decimal val, int id) : base(categ, val)
        {
            SelectedCategory = new SubCategory(subCat);
            Category = categ;
            Value = val;
            IdNumber = id;
            NumOfIncomes++;
        }
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
                IdNumber = Int32.Parse(fields[0]),
                Category = fields[1],
                Value = Decimal.Parse(fields[2], System.Globalization.NumberStyles.Currency),
                SelectedCategory = new SubCategory(fields[3])
            };
        }

        /// <summary>
        /// Clears the SubCategories.
        /// </summary>
        public static void ClearData()
        {
            AllIncomeCategories.Clear();
        }

        public override bool Equals(Object obj)
        {
            var a = obj as Income;
            return a != null &&
                this.IdNumber == a.IdNumber &&
                this.Category == a.Category &&
                this.Value == a.Value &&
                this.SelectedCategory == a.SelectedCategory;
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

        public static List<SubCategory> AllIncomeCategories
        {
            get { return _allIncomeCategories; }
            set
            {
                _allIncomeCategories = value;
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
