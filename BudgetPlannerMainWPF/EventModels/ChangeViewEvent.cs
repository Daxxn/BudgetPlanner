﻿namespace BudgetPlannerMainWPF.EventModels
{
    public enum PrevView
    {
        DataView = 1,
        CategoryView = 2
    }

    public class ChangeViewEvent
    {
        #region - Fields
        public PrevView PreviousView { get; private set; }
        #endregion

        #region - Constructors
        public ChangeViewEvent(PrevView prevView)
        {
            PreviousView = prevView;
        }
        #endregion
    }
}