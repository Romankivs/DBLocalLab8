using System;
using System.Collections.Generic;
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
    /// Interaction logic for RemoveColumnDialog.xaml
    /// </summary>
    public partial class RemoveColumnDialog : Window
    {
        public RemoveColumnDialog()
        {
            InitializeComponent();
        }

        private void Button_Remove(object sender, RoutedEventArgs e)
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
