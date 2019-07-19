using System;

namespace BudgetPlannerMainWPF.ViewModels
{
    public class SubCategoryEventArgs : EventArgs
    {
        #region - Fields
        public int SaveCode { get; private set; }
        public string SubCatFileName { get; private set; }
        public string SubCatDirectory { get; private set; }
        #endregion

        #region - Constructors
        public SubCategoryEventArgs(int saveCode)
        {
            SaveCode = saveCode;
        }

        public SubCategoryEventArgs(int saveCode, string dir, string name)
        {
            SaveCode = saveCode;
            SubCatDirectory = dir;
            SubCatFileName = name;
        }
        #endregion
    }
}