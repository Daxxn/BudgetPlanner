using System;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class SimpleKeyEventAgrs : EventArgs
    {
        #region - Fields
        public int SenderId { get; set; }
        #endregion

        #region - Constructors
        public SimpleKeyEventAgrs(int senderId)
        {
            SenderId = senderId;
        }
        #endregion

        #region - Methods

        #endregion

        #region - Properties

        #endregion
    }
}