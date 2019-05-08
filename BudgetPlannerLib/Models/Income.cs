using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerLib.Models
{
    public class Income : DataElement
    {
        #region - Fields
        private static int AllIncomes { get; set; } = 0;
        private int _idNum;
        #endregion

        #region - Constructors
        public Income(string categ, double val, int id) : base(categ, val)
        {
            Category = categ;
            Value = val;
            IdNumber = id;
            AllIncomes++;
        }
        #endregion

        #region - Methods

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
