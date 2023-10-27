using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace WpfApp2
{
    public sealed class TabItem
    {
        public required string Header { get; set; }
        public required DataTable Content { get; set; }
    }

    class Serializer
    {
        public static IList<TabItem>? Deserialize(string a_fileName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<TabItem>));

            TextReader reader = new StreamReader(a_fileName);

            object? obj = deserializer.Deserialize(reader);

            reader.Close();

            return obj as List<TabItem>;
        }

        public static void Serialization(IList<TabItem> a_stations, string a_fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<TabItem>));

            using (var stream = File.OpenWrite(a_fileName))
            {
                serializer.Serialize(stream, a_stations);
            }
        }
    }

    public partial class MainWindow : Window
    {
        ObservableCollection<TabItem> database = new ObservableCollection<TabItem>();

        public MainWindow()
        {
            InitializeComponent();

            tabTables.ItemsSource = database;
        }

        private void CreateDB(object sender, RoutedEventArgs e)
        {
            database.Clear();
        }

        private void SaveDB(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Database";
            dialog.DefaultExt = ".dbase";
            dialog.Filter = "Database files (.dbase)|*.dbase";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                Serializer.Serialization(database.ToList(), filename);
            }
        }

        private void LoadDB(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Database files (.dbase)|*.dbase";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                database.Clear();
                var tablesList = Serializer.Deserialize(filename);
                if (tablesList is List<TabItem> tablesL)
                {
                    foreach (var table in tablesL)
                        database.Add(table);
                }
            }
        }

        private void CreateTable(object sender, RoutedEventArgs e)
        {
            AddTableDialog addDialog = new AddTableDialog();
            var result = addDialog.ShowDialog();

            if (result == true)
            {
                database.Add(new TabItem { Header = addDialog.TabName, Content = new DataTable(addDialog.TabName) });
            }
        }

        private void DeleteTable(object sender, RoutedEventArgs e)
        {
            RemoveTableDialog removeDialog = new RemoveTableDialog();
            var result = removeDialog.ShowDialog();

            try
            {
                if (result == true)
                {
                    bool found = false;
                    for (int i = 0; i < database.Count; i++)
                    {
                        if (database[i].Header == removeDialog.TabName)
                        {
                            database.RemoveAt(i);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        throw new KeyNotFoundException();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Couldn't find such table");
            }
        }

        private void AddColumn(object sender, RoutedEventArgs e)
        {
            AddColumnDialog addDialog = new AddColumnDialog();
            var result = addDialog.ShowDialog();

            if (result == true)
            {
                var index = tabTables.SelectedIndex;
                if (index < 0)
                {
                    return;
                }
                DataTable selectedTable = database[index].Content;

                try
                {
                    Type colType = ColTypes.ColStringToType(addDialog.ColType);
                    DataColumn newCol = new DataColumn(addDialog.ColName + " (" + addDialog.ColType +")", colType);
                    selectedTable.Columns.Add(newCol);

                    tabTables.Items.Refresh();
                }
                catch
                {
                    MessageBox.Show("Couldn't add column");
                }
            }
        }

        private void DeleteColumn(object sender, RoutedEventArgs e)
        {
            RemoveColumnDialog removeDialog = new RemoveColumnDialog();
            var result = removeDialog.ShowDialog();

            if (result == true)
            {
                var index = tabTables.SelectedIndex;
                if (index < 0)
                {
                    return;
                }
                DataTable selectedTable = database[index].Content;

                try
                {
                    Type colType = ColTypes.ColStringToType(removeDialog.ColType);
                    bool found = false;
                    for (int i = 0; i < selectedTable.Columns.Count; i++)
                    {
                        if (selectedTable.Columns[i].ColumnName == removeDialog.ColName + " (" + removeDialog.ColType + ")")
                        {
                            selectedTable.Columns.RemoveAt(i);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        throw new KeyNotFoundException();
                    }

                    tabTables.Items.Refresh();
                }
                catch
                {
                    MessageBox.Show("Couldn't find such column");
                }
            }
        }

        private void RemoveDuplicateRows(object sender, RoutedEventArgs e)
        {
            var index = tabTables.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            var table = database[index].Content;

            List<string> uniqueRowHashes = new List<string>();
            List<DataRow> rowsToRemove = new List<DataRow>();
            StringBuilder rowHashBuilder = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {
                rowHashBuilder.Clear();
                foreach (DataColumn column in table.Columns)
                {
                    rowHashBuilder.Append(row[column.ColumnName].ToString());
                }

                string rowHash = rowHashBuilder.ToString();
                if (uniqueRowHashes.Contains(rowHash))
                {
                    rowsToRemove.Add(row);
                }
                else
                {
                    uniqueRowHashes.Add(rowHash);
                }
            }

            foreach (DataRow row in rowsToRemove)
            {
                table.Rows.Remove(row);
            }

            tabTables.Items.Refresh();
        }
    }
}
