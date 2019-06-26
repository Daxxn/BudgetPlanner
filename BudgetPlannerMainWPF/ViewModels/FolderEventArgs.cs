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
        public string SubCatPath { get; private set; }
        public bool OpenSubCategories { get; private set; } = false;
        #endregion

        #region - Constructors
        public FolderEventArgs() { }
        public FolderEventArgs(string folder, string name)
        {
            FolderPath = folder;
            BudgetName = name;
        }
        public FolderEventArgs(string folder, string name, string subFolder)
        {
            FolderPath = folder;
            BudgetName = name;
            SubCatPath = subFolder;
        }
        public FolderEventArgs(string folder, string name, string subFolder, bool openSubs)
        {
            FolderPath = folder;
            BudgetName = name;
            SubCatPath = subFolder;
            OpenSubCategories = openSubs;
        }
        #endregion
    }
}
