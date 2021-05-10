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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "server=localhost;user id=enot;persistsecurityinfo=True;database=enotdb;allowuservariables=True;Password=ctccblecz"; //строка подключения
            string sql = "SELECT ID as Федералка, IDMU as \"Рег. номер\", CONCAT (NAME, \" \" , surname) AS ФИО from Notaries";  // sql-запрос
            using (MySqlConnection connection = new MySqlConnection(connectionString))  //создаем объект подключения к mysql
            {
                // Создаем объект DataAdapter
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                {
                    // Создаем объект DataSet

                    using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                    {
                        
                        adapter.FillAsync(ds);  // метод адаптера заполняет кэш
                        // Отображаем данные

                        //ds.Tables[0].Columns[0].ColumnName; //имя первого столбца в таблице №1 в коллекции ds.Tables
                        //ds.Tables[0].Rows[0].ItemArray[0]; // значение первой ячейки первой строки
                        h1.Columns.Clear();

                        h1.ItemsSource = ds.Tables[0].DefaultView;  //читаем из кэша стоблцы и строки

                    }
                }
               
      
            }
             
         
        }

        
    }
}
