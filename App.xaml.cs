using System;
using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Type[] extraAllowedTypes = new Type[]
            {
                typeof(ComplexInteger),
                typeof(ComplexReal)
            };
            AppDomain.CurrentDomain.SetData("System.Data.DataSetDefaultAllowedTypes", extraAllowedTypes);
        }
    }
}
