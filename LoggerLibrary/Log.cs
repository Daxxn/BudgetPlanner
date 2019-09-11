using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLibrary
{
    public class Log
    {
        #region - Fields
        public Exception Error { get; set; }
        public DateTime ErrorTime { get; set; }
        #endregion

        #region - Constructors
        public Log() { }
        public Log(Exception error, DateTime errorTime)
        {
            this.Error = error;
            this.ErrorTime = errorTime;
        }
        #endregion

        #region - Methods
        public override string ToString()
        {
            return $"Time of Exception:\n{ErrorTime.ToLongDateString()}  {ErrorTime.ToLongTimeString()}\nException:\n{Error.ToString()}";
        }
        #endregion

        #region - Properties

        #endregion
    }
}
