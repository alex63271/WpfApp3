using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using WpfApp3.GetNotification;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            FileInfo conf = new FileInfo("conf.xml");
            if (conf.Exists)
            {
                
                using (FileStream fs = new FileStream("conf.xml", FileMode.OpenOrCreate))
                {
                    //выполняем десериализацию конфига

                    XmlSerializer formatter = new XmlSerializer(typeof(Conf));
                    Conf xmlconf = (Conf)formatter.Deserialize(fs);

                    DataContext = xmlconf; //помещаю нужный объект в DataContext перед InitializeComponent для работы привязки 
                    InitializeComponent();  //выполняем инициализацию после создания объекта и помещения его в DataContext
                    PasswordBox.Password = xmlconf.password; // заполняю порграммно т.к. привязка не работает с PasswordBox

                    //далее заполняем connectionString из конфига
                    Check.connectionString = "server=" + xmlconf.adress + ";user id=" + xmlconf.login + ";persistsecurityinfo=True;database=" + xmlconf.DBName + ";allowuservariables=True;Password=" + xmlconf.password + ";port=" + xmlconf.port; 
                   
                }
                
                GlavnayaWindow glavnaya = new GlavnayaWindow();
                glavnaya.ShowDialog();
                this.Close();    //закрываем страницу настроек и открываем главную
            }
            else 
            {
                
                InitializeComponent();
                
 
            }

        }

        private void Button_ОК_Click(object sender, RoutedEventArgs e)
        {

            //проверяем подключение к БД с введенными данными
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=" + AdressMysql.Text + ";user id=" + Login.Text + ";persistsecurityinfo=True;database=" + DBName.Text + ";allowuservariables=True;Password=" + PasswordBox.Password + ";port=" + port.Text))  //создаем объект подключения к mysql
                {
                    connection.Open();
                  
                }

            }
            catch (MySqlException ex) //если возникло исключение, то выводим окно с ошибкой
            {
                if (ex.Number == 1042)
                {
                    MessageBox.Show($"Не удалось найти Mysql сервер по адресу: {AdressMysql.Text}");
                    //return;
                }
                else if (ex.Message.Contains("Access denied for user"))
                {
                    MessageBox.Show("не правильный логин/пароль БД Mysql");
                   // return;
                }
                return;
            }
            // если исключений не возникло, то сохраняем данные в конфиг
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

                   // MessageBox.Show("Объект сериализован");
                }


                GlavnayaWindow glavnaya = new GlavnayaWindow();
                glavnaya.ShowDialog();
                Close();    //закрываем страницу настроек и открываем главную

        }

        private void port_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }
    }
}




