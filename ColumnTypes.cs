using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class ColTypes : ObservableCollection<string>
    {
        public static Type ColStringToType(string type)
        {
            Type colType = typeof(string);
            switch (type)
            {
                case "String": colType = typeof(string); break;
                case "Integer": colType = typeof(int); break;
                case "Real": colType = typeof(double); break;
                case "Char": colType = typeof(char); break;
                case "Complex Integer": colType = typeof(ComplexInteger); break;
                case "Complex Real": colType = typeof(ComplexReal); break;
            }
            return colType;
        }

        public ColTypes()
        {
            Add("String");
            Add("Char");
            Add("Integer");
            Add("Real");
            Add("Complex Integer");
            Add("Complex Real");
        }
    }

}
