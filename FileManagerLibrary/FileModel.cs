using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerLibrary
{
    public class FileModel
    {
        public string FullPath { get; set; } = "Null";

        public FileModel() { }
        public FileModel(string path)
        {
            FullPath = path;
        }
    }
}
