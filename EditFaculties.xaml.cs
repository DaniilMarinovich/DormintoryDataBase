using Microsoft.Data.SqlClient;
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
    /// Логика взаимодействия для EditFaculties.xaml
    /// </summary>
    public partial class EditFaculties : Window
    {
        private string connectionString = MainWindow.connectionString;
        private string OpenTable = MainWindow.OpenTable;
        public EditFaculties()
        {
            InitializeComponent();

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var queryListCodeRequest = $"SELECT * FROM {OpenTable}";
            loadElementToComboBox(queryListCodeRequest, "ID_Faculty", IDFaculties);
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "UPDATE " + OpenTable + " SET Faculty_Name=@NameFaculties, Deans_Full_Name=@FullNameDean, Deans_Phone_Number=@NumberDean WHERE ID_Faculty=@IDFaculties";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@IDFaculties", IDFaculties.Text);
                cmd.Parameters.AddWithValue("@NameFaculties", NameFaculties.Text);
                cmd.Parameters.AddWithValue("@FullNameDean", FullNameDean.Text);
                cmd.Parameters.AddWithValue("@NumberDean", NumberDean.Text);

                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                MessageBox.Show("Э! Нармалино ввэды!");
            }
            
            Close();
        }

        private void IDFaculties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT * FROM " + OpenTable + " WHERE ID_Faculty=@IDFaculties";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@IDFaculties", IDFaculties.SelectedIndex + 1);

            connection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                NameFaculties.Text = reader["Faculty_Name"].ToString();
                FullNameDean.Text = reader["Deans_Full_Name"].ToString();
                NumberDean.Text = reader["Deans_Phone_Number"].ToString();
            }

            reader.Close();
            connection.Close();
        }
    }
}
