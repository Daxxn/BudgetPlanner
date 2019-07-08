using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetPlannerMainWPF.EventModels
{
    public class EnterEvent
    {
        #region - Fields
        public Key PressedKey { get; private set; }
        #endregion

        #region - Constructors
        public EnterEvent(Key pressedKey)
        {
            PressedKey = pressedKey;
        }
        #endregion

        #region - Methods

        #endregion

        #region - Properties

        #endregion
    }
}
