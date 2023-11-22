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
    /// Логика взаимодействия для AddFaculties.xaml
    /// </summary>
    public partial class AddFaculties : Window
    {
        public AddFaculties()
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

                string query = "INSERT INTO Students (ID_Faculty, Faculty_Name, Deans_Full_Name, Deans_Phone_Number) VALUES " +
        "(@ID_Faculty, @Faculty_Name, @Deans_Full_Name, @Deans_Phone_Number)";

                // Создайте объект SqlCommand и добавьте параметры
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@ID_Faculty", ++MainWindow.OpenTable_LastID);
                cmd.Parameters.AddWithValue("@Faculty_Name", NameFaculties.Text);
                cmd.Parameters.AddWithValue("@Deans_Full_Name", FullNameDean.Text);
                cmd.Parameters.AddWithValue("@Deans_Phone_Number", NumberDean.Text);

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
