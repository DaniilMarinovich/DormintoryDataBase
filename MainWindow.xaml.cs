using System;
using Microsoft.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;

namespace Lab8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string OpenTable = "";
        public static int OpenTable_LastID = 0;
        public static string connectionString = "Server=DOMANSTIK;Database=Dormitory_No_2;Trusted_Connection=True;TrustServerCertificate=True;";

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
        public MainWindow()
        {

            InitializeComponent();
            FilterAndSortBox.Visibility = Visibility.Hidden;
        }
        void Hide()
        {
            FilterBox_.Visibility = Visibility.Hidden;
            Label1.Visibility = Visibility.Visible;
            Label2.Visibility = Visibility.Visible;
            FilterStart.Visibility = Visibility.Visible;
            FilterFinish.Visibility = Visibility.Visible;
        }
        void Show()
        {
            FilterBox_.Visibility = Visibility.Visible;
            Label1.Visibility = Visibility.Hidden;
            Label2.Visibility = Visibility.Hidden;
            FilterStart.Visibility = Visibility.Hidden;
            FilterFinish.Visibility = Visibility.Hidden;
        }
        public async Task FillDataGrid(string tableName)
        {
            FilterBox.Items.Clear();

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query;
            var queryListCodeRequest = $"SELECT * FROM {OpenTable}";

            switch (tableName)
            {
                case "Students":
                    Hide();
                    query = $"SELECT Full_Name, ID_Faculty, Room_Number, [Group], ID_Tutor FROM {tableName}";
                    break;

                case "Tutors":
                    Show();
                    loadElementToComboBox(queryListCodeRequest, "ID_Faculty", FilterBox);
                    query = $"SELECT Full_Name,Tutor_Address,Tutor_Phone_Number,ID_Faculty,Position FROM {tableName}";
                    break;

                case "Faculties":
                    Show();
                    loadElementToComboBox(queryListCodeRequest, "Deans_Full_Name", FilterBox);
                    query = $"SELECT Faculty_Name, Deans_Full_Name, Deans_Phone_Number FROM {tableName}";
                    break;

                default:
                    query = $"SELECT * FROM {tableName}";
                    break;
            }
             // Из какой таблицы нужен вывод 
            SqlCommand createCommand = new SqlCommand(query, connection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable(); // В скобках указываем название таблицы
            dataAdp.Fill(dt);
            DataGrid.ItemsSource = dt.DefaultView; // Сам вывод 

            OpenTable_LastID = dt.Rows.Count;

            FilterStart.Text = "";
            FilterFinish.Text = "";

        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (OpenTable)
            {
                case "Students":
                    AddStudents addStudents = new AddStudents();
                    addStudents.ShowDialog();
                    break;

                case "Tutors":
                    AddTutors addTutors = new AddTutors();
                    addTutors.ShowDialog();
                    break;

                case "Faculties":
                   AddFaculties addFaculties = new AddFaculties();
                    addFaculties.ShowDialog();
                    break;

                default:
                    System.Windows.MessageBox.Show("Э! Таблицу выбери!");
                    break;
            }

            FillDataGrid(OpenTable);
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (OpenTable == "") System.Windows.MessageBox.Show("Э! Таблицу выбери!");
            else
            {
                DeleteWindow deleteWindow = new DeleteWindow();
                deleteWindow.ShowDialog();

                FillDataGrid(OpenTable);
            }
        }
        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            switch (OpenTable)
            {
                case "Students":
                    EditStudents editStudents = new EditStudents();
                    editStudents.ShowDialog();
                    Console.Text = editStudents.IDStudent.Text;
                    break;

                case "Tutors":
                    EditTutors editTutors = new EditTutors();
                    editTutors.ShowDialog();
                    break;

                case "Faculties":
                    EditFaculties editFaculties = new EditFaculties();
                    editFaculties.ShowDialog();
                    break;

                default:
                    System.Windows.MessageBox.Show("Э! Таблицу выбери!");
                    break;
            }

            FillDataGrid(OpenTable);
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FilterAndSortBox.Visibility = Visibility.Visible;

            MenuItem menuItem = (MenuItem)sender;

            Console.Text = menuItem.Header.ToString();

            switch (menuItem.Header.ToString())
            {
                case "Студенты":
                    OpenTable = "Students";
                    FillDataGrid("Students");
                    break;

                case "Кураторы":
                    OpenTable = "Tutors";
                    FilterBox_.Header = "Номер факультета";
                    FillDataGrid("Tutors");
                    break;

                case "Факультеты":
                    OpenTable = "Faculties";
                    FilterBox_.Header = "Первая буква имени декана";
                    FillDataGrid("Faculties");
                    break;

                case "Запросы":
                    RequestWindow requestWindow = new RequestWindow();
                    requestWindow.ShowDialog();
                    break;
            }
        }
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string query;
                DataGrid.ItemsSource = null;
                DataGrid.Columns.Clear();

                switch (OpenTable)
                {
                    case "Students":
                        query = $"SELECT Full_Name, ID_Faculty, [Group], ID_Tutor FROM {OpenTable} where Room_Number >= {FilterStart.Text} AND Room_Number <= {FilterFinish.Text}";
                        break;

                    case "Tutors":
                        query = $"SELECT Full_Name,Tutor_Address,Tutor_Phone_Number,Position FROM {OpenTable} where ID_Faculty ={FilterBox.SelectedIndex + 1}";
                        break;

                    case "Faculties":
                        query = $"SELECT Faculty_Name, Deans_Full_Name, Deans_Phone_Number FROM {OpenTable} where Deans_Full_Name LIKE '{FilterBox.Text[0]}%'";
                        break;

                    default:
                        query = $"SELECT * FROM {OpenTable}";
                        break;
                }

                OpenTable = "";
                SqlCommand createCommand = new SqlCommand(query, connection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable(); // В скобках указываем название таблицы
                dataAdp.Fill(dt);
                DataGrid.ItemsSource = dt.DefaultView; // Сам вывод 
            }
            catch (Exception)
            {
                MessageBox.Show("Э! Нармалино ввэды!");
            }

            OpenTable = "";
            FilterBox.Items.Clear();
            FilterAndSortBox.Visibility = Visibility.Hidden;

        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DataGrid.ItemsSource = null;
            OpenTable = "";
            FilterAndSortBox.Visibility = Visibility.Hidden;
        }
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string query;
                DataGrid.ItemsSource = null;
                DataGrid.Columns.Clear();

                switch (OpenTable)
                {
                    case "Students":
                        query = $"SELECT Full_Name, ID_Faculty, [Group], ID_Tutor FROM {OpenTable} ORDER BY Full_Name";
                        break;

                    case "Tutors":
                        query = $"SELECT Full_Name,Tutor_Address,Tutor_Phone_Number,Position FROM {OpenTable} ORDER BY Tutor_Address";
                        break;

                    case "Faculties":
                        query = $"SELECT Faculty_Name, Deans_Full_Name, Deans_Phone_Number FROM {OpenTable} ORDER BY Faculty_Name";
                        break;

                    default:
                        query = $"SELECT * FROM {OpenTable}";
                        break;
                }

                OpenTable = "";
                SqlCommand createCommand = new SqlCommand(query, connection);
                createCommand.ExecuteNonQuery();

                SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                DataTable dt = new DataTable(); // В скобках указываем название таблицы
                dataAdp.Fill(dt);
                DataGrid.ItemsSource = dt.DefaultView; // Сам вывод 
            }
            catch (Exception)
            {
                MessageBox.Show("Э! Нармалино ввэды!");
            }

            OpenTable = "";
            FilterAndSortBox.Visibility = Visibility.Hidden;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}