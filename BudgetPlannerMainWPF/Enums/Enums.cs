using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerMainWPF.Enums
{
    public enum AddTo
    {
        AddToFront = 1,
        AddToEnd = 2,
        CreateNew = 3
    }

    public enum ExtensionType
    {
        Main,
        Budget,
        Category,
        Paystub
    }
}
