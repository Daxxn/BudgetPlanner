using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerMainWPF.Structs
{
    public class AddToSelector
    {
        uint _code;
        string _display;

        public AddToSelector(uint code, string disp)
        {
            Code = code;
            Display = disp;
        }

        public string Display
        {
            get { return _display; }
            set
            {
                _display = value;
            }
        }

        public uint Code
        {
            get { return _code; }
            set
            {
                _code = value;
            }
        }
    }
}
