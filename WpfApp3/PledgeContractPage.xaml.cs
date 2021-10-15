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
    /// Логика взаимодействия для PledgeContractPage.xaml
    /// </summary>
    public partial class PledgeContractPage : Page
    {
        public PledgeContractPage()
        {
            InitializeComponent();

            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {

                connection.Open();
                MySqlCommand SELECTContr = new MySqlCommand("SELECT * FROM Contracts where Hash = '" + Check.HashContract.ToString() + "'", connection);
                DbDataReader read = SELECTContr.ExecuteReader(); // получаем из БД список 
                read.Read();
                if (read.HasRows)
                {
                    Date.SelectedDate = Convert.ToDateTime(read["Data"]);
                    Number.Text = read["Number"].ToString();
                    Name.Text = read["Name"].ToString();
                    TermOfContract.SelectedDate = Convert.ToDateTime(read["TermOfContract"]);
                }
            }
        }

        private async void SaveContract_Click(object sender, RoutedEventArgs e)
        {

            if (Check.CheckContracts((Date.SelectedDate.Value + Number.Text + TermOfContract.SelectedDate.Value).GetHashCode().ToString()))    //проверяем, есть ли в БД такой договор
            {
                Check.HashContract = (Date.SelectedDate.Value + Number.Text + TermOfContract.SelectedDate.Value).GetHashCode();
                using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
                {                                                                                     // если нет, то вносим в БД
                    string sqlContracts = "SELECT * FROM Contracts";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sqlContracts, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                    {
                        using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                        {

                            await adapter.FillAsync(ds);

                            DataTable dt = ds.Tables[0]; // создаем объект таблицы на основе кэша

                            DataRow newRow = dt.NewRow();   //создаем объект новой записи в таблице dt
                            newRow["Data"] = Date.SelectedDate.Value;
                            newRow["Number"] = Number.Text;
                            newRow["Name"] = Name.Text;
                            newRow["TermOfContract"] = TermOfContract.SelectedDate.Value;
                            newRow["Hash"] = Check.HashContract.ToString();
                            dt.Rows.Add(newRow);
                            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                            adapter.Update(ds);

                        }
                    }
                }
            }

            NavigationService.Navigate(new ApplicantPage());
        }

        private void ContractBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new VehiclePropertyPage());
        }
    }
}
