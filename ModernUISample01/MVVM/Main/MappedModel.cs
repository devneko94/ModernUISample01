using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernUISample01.MVVM.Main
{
    public class MappedModel
    {
        public string TableNameJP { get; set; }
        public string TableName { get; set; }
        public string GroupName { get; set; }
        public string FilePath { get; set; }

        public string[] GetPropertyNames()
        {
            return GetType().GetProperties().Select(x => x.Name).ToArray();
        }

        public string[] GetValuesArray()
        {
            string[] texts = new string[4] { TableNameJP, TableName, GroupName, FilePath };
            return texts;
        }
    }
}
