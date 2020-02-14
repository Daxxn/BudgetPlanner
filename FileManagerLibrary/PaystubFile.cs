using PaystubLibrary;

namespace FileManagerLibrary
{
    public class PaystubFile : FileModel
    {
        #region - Fields & Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public Paystub[] Paystubs { get; set; }
        #endregion

        #region - Constructors
        public PaystubFile() : base() { }
        public PaystubFile(string path) : base(path) { }
        public PaystubFile(string path, Paystub[] paystubs) : base(path)
        {
            Paystubs = paystubs;
        }
        #endregion

        #region - Methods

        #endregion

        #region - Full Properties

        #endregion
    }
}