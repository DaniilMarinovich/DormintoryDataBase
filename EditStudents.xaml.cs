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
    /// Логика взаимодействия для EditStudents.xaml
    /// </summary>
    /// 
    public partial class EditStudents : Window
    {
        private string connectionString = MainWindow.connectionString;
        private string OpenTable = MainWindow.OpenTable;

        public EditStudents()
        {
            InitializeComponent();

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var queryListCodeRequest = $"SELECT * FROM {OpenTable}";
            loadElementToComboBox(queryListCodeRequest, "ID_Of_The_Students_Pass", IDStudent);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "update " + OpenTable + " set Full_Name=@FullNameStudent, ID_Faculty=@IDFaculties, Room_Number=@RoomNumber, [Group]=@IDGroup, ID_Tutor=@IDTutor where ID_Of_The_Students_Pass=@IDStudent";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@IDStudent", IDStudent.SelectedIndex + 1);
                cmd.Parameters.AddWithValue("@FullNameStudent", FullNameStudent.Text);
                cmd.Parameters.AddWithValue("@IDFaculties", IDFaculties.Text);
                cmd.Parameters.AddWithValue("@RoomNumber", RoomNumber.Text);
                cmd.Parameters.AddWithValue("@IDGroup", IDGroup.Text);
                cmd.Parameters.AddWithValue("@IDTutor", IDTutor.Text);

                cmd.ExecuteNonQuery();
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

        private void IDStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT * FROM " + OpenTable + " WHERE ID_Of_The_Students_Pass=@IDStudent";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@IDStudent", IDStudent.SelectedIndex + 1);

            connection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                FullNameStudent.Text = reader["Full_Name"].ToString();
                IDFaculties.Text = reader["ID_Faculty"].ToString();
                RoomNumber.Text = reader["Room_Number"].ToString();
                IDGroup.Text = reader["Group"].ToString();
                IDTutor.Text = reader["ID_Tutor"].ToString();
            }
            
            reader.Close();
            connection.Close();
        }
    }
}
