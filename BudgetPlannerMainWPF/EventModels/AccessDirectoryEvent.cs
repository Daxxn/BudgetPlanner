﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerMainWPF.EventModels
{
    public class AccessDirectoryEvent
    {
        #region - Fields
        public string Description { get; private set; }
        #endregion

        #region - Constructors
        public AccessDirectoryEvent(string desc)
        {
            Description = desc;
        }
        #endregion
    }
}
