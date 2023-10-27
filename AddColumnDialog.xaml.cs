using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for AddColumnDialog.xaml
    /// </summary>
    public partial class AddColumnDialog : Window
    {
        public AddColumnDialog()
        {
            InitializeComponent();
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string ColName
        {
            get { return ColumnName.Text; }
        }

        public string ColType
        {
            get { return (string)ColumnType.SelectedItem; }
        }
    }
}
