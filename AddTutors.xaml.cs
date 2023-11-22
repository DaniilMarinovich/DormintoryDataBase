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
    /// Логика взаимодействия для AddTutors.xaml
    /// </summary>
    public partial class AddTutors : Window
    {
        public AddTutors()
        {
            InitializeComponent();
        }

        private string connectionString = MainWindow.connectionString;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO Students (ID_Tutor, Full_Name, Tutor_Address, Tutor_Phone_Number, ID_Faculty, Position) VALUES " +
        "(@ID_Tutor, @Full_Name, @Tutor_Address, @Tutor_Phone_Number, @ID_Faculty, @Position)";

                // Создайте объект SqlCommand и добавьте параметры
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@ID_Tutor", ++MainWindow.OpenTable_LastID);
                cmd.Parameters.AddWithValue("@Full_Name", FullNameTutor.Text);
                cmd.Parameters.AddWithValue("@Tutor_Address", AddressTutor.Text);
                cmd.Parameters.AddWithValue("@Tutor_Phone_Number", NumberTutor.Text);
                cmd.Parameters.AddWithValue("@ID_Faculty", IDFaculty.Text);
                cmd.Parameters.AddWithValue("@Position", Position.Text);

                // Выполните запрос
                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                MessageBox.Show("Э! Нармалино ввэды!");
            }

            Close();
        }
    }
}
