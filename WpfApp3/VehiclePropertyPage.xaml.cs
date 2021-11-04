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
    /// Логика взаимодействия для VehiclePropertyPage.xaml
    /// </summary>
    public partial class VehiclePropertyPage : Page
    {
        public VehiclePropertyPage()
        {
            InitializeComponent();

            LoadLabel();

        }

        async void LoadLabel()
        {
            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT VIN, Description FROM VehicleProperty WHERE Notificationid = '" + Check.NotificationId.ToString() + "'", connection);
                using (DbDataReader reader = await SELECT.ExecuteReaderAsync())
                { // получаем из БД имущество
                    reader.Read();
                    if (reader.HasRows)
                    {
                        VIN.Text = reader["VIN"].ToString();
                        Description.Text = reader["Description"].ToString();

                    }
                }
            }
        }


            private async void SaveVehicleProperty_Click(object sender, RoutedEventArgs e)
        {
            // проверяем поля на пустоту
            if (string.IsNullOrEmpty(VIN.Text) && string.IsNullOrEmpty(Description.Text) )
            {
                MessageBox.Show("нужно заполнить хотябы одно поле");
                return;
            }



            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                string sqlVehicleProperty = "SELECT * FROM VehicleProperty";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(sqlVehicleProperty, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                {
                    using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                    {
                        await adapter.FillAsync(ds);


                        DataTable dt = ds.Tables[0]; // создаем объект таблицы на основе кэша

                        DataRow newRow = dt.NewRow();   //создаем объект новой записи в таблице dt
                        newRow["VIN"] = VIN.Text;
                        newRow["Description"] = Description.Text;
                        newRow["Notificationid"] = Check.NotificationId;
                        dt.Rows.Add(newRow);
                        MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                        adapter.Update(ds);


                    }
                }
            }

            NavigationService.Navigate(new PledgeContractPage());
        }

        private void VehicleBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PledgeePage());
        }
    }
}
