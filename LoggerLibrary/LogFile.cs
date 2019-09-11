using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLibrary
{
    public class LogFile
    {
        #region - Fields
        public string FilePath { get; set; }
        public DateTime CreationTime { get; set; }
        #endregion

        #region - Constructors
        public LogFile() { }
        public LogFile(string filePath, DateTime creationTime)
        {
            this.FilePath = filePath;
            this.CreationTime = creationTime;
        }
        #endregion

        #region - Methods

        #endregion

        #region - Properties

        #endregion
    }
}
