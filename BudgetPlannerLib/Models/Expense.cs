using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerLib.Models
{
    public class Expense : DataElement
    {
        #region - Fields
        private static int AllExpenses { get; set; } = 0;
        private int _idNum;
        #endregion

        #region - Constructors
        public Expense() : base() { }
        public Expense(string categ, double val, int id) : base(categ, val)
        {
            Category = categ;
            Value = val;
            IdNumber = id;
            AllExpenses++;
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
        #endregion
    }
}
