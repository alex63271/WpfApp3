using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
    /// Логика взаимодействия для PledgorPage.xaml
    /// </summary>
    public partial class PledgorPage : Page
    {
        public PledgorPage()
        {
            InitializeComponent();
            LoadComboBoxRegionAsync();    //выполнем асинхронный метод заполнения комбобокса "Регионы" у залогодателя и залогодержателя

            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {


                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT * FROM Person JOIN Regions ON Person.Region = Regions.Region WHERE Person.Hash = " + Check.HashPledgor.ToString(), connection);
                DbDataReader reader = SELECT.ExecuteReader(); // получаем из БД залогодателя по его хэшу
                reader.Read();
                if (reader.HasRows)
                {
                    Last.Text = reader["Last"].ToString();
                    First.Text = reader["First"].ToString();
                    Middle.Text = reader["Middle"].ToString();
                    BirthDate.SelectedDate = Convert.ToDateTime(reader["BirthDate"]);
                    Region.Text = reader["Region"].ToString();
                    Number_passport.Text = reader["PersonDocument"].ToString();
                }
                else 
                {
                  
                    Check.NotificationId = Guid.NewGuid().ToString(); // запуск первой страницы создания уведомления - генеируем NotificationId

                }

            }

        }

        async void LoadComboBoxRegionAsync()
        {
            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {

                using (connection)
                {
                    List<string> Region_list = new List<string>();  //создание списка регионов

                    connection.Open();
                    MySqlCommand SELECT = new MySqlCommand("SELECT Region FROM Regions", connection);

                    DbDataReader reader = await SELECT.ExecuteReaderAsync(); // получаем из БД регионы

                    while (reader.Read())
                    {
                        Region_list.Add(reader["Region"].ToString());    // записываем регионы в список

                    }
                    Region_list.Sort();
                    reader.Close();


                    Region.ItemsSource = Region_list;
                    //RegionPledgee.ItemsSource = Region_list;
                }
            }
        }

        private void PledgorButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlPerson = "SELECT * FROM Person";


            if (Check.CheckPerson((Last.Text + First.Text + Middle.Text + BirthDate.SelectedDate).GetHashCode().ToString())) //проверяем, есть ли в БД такое физ.лицо
            {                                                                                                                    // если нет, то вносим в БД
                Check.HashPledgor = (Last.Text + First.Text + Middle.Text + BirthDate.SelectedDate).GetHashCode();
                using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
                {
                    using (MySqlDataAdapter adapterperson = new MySqlDataAdapter(sqlPerson, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                    {
                        using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                        {

                            adapterperson.FillAsync(ds);

                            DataTable dt = ds.Tables[0]; // создаем объект таблицы на основе кэша

                            DataRow newRow = dt.NewRow();   //создаем объект новой записи в таблице dt
                            newRow["Last"] = Last.Text;
                            newRow["First"] = First.Text;
                            newRow["Middle"] = Middle.Text;
                            newRow["BirthDate"] = BirthDate.SelectedDate;
                            newRow["PersonDocument"] = Number_passport.Text;
                            newRow["Region"] = Region.SelectedValue;
                            newRow["Hash"] = Check.HashPledgor.ToString();
                            dt.Rows.Add(newRow);
                            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapterperson);
                            adapterperson.Update(ds);

                        }
                    }

                }
            }
            //CreateUZ1Win.DialogResult = true;
            NavigationService.Navigate(new PledgeePage());
        }
    }
}
