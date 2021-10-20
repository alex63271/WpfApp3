using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow2.xaml
    /// </summary>
    public partial class SettingsWindow2 : Window
    {
        public SettingsWindow2()
        {
            using (FileStream fs = new FileStream("conf.xml", FileMode.OpenOrCreate))
            {
                //выполняем десериализацию конфига

                XmlSerializer formatter = new XmlSerializer(typeof(Conf));
                Conf xmlconf = (Conf)formatter.Deserialize(fs);

                DataContext = xmlconf; //помещаю нужный объект в DataContext перед InitializeComponent для работы привязки 
                InitializeComponent();  //выполняем инициализацию после создания объекта и помещения его в DataContext
                PasswordBox.Password = xmlconf.password; // заполняю порграммно т.к. привязка не работает с PasswordBox
            }
        }

        private void Button_ОК_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=" + AdressMysql.Text + ";user id=" + Login.Text + ";persistsecurityinfo=True;database=" + DBName.Text + ";allowuservariables=True;Password=" + PasswordBox.Password))  //создаем объект подключения к mysql
                {
                    connection.Open();
                    //using (MySqlDataAdapter adapter = new MySqlDataAdapter(Check.sql, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                    //{


                    //}
                }

            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042)
                {
                    MessageBox.Show($"Не удалось найти сервер по адресу: {AdressMysql.Text}");
                    return;
                }
                else if (ex.Message.Contains("Access denied for user"))
                {
                    MessageBox.Show("не правильный логин/пароль");
                    return;
                }
                return;
            }

            Conf conf = new Conf(AdressMysql.Text, DBName.Text, Login.Text, PasswordBox.Password, Convert.ToInt32(port.Text), pathCSP.Text); //создается объект для сериализации
            Check.connectionString = "server=" + AdressMysql.Text + ";user id=" + Login.Text + ";persistsecurityinfo=True;database=" + DBName.Text + ";allowuservariables=True;Password=" + PasswordBox.Password;

            using (FileStream fs = new FileStream("conf.xml", FileMode.Create))
            {

                //SymmetricAlgorithm key = SymmetricAlgorithm.Create();
                //using (CryptoStream cs = new CryptoStream(fs, key.CreateEncryptor(), CryptoStreamMode.Write))
                //{
                //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(PledgeNotificationToNotary));
                //    xmlSerializer.Serialize(cs, UZ1);
                //}




                XmlSerializer formatter = new XmlSerializer(typeof(Conf));
                formatter.Serialize(fs, conf);

                MessageBox.Show("Объект сериализован");
            }


            Close();
        }

        private void port_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            e.Handled = !Char.IsDigit(e.Text, 0);
        }
    }       
}
