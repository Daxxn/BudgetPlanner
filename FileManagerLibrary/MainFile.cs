using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerLibrary
{
    public class MainFile : FileModel
    {
        #region - Fields & Properties
        public string FileName { get; set; } = "No Name";
        public BudgetFile Budget { get; set; }
        public CategoryFile Category { get; set; }
        public PaystubFile Paystub { get; set; }
        #endregion

        #region - Constructors
        public MainFile() : base() { }
        public MainFile(string path) : base(path) { }
        public MainFile(string path, string fileName, BudgetFile budget, CategoryFile category, PaystubFile paystub) : base(path)
        {
            FileName = fileName;
            Budget = budget;
            Category = category;
            Paystub = paystub;
        }
        #endregion

        #region - Methods

        #endregion

        #region - Full Properties

        #endregion
    }
}
