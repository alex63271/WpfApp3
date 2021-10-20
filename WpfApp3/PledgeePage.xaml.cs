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
    /// Логика взаимодействия для PledgeePage.xaml
    /// </summary>
    public partial class PledgeePage : Page
    {
        public PledgeePage()
        {
            InitializeComponent();
            LoadComboBoxRegion();

            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT * FROM Organization JOIN Regions ON Organization.Region = Regions.Region WHERE Organization.Hash =" + Check.HashPledgee.ToString(), connection);
                DbDataReader reader = SELECT.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    NameFull.Text = reader["NameFull"].ToString();
                    OGRN.Text = reader["OGRN"].ToString();                
                    INN.Text = reader["INN"].ToString();
                    RegionPledgee.Text = reader["Region"].ToString();
                    mail.Text = reader["E-mail"].ToString();                   
                }
            }

        }

        async void LoadComboBoxRegion()
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


                    RegionPledgee.ItemsSource = Region_list;
                  
                }
            }
        }

        private void ButtonSavePledgee_Click(object sender, RoutedEventArgs e)
        {

            // проверяем поля на пустоту
            if (string.IsNullOrEmpty(NameFull.Text) || string.IsNullOrEmpty(INN.Text) || string.IsNullOrEmpty(OGRN.Text) || string.IsNullOrEmpty(mail.Text) || string.IsNullOrEmpty(RegionPledgee.Text))
            {
                MessageBox.Show("нужно заполнить все поля");
                return;
            }



            string sqlOrganization = "SELECT * FROM Organization";
            Check.HashPledgee = (NameFull.Text + OGRN.Text + INN.Text + mail.Text).GetHashCode();

            if (Check.CheckOrganization(Check.HashPledgee.ToString())) //проверяем, есть ли в БД такая организация
            {                                                                                                  //если нет, то вносим в БД
                using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sqlOrganization, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                    {
                        using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                        {
                            adapter.FillAsync(ds);

                            DataTable dt = ds.Tables[0]; // создаем объект таблицы на основе кэша
                            DataRow newRow = dt.NewRow();   //создаем объект новой записи в таблице dt
                            newRow["NameFull"] = NameFull.Text;
                            newRow["OGRN"] = OGRN.Text;
                            newRow["INN"] = Convert.ToUInt64(INN.Text);
                            newRow["E-mail"] = mail.Text; 
                            newRow["Region"] = RegionPledgee.SelectedValue;
                            newRow["Hash"] = Check.HashPledgee.ToString();
                            dt.Rows.Add(newRow);
                            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                            adapter.Update(ds);
                        }
                    }
                }
            }

            NavigationService.Navigate(new VehiclePropertyPage());
        }

        private void PledgeeBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PledgorPage());
        }

        private void INN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }

        private void OGRN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }
    }
}
