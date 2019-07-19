﻿using System.Collections.Generic;
using BudgetPlannerLib.Models;

namespace BudgetPlannerMainWPF.EventModels
{
    public class UpdateSubCatEvent
    {
        #region - Fields
        public List<SubCategory> IncomeSubs { get; private set; }
        public List<SubCategory> ExpenseSubs { get; private set; }
        #endregion

        #region - Constructors
        public UpdateSubCatEvent() { }
        public UpdateSubCatEvent(List<SubCategory> income, List<SubCategory> expense)
        {
            IncomeSubs = income;
            ExpenseSubs = expense;
        }
        #endregion
    }
}