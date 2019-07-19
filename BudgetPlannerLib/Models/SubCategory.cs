using System;

namespace BudgetPlannerLib.Models
{
    public class SubCategory : IEquals
    {
        #region - Fields
        private string _name;
        private decimal _value = 0;
        #endregion

        #region - Constructors
        public SubCategory() { }
        public SubCategory(string subCat)
        {
            Name = subCat;
        }
        #endregion

        #region - Methods
        /// <summary>
        /// Returns a new SubCategory from the opened file.
        /// </summary>
        /// <param name="fields">String array from the Parser.</param>
        /// <returns>Fresh SubCategory</returns>
        public static SubCategory FromFields(string[] fields)
        {
            return new SubCategory()
            {
                Name = fields[0],
                Value = 0
            };
        }

        public override Boolean Equals(Object obj)
        {
            var a = obj as SubCategory;
            return a != null &&
                this.Name == a.Name &&
                this.Value == a.Value;
        }
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
        public decimal Value
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
