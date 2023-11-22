using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для EditTutors.xaml
    /// </summary>
    public partial class EditTutors : Window
    {
        private string connectionString = MainWindow.connectionString;
        private string OpenTable = MainWindow.OpenTable;

        public EditTutors()
        {
            InitializeComponent();

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var queryListCodeRequest = $"SELECT * FROM {OpenTable}";
            loadElementToComboBox(queryListCodeRequest, "ID_Tutor", IDTutor);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "update " + OpenTable + " set Full_Name=@FullNameTutor, Tutor_Address=@AddressTutor, Tutor_Phone_Number=@NumberTutor, ID_Faculty=@IDFaculty, Position=@Position where ID_Tutor=@ID_Tutor";


                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@ID_Tutor", IDTutor.SelectedIndex + 1);
                cmd.Parameters.AddWithValue("@FullNameTutor", FullNameTutor.Text);
                cmd.Parameters.AddWithValue("@AddressTutor", AddressTutor.Text);
                cmd.Parameters.AddWithValue("@NumberTutor", NumberTutor.Text);
                cmd.Parameters.AddWithValue("@IDFaculty", IDFaculty.Text);
                cmd.Parameters.AddWithValue("@Position", Position.Text);

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

        private void IDTutor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected;
            int.TryParse(IDTutor.Text, out selected);
            ++selected;

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            // Создание запроса
            var query = $"SELECT Full_Name, Tutor_Address, Tutor_Phone_Number, ID_Faculty, Position FROM {OpenTable} WHERE ID_Tutor={selected}";

            // Создание команды
            SqlCommand command = new SqlCommand(query, connection);

            // Добавление параметра
            command.Parameters.AddWithValue("@IDTutor", IDTutor.Text);

            // Выполнение запроса и получение результатов
            SqlDataReader reader = command.ExecuteReader();

            // Проверка, что есть результаты
            if (reader.Read())
            {
                // Заполнение TextBox'ов
                FullNameTutor.Text = reader["Full_Name"].ToString();
                AddressTutor.Text = reader["Tutor_Address"].ToString();
                NumberTutor.Text = reader["Tutor_Phone_Number"].ToString();
                IDFaculty.Text = reader["ID_Faculty"].ToString();
                Position.Text = reader["Position"].ToString();
            }

            // Закрытие подключения
            connection.Close();
        }
    }
}
