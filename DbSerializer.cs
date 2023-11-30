using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLocal
{
    public class DbSerializer
    {
        static public void SaveListToSQLite(List<DataTable> dataTables, string dbName)
        {
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbName};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    DropExistingTables(command);

                    foreach (DataTable dataTable in dataTables)
                    {
                        CreateTable(command, dataTable.TableName, dataTable.Columns);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            InsertRow(command, dataTable.TableName, row);
                        }
                    }
                }
            }
        }

        static void DropExistingTables(SQLiteCommand command)
        {
            List<string> tableNames = GetTableNames(command);

            foreach (string tableName in tableNames)
            {
                command.CommandText = $"DROP TABLE IF EXISTS [{tableName}]";
                command.ExecuteNonQuery();
            }
        }

        static public List<DataTable> ReadListFromSQLite(string dbName)
        {
            List<DataTable> retrievedDataTables = new List<DataTable>();

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={dbName};Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    List<string> tableNames = GetTableNames(command);

                    foreach (string tableName in tableNames)
                    {
                        DataTable dataTable = ReadFromSQLite(command, tableName);
                        retrievedDataTables.Add(dataTable);
                    }
                }
            }

            return retrievedDataTables;
        }

        static List<string> GetTableNames(SQLiteCommand command)
        {
            List<string> tableNames = new List<string>();

            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableNames.Add(reader["name"].ToString());
                }
            }

            return tableNames;
        }

        static DataTable ReadFromSQLite(SQLiteCommand command, string tableName)
        {
            DataTable dataTable = new DataTable();

            command.CommandText = $"SELECT * FROM [{tableName}]";

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                dataTable.Load(reader);
            }

            return dataTable;
        }

        static void CreateTable(SQLiteCommand command, string tableName, DataColumnCollection columns)
        {
            command.CommandText = $"CREATE TABLE IF NOT EXISTS [{tableName}] ({GetColumnDefinitions(columns)})";
            command.ExecuteNonQuery();
        }

        static void InsertRow(SQLiteCommand command, string tableName, DataRow row)
        {
            command.CommandText = $"INSERT INTO [{tableName}] ({GetColumnNames(row)}) VALUES ({GetParamNames(row)})";

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                command.Parameters.AddWithValue($"@p{i}", row[i]);
            }

            command.ExecuteNonQuery();

            command.Parameters.Clear();
        }

        static string GetColumnDefinitions(DataColumnCollection columns)
        {
            string columnDefinitions = string.Join(", ", columns.Cast<DataColumn>().Select(c => $"[{c.ColumnName}] {GetSqlType(c.DataType)}"));
            return columnDefinitions;
        }

        static string GetColumnNames(DataRow row)
        {
            string columnNames = string.Join(", ", row.Table.Columns.Cast<DataColumn>().Select(c => $"[{c.ColumnName}]"));
            return columnNames;
        }

        static string GetParamNames(DataRow row)
        {
            string paramNames = string.Join(", ", Enumerable.Range(0, row.Table.Columns.Count).Select(i => $"@p{i}"));
            return paramNames;
        }

        static string GetSqlType(Type dataType)
        {
            if (dataType == typeof(int) || dataType == typeof(Int64))
            {
                return "INTEGER";
            }
            else if (dataType == typeof(string) || dataType == typeof(char))
            {
                return "TEXT";
            }
            else if (dataType == typeof(double))
            {
                return "REAL";
            }
            throw new ArgumentException($"Unsupported data type: {dataType.Name}");
        }
    }
}
