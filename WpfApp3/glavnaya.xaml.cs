using MySql.Data.MySqlClient;
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

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для glavnaya.xaml
    /// </summary>
    public partial class glavnaya : Page
    {
        public glavnaya()
        {
            InitializeComponent();
            string connectionString = "server=localhost;user id=enot;persistsecurityinfo=True;database=TestRUZDI;allowuservariables=True;Password=ctccblecz"; //строка подключения
            string sql = "SELECT RID, ID , IDMU , CONCAT (NAME, \' \' , surname) as ФИО from Person";  // sql-запрос
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

                        //MessageBox.Show(Guid.NewGuid().ToString("N").ToUpper());
                       // h1.ItemsSource = ds.Tables[0].DefaultView;  //читаем из кэша таблицу и запысываем ее в datagrid

                        DataTable dt = ds.Tables[0]; // создаем объект таблицы на основе кэша
                        // добавим новую строку
                        DataRow newRow = dt.NewRow();   //создаем объект новой записи в таблице dt
                        newRow["RID"] = Guid.NewGuid().ToString("N").ToUpper();
                        newRow["ID"] = 94200165;
                        dt.Rows.Add(newRow);    //добавляем запись в таблицу на основе созданного ранее объекта

                        // Изменим значение в столбце Age для первой строки
                        //dt.Rows[0]["Age"] = 17;

                        // создаем объект SqlCommandBuilder
                        MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                        adapter.Update(ds);

                        ds.Clear();
                        // перезагружаем данные
                        adapter.FillAsync(ds);
                        h1.ItemsSource = ds.Tables[0].DefaultView;
                    }
                }


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new aplicant());
        }

       
    }
}
