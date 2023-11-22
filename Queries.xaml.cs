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
    /// Логика взаимодействия для Queries.xaml
    /// </summary>
    public partial class Queries : Window
    {
        string connectionString = ("Data Source=USER-PC;Initial Catalog=ZHEY;Trusted_Connection=True;TrustServerCertificate=True;");
        SqlDataAdapter adapter;
        SqlConnection connection = null;
        DataTable queryTable1 = new DataTable();
        DataTable queryTable2 = new DataTable();
        DataTable queryTable3 = new DataTable();
        DataTable queryTable4 = new DataTable();
        DataTable queryTable5 = new DataTable();
        public Queries()
        {
            InitializeComponent();
        }
        public void ConnectToDB(string sql, DataTable table)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                table.Clear();
                adapter.Fill(table);
                QueryGrid.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void Query1btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "SELECT FIO_wor FROM Workers, Completed_work WHERE Completed_work.Worker_ID = Workers.Worker_ID AND Date = '" + this.Query1txt.Text + "'";
                ConnectToDB(sql, queryTable1);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "SELECT Service_name FROM Service, Completed_work WHERE Service.Service_ID = Completed_work.Service_ID AND Kol > 2";
                ConnectToDB(sql, queryTable2);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "SELECT Number FROM Completed_work Where Completed_work.Worker_ID = '" + this.Query3IDtxt.Text + "' AND Date > '" + this.Query3Date1txt.Text + "' AND Date < '" + this.Query3Date2txt.Text + "'";
                ConnectToDB(sql, queryTable3);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateTime = new DateTime(year: int.Parse(Yeartxt.Text), month: int.Parse(Monthtxt.Text), 1);
                var dateTime2 = dateTime.AddMonths(1).AddDays(-1);
                string sql = "SELECT FIO_wor, COUNT(Completed_work.Service_ID), SUM(summa) FROM Service, Completed_work, Workers WHERE Service.Service_ID = Completed_work.Service_ID AND Workers.Worker_ID = Completed_work.Worker_ID AND Date >= '"+ dateTime +"' AND Date <= '"+ dateTime2 +"' GROUP BY FIO_wor";
                ConnectToDB(sql, queryTable4);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "SELECT Service_name, Date, Kol FROM Service, Completed_work WHERE Service.Service_ID = Completed_work.Service_ID GROUP BY Service_name, Date, Kol ORDER BY Service_name";
                ConnectToDB(sql, queryTable5);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
