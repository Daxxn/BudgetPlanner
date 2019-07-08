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
        public string BudgetName_E { get; private set; }
        public string MainFolder_E { get; private set; }
        public string SubCatFolder_E { get; private set; }
        public bool OpenSubCategories_E { get; private set; }
        #endregion

        #region - Constructors
        public NewBudgetEvent(string budgetName, string mainFolder,
            string subCatFolder, bool openSubs)
        {
            BudgetName_E = budgetName;
            MainFolder_E = mainFolder;
            SubCatFolder_E = subCatFolder;
            OpenSubCategories_E = openSubs;
        }

        public NewBudgetEvent(string budgetName, string mainFolder, string subCatFolder)
        {
            BudgetName_E = budgetName;
            MainFolder_E = mainFolder;
            SubCatFolder_E = subCatFolder;
        }
        #endregion
    }
}
