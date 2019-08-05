using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerMainWPF
{
    public interface IExceptionLogger
    {
        void Info(string format, params object[] args);
        void Warn(string format, params object[] args);
        void Error(Exception exception);
    }
}
