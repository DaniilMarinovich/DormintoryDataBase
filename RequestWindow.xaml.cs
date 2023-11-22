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
    /// Логика взаимодействия для RequestWindow.xaml
    /// </summary>
    public partial class RequestWindow : Window
    {

        string OpenTable = MainWindow.OpenTable;
        string connectionString = MainWindow.connectionString;
        string selectedItemText = "";
        public RequestWindow()
        {
            InitializeComponent();
        }

        void Hide()
        {
            SelectBox.Visibility = Visibility.Hidden;
        }
        void Show()
        {
            SelectBox.Visibility = Visibility.Visible;
        }
        public async Task FillDataGrid(string query)
        {
            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var queryListCodeRequest = $"SELECT * FROM {OpenTable}";

            // Из какой таблицы нужен вывод 
            SqlCommand createCommand = new SqlCommand(query, connection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable(); // В скобках указываем название таблицы
            dataAdp.Fill(dt);
            DataGrid.ItemsSource = dt.DefaultView; // Сам вывод 

        }

        public HashSet<string> GetColumnValues(string query, string columnName)
        {
            HashSet<string> columnValues = new HashSet<string>();

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

            return columnValues.OrderBy(x => x).ToHashSet();
        }
        public void loadElementToComboBox(string stringQuery, string column, ComboBox myBox)
        {
            HashSet<string> columnValues = GetColumnValues(stringQuery, column);
            foreach (string value in columnValues) { myBox.Items.Add(value); }
        }
        private void Do_Button_Click(object sender, RoutedEventArgs e)
        {
            string query = "";
            switch (RequestBox.SelectedIndex)
            {
                case 0:
                    query = $"SELECT Full_Name FROM Students where Room_Number={SelectBox.SelectedItem}";
                    break;

                case 1:
                    Show();
                    query = $"SELECT Full_Name, Room_Number FROM Students, Tutors where Tutors.Full_name LIKE '%{SelectBox.SelectedItem}%'";
                    break;

                case 2:
                    query = $"SELECT Faculty_Name, Room_Number, COUNT(Students.) FROM Students, Tutors where Tutors.Full_name LIKE '%{SelectBox.SelectedItem}%'";
                    break;

                case 3:
                    break;

                case 4:
                    break;

                default:
                    break;
            }
        }

        private void RequestBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Hide();
            SelectBox.Items.Clear();
            switch (RequestBox.SelectedIndex)
            {
                case 0:
                    Show();
                    loadElementToComboBox("SELECT * FROM Students", "Room_Number", SelectBox);
                    break;

                case 1:
                    Show();
                    loadElementToComboBox("SELECT * FROM Tutors", "Full_Name", SelectBox);
                    break;

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;

                default:
                    break;
            }

            
        }
    }
}
