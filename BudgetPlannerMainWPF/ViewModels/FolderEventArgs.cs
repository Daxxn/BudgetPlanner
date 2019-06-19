using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerMainWPF.ViewModels
{
    /// <summary>
    /// CreatingNewBudget Event Arguments
    /// </summary>
    public class FolderEventArgs : EventArgs
    {
        #region - Fields
        /// <summary>
        /// Path entered by the user in the NewBudgetViewModel
        /// </summary>
        public string FolderPath { get; private set; }
        /// <summary>
        /// Name entered by the user in the NewBudgetViewModel
        /// </summary>
        public string BudgetName { get; private set; }
        #endregion

        #region - Constructors
        public FolderEventArgs() { }
        public FolderEventArgs(string folder, string name)
        {
            FolderPath = folder;
            BudgetName = name;
        }
        #endregion
    }
}
