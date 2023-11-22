    using Microsoft.Data.SqlClient;
using System;
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
    /// Логика взаимодействия для AddStudents.xaml
    /// </summary>
    public partial class AddStudents : Window
    {
        public AddStudents()
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

                string query = "INSERT INTO Students (ID_Of_The_Students_Pass, Full_Name, ID_Faculty, Room_Number, [Group], ID_Tutor) VALUES " +
        "(@ID_Of_The_Students_Pass, @Full_Name, @ID_Faculty, @Room_Number, @Group, @ID_Tutor)";

                // Создайте объект SqlCommand и добавьте параметры
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@ID_Of_The_Students_Pass", ++MainWindow.OpenTable_LastID);
                cmd.Parameters.AddWithValue("@Full_Name", FullNameStudent.Text);
                cmd.Parameters.AddWithValue("@ID_Faculty", IDFaculties.Text);
                cmd.Parameters.AddWithValue("@Room_Number", RoomNumber.Text);
                cmd.Parameters.AddWithValue("@Group", IDGroup.Text);
                cmd.Parameters.AddWithValue("@ID_Tutor", IDTutor.Text);

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
