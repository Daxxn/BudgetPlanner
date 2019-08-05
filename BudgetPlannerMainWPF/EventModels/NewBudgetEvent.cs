using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerMainWPF.EventModels
{
    public class NewBudgetEvent
    {
        #region - Fields
        public string BudgetName { get; private set; }
        public string MainFolder { get; private set; }
        public string SubCatFolder { get; private set; }
        public bool OpenSubCategories { get; private set; }
        #endregion

        #region - Constructors
        public NewBudgetEvent(string budgetName, string mainFolder,
            string subCatFolder, bool openSubs)
        {
            BudgetName = budgetName;
            MainFolder = mainFolder;
            SubCatFolder = subCatFolder;
            OpenSubCategories = openSubs;
        }

        public NewBudgetEvent(string budgetName, string mainFolder, string subCatFolder)
        {
            BudgetName = budgetName;
            MainFolder = mainFolder;
            SubCatFolder = subCatFolder;
        }
        #endregion
    }
}
