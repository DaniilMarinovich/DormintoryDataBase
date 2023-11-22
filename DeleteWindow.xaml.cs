using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
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

namespace Lab8
{
    /// <summary>
    /// Логика взаимодействия для DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        private string categoryToDel = "";
        private string connectionString = MainWindow.connectionString;
        private string OpenTable = MainWindow.OpenTable;
        public DeleteWindow()
        {
            InitializeComponent();

            switch (OpenTable)
            {
                case "Students":
                    categoryToDel = "Full_Name";
                    break;

                case "Tutors":
                    categoryToDel = "Full_Name";
                    break;

                case "Faculties":
                    categoryToDel = "Faculty_Name";
                    break;
            }

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var queryListCodeRequest = $"SELECT * FROM {OpenTable}";
            loadElementToComboBox(queryListCodeRequest, categoryToDel, DeleteBox);
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создание подключения
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                if (DeleteBox.SelectedIndex == -1)
                {
                    System.Windows.MessageBox.Show("Э! Индекс выбери!");
                }
                else
                {
                    string query = $"DELETE from {OpenTable} where CAST({categoryToDel} as varchar(max))='" + DeleteBox.SelectedIndex + "'";
                    SqlCommand createCommand = new SqlCommand(query, connection);
                    createCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Э! Нармалино ввэды!");
            }
            
            Close();
        }

        public void loadElementToComboBox(string stringQuery, string column, ComboBox myBox)
        {
            List<string> columnValues = GetColumnValues(stringQuery, column);
            foreach (string value in columnValues) { myBox.Items.Add(value); }
        }
        public List<string> GetColumnValues(string query, string columnName)
        {
            List<string> columnValues = new List<string>();

            SqlConnection myCon = new SqlConnection(connectionString);
            myCon.Open();
            using (SqlCommand command = new SqlCommand(query, myCon))
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    object columnValueObject = reader.GetValue(reader.GetOrdinal(columnName));
                    string columnValue = columnValueObject != DBNull.Value ? columnValueObject.ToString() : "";
                    columnValues.Add(columnValue);
                }
            }
            return columnValues;
        }
    }
}
