using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerMainWPF.EventModels
{
    public class SaveSubCategoryEvent
    {
        #region - Fields
        public uint ActionCode { get; private set; } = 0;
        public string Path { get; private set; }
        #endregion

        #region - Constructors
        public SaveSubCategoryEvent(uint code)
        {
            ActionCode = code;
        }
        public SaveSubCategoryEvent(uint code, string path)
        {
            ActionCode = code;

            Path = path;
        }
        #endregion
    }
}
