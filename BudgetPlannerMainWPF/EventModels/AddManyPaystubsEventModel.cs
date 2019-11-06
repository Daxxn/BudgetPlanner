using PaystubLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerMainWPF.EventModels
{
    public class AddManyPaystubsEventModel
    {
        #region - Fields & Properties
        public Paystub[] Paystubs { get; private set; }
        public List<Paystub> PaystubDataList { get; private set; }
        #endregion

        #region - Constructors
        public AddManyPaystubsEventModel() { }
        public AddManyPaystubsEventModel(Paystub[] paystubs)
        {
            Paystubs = paystubs;
        }
        public AddManyPaystubsEventModel(List<Paystub> paystubs)
        {
            PaystubDataList = paystubs;
        }
        #endregion

        #region - Methods

        #endregion

        #region - Full Properties

        #endregion
    }
}
