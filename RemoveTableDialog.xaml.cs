﻿using System;
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
    /// Interaction logic for RemoveTableDialog.xaml
    /// </summary>
    public partial class RemoveTableDialog : Window
    {
        public RemoveTableDialog()
        {
            InitializeComponent();
        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string TabName
        {
            get { return TableName.Text; }
        }
    }
}
