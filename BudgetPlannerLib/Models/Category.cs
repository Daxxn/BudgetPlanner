using System;

namespace BudgetPlannerLib.Models
{
    public class Category : IEquals
    {
        #region - Fields
        private string _name;
        private decimal _amount;
        #endregion

        #region - Constructors
        public Category() { }
        public Category(string name)
        {
            Name = name;
        }
        public Category(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
        #endregion

        #region - Methods
        /// <summary>
        /// Returns a new SubCategory from the opened file.
        /// </summary>
        /// <param name="fields">String array from the Parser.</param>
        /// <returns>Fresh SubCategory</returns>
        public static Category FromFields(string[] fields)
        {
            return new Category()
            {
                Name = fields[0],
                Amount = 0
            };
        }

        //public override Boolean Equals(Object obj)
        //{
        //    var a = obj as Category;
        //    return a != null &&
        //        this.Name == a.Name &&
        //        this.Value == a.Value;
        //}
        #endregion

        #region - Properties
        /// <summary>
        /// SubCategory Name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

            }
        }

        /// <summary>
        /// SubCategory Total Value
        /// </summary>
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
            }
        }
        #endregion
    }
}
