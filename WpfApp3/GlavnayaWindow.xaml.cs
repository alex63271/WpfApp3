using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
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
using WpfApp3.GetNotification;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для GlavnayaWindow.xaml
    /// </summary>
    /// 

    public class WindowCommands
    {
        static WindowCommands()
        {
            CopyPackageid = new RoutedCommand("CopyPackageid", typeof(GlavnayaWindow));
        }
        public static RoutedCommand CopyPackageid { get; set; }
    }

   


    public partial class GlavnayaWindow : Window
    {
        public GlavnayaWindow()
        {
            InitializeComponent();
            UpdateDataGrid();
        }

        private async void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {

                using (connection)
                {                  

                    connection.Open();
                    MySqlCommand SELECT = new MySqlCommand(Check.sql, connection);

                    using (DbDataReader reader = await SELECT.ExecuteReaderAsync())
                    {
                        
                        for (int i=0; i<h1.Items.IndexOf(h1.SelectedItem); i++)
                        {
                            reader.Read();
                        }
                        reader.Read();//если цикл не выполнялся, значит индекс 0
                        MessageBox.Show(reader["Packageid"].ToString());

                        Clipboard.SetText(reader["Packageid"].ToString());     // записываем Packageid в буфер обмена


                    }


                }
            }

        }

        public void UpdateDataGrid()
        {
            
                using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(Check.sql, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                    {
                        using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                        {
                            adapter.Fill(ds);
                            h1.ItemsSource = ds.Tables[0].DefaultView;
                        }

                    }

                }
            
           
        }
        void Create_UZ1(object sender, RoutedEventArgs e)
        {

            CreateUZ1Window CreateUZ1Win = new CreateUZ1Window();
            CreateUZ1Win.ShowDialog();
        }

        private void Create_UP1_Click(object sender, RoutedEventArgs e)
        {
            CreateUP1Window CreateUP1Win = new CreateUP1Window();
            CreateUP1Win.ShowDialog();
        }


        private async void RenewState_Click(object sender, RoutedEventArgs e)
        {

            // создаем объект-запрос к серверу
            ruzdiGetNotificationPackageStateService_v1_1PortTypeClient request = new ruzdiGetNotificationPackageStateService_v1_1PortTypeClient("ruzdiGetNotificationPackageStateService_v1_1HttpSoap11Endpoint");

            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {

                using (connection)
                {
                    List<string> packageid_list = new List<string>();  //список пакетов по которым надо сделать запросы

                    connection.Open();
                    MySqlCommand SELECT = new MySqlCommand("SELECT Packageid FROM notification WHERE STATUS NOT IN ('FAULT', 'RESULT (зарегистрировано)', 'RESULT (отказ в регистрации)', 'Черновик')", connection);

                    using (DbDataReader reader = await SELECT.ExecuteReaderAsync())
                    { // получаем из БД список не обработанных пакетов

                        while (reader.Read())
                        {
                            packageid_list.Add(reader["Packageid"].ToString());    // записываем пакеты в коллекцию

                        }
                    }


                    foreach (string packageid in packageid_list)    //перебираем коллецию
                    {
                        getNotificationPackageStateRequest Package = new getNotificationPackageStateRequest(packageid); // объект - тело запроса к серверу, в конструктор передаём  packageid

                        getNotificationPackageStateResponse response = await request.getNotificationPackageStateAsync(Package); // делаем запрос и записываем ответ в response



                        if (response.packageState.ToString() == "RESULT")
                        {
                            //достаём из ответа св-во о регистрации в виде массива байтов
                            documentAndSignatureType Doc = (documentAndSignatureType)response.pledgeNotificationStatePackage.pledgeNotificationStateList[0].Item;
                            //достаём из ответа notificationId, он нужен для формирования имени файла
                            string pathgzip = response.pledgeNotificationStatePackage.pledgeNotificationStateList[0].notificationId;


                            //сохраняем массив байтов как zip архив
                            using (FileStream fstream = new FileStream("Temp/" + pathgzip + ".zip", FileMode.OpenOrCreate))
                            {

                                // запись массива байтов в поток файла
                                fstream.Write(Doc.Value, 0, Doc.Value.Length);
                                string mystr = Convert.ToBase64String(Doc.Value);
                                MySqlCommand INSERT = new MySqlCommand("INSERT INTO `TestRUZDI`.`RegistrationCertificate` (`documentAndSignature`, `Notificationid`) VALUES ( '" + mystr + "' , '" + pathgzip + "')", connection);
                                await INSERT.ExecuteNonQueryAsync(); //заполняем таблицу результатом запроса
                                MessageBox.Show("Файл сохранен в БД");
                            }




                            //распаковка zip-архива
                            ZipFile.ExtractToDirectory("Temp/" + pathgzip + ".zip", "Temp/");
                            //удаление zip-архива
                            File.Delete("Temp/" + pathgzip + ".zip");

                            //десериализовать xml, затем ее удалить


                            using (FileStream fs = new FileStream("Temp/" + pathgzip + ".xml", FileMode.OpenOrCreate))
                            {
                                try     //выполняем десериализацию св-ва о регистрации
                                {
                                    XmlSerializer formatter = new XmlSerializer(typeof(RegistrationCertificate));
                                    RegistrationCertificate xml = (RegistrationCertificate)formatter.Deserialize(fs);
                                    MySqlCommand UPDATE = new MySqlCommand("UPDATE notification SET Status= \"" + response.packageState + " (зарегистрировано)" + "\"" + ", NumberNotification = \"" + xml.RegistrationCertificateData.NotificationReferenceNumber.ToString() + "\"" + ", DataTime = \"" + xml.RegistrationCertificateData.RegistrationTime.ToString("s") + "\" WHERE  Packageid = \"" + packageid + "\"", connection);
                                    await UPDATE.ExecuteNonQueryAsync(); //заполняем таблицу результатом 

                                }

                                catch (InvalidOperationException)   //если выпало исключение - значит это св-во об отказе, десериализируем его
                                {
                                    //ниже закоментирован код десериализации св-ва об отказе, т.к. созданный объект(после десериализации) не используется
                                    //fs.Position = 0;
                                    //XmlSerializer formatter = new XmlSerializer(typeof(RegistrationRejectMessage));
                                    //RegistrationRejectMessage xml = (RegistrationRejectMessage)formatter.Deserialize(fs);
                                    MySqlCommand UPDATE = new MySqlCommand("UPDATE notification SET Status= \"" + response.packageState + " (отказ в регистрации)" + "\" WHERE  Packageid = \"" + packageid + "\"", connection);
                                    await UPDATE.ExecuteNonQueryAsync(); //заполняем таблицу результатом 

                                }
                            }
                            File.Delete("Temp/" + pathgzip + ".xml");
                            File.Delete("Temp/" + pathgzip + ".xml.sig");


                        }
                        else    //если статус != "RESULT" то устанавлиаем в БД его значение
                        {
                            MySqlCommand UPDATE = new MySqlCommand("UPDATE notification SET Status= \"" + response.packageState + "\" WHERE  Packageid = \"" + packageid + "\"", connection);
                            await UPDATE.ExecuteNonQueryAsync(); //заполняем таблицу результатом 
                        }

                    }


                    connection.Close();

                }

                //далее автоматом обновляем datagrid
                //using (MySqlDataAdapter adapter = new MySqlDataAdapter(Check.sql, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                //{
                //    using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                //    {
                //        await adapter.FillAsync(ds);
                //        h1.ItemsSource = ds.Tables[0].DefaultView;
                //    }

                //}
                UpdateDataGrid();
            }

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow2 Settings = new SettingsWindow2();
            Settings.Show();
        }

        
    }
}
