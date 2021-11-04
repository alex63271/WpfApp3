using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
using WpfApp3.UploadNotification;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для CreateUP1Window.xaml
    /// </summary>
    public partial class CreateUP1Window : Window
    {
        public CreateUP1Window()
        {
            InitializeComponent();
            LoadComboBoxRegionAsync();
            Check.NotificationId = Guid.NewGuid().ToString(); // запуск окна создания уведомления - генеируем NotificationId





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

                    using (DbDataReader reader = await SELECT.ExecuteReaderAsync())
                    { // получаем из БД регионы

                        while (reader.Read())
                        {
                            Region_list.Add(reader["Region"].ToString());    // записываем регионы в список

                        }
                        Region_list.Sort();
                    }


                    AplicantRegion.ItemsSource = Region_list;
              
                }
            }
        }

        //ниже метод обработки события закрытия окна, он обновляет datagrid основоного окна

        private void Window_Closed_1(object sender, EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(GlavnayaWindow))
                {
                    (window as GlavnayaWindow).UpdateDataGrid();
                }
            }
        }
       

        private async void SendNotificationUP1_Click(object sender, RoutedEventArgs e)
        {

            string Temp = "Temp/"; // папка для временных файлов

            string TempNotificationId = Temp + Check.NotificationId;    //папка для хранения уведомления и подписи
            string TempNotificationIdXml = TempNotificationId + "/" + Check.NotificationId + ".xml";
            Directory.CreateDirectory(Temp);
            Directory.CreateDirectory(TempNotificationId);  // создаем папку куда потом сохраним уведомление



            //создаем уведомление о возникновении, в конструктор передаем методы, возвращающие нужные значения
            FormUP1 FormUP1 = new FormUP1(NumberNotification.Text,  Check.CreateApplicant(NameORG.Text, AplicantOGRN.Text, AplicantINN.Text, AplicantRegion.Text, FamiliyaTexbox.Text, NameTexbox.Text, Ot4estTexbox.Text, Convert.ToUInt64(NumberPasspTexbox.Text), Convert.ToDateTime(BirthDatePicker.SelectedDate), Polnomo4Texbox.Text, mailTextbox.Text));

            //создание объекта для тега NotificationData
            NotificationData NotificationData = new NotificationData(FormUP1, 2.3M);

            //объект корневого тега xml
            PledgeNotificationToNotary UP1 = new PledgeNotificationToNotary(Check.NotificationId, NotificationData);

            //сериализация xml
            using (FileStream fs = new FileStream(TempNotificationIdXml, FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(PledgeNotificationToNotary));
                formatter.Serialize(fs, UP1);

                MessageBox.Show("Объект сериализован");
            }




            //подписание уведомления
            string csp = "\"C:/Program Files/Crypto Pro/CSP/csptest.exe\"";
            string putxml = Temp + Check.NotificationId + "/" + Check.NotificationId;


            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C " + csp + " -sfsign -sign -detached -add -in " + putxml + ".xml -out " + putxml + ".xml.sig -my eb3aad1807409334b41fa435241f1f824cc6ffc6",
                WindowStyle = ProcessWindowStyle.Hidden
            });

            MessageBox.Show("уведомление подписано");




            // архивирование уведомления и подписи

            string sourceFile = TempNotificationId + "/"; // исходная папка(архивируем все, что в ней есть)
            string compressedFile = "Temp/" + Check.NotificationId + ".zip"; // сжатый файл       
            ZipFile.CreateFromDirectory(sourceFile, compressedFile, CompressionLevel.Optimal, false);   // создание архива
            MessageBox.Show("архивировние завершено");



            //
            Check.guidp = Convert.ToString(Guid.NewGuid()); //генерируем гуид пкета

            using (FileStream fstr = File.OpenRead(compressedFile))
            {//создаем поток из архива

                // преобразуем строку в байты
                byte[] array = new byte[fstr.Length]; //создаем массив байтов длинною в поток архива
                                                      //читаем поток файла
                fstr.Read(array, 0, array.Length); //записываем в массив байты из архива

                Check.array = array;          //использую переменную статического класса чтобы вывести значение за using
            }



            pledgeNotificationListElementType pledgeNotificationListElement = new pledgeNotificationListElementType  //использую инициализатор, т.к. нет перегруженного конструктора
            {
                notificationId = Check.NotificationId, // прописываем гуид уведомления
                documentAndSignature = Check.array      //передаем массив байтов в тег documentAndSignature
            };




            pledgeNotificationListElementType[] NotificationList = new pledgeNotificationListElementType[1]; //массив уведомлений 1 - 20
            NotificationList[0] = pledgeNotificationListElement;    //в первую ячейку массива записываю уведомление



            pledgeNotificationPackageType pledgeNotificationPackage = new pledgeNotificationPackageType     //использую инициализатор, т.к. нет перегруженного конструктора
            {

                packageId = Check.guidp, //присваиваем пакету гуид
                senderType = (senderTypeType)0, // присваиваем занчение senderType(0)
                uip = "000000000000000000000TEST", //прописываем УИП
                pledgeNotificationList = NotificationList   //записываю в пакет массив уведомления(ний)

            };

            uploadNotificationPackageRequest package = new uploadNotificationPackageRequest(pledgeNotificationPackage);    //формирование объекта-пакет

            MessageBox.Show("Пакет сформирован");

            //далее записываем в БД информацию о сформированном пакете
            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                string sqlnotification = "SELECT * FROM notification;";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(sqlnotification, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                {
                    using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                    {

                        adapter.Fill(ds);

                        DataTable dt = ds.Tables[0]; // создаем объект таблицы на основе кэша

                        DataRow newRow = dt.NewRow();   //создаем объект новой записи в таблице dt
                        newRow["Packageguid"] = Check.guidp;
                        newRow["NotificationId"] = Check.NotificationId;
                        newRow["TypeNotification"] = "Исключение";
                        newRow["Pledgor"] = Check.HashPledgor.ToString();
                        newRow["PledgeContract"] = Check.HashContract.ToString();
                        newRow["NumberNotification"] = NumberNotification.Text;
                        newRow["Status"] = "Первичная отправка";
                        dt.Rows.Add(newRow);
                        MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                        adapter.Update(ds);


                    }
                }
            }

            //Запись сфорированного архива в БД
            using (FileStream fstream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку

                string mystr = Convert.ToBase64String(array);

                using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
                {
                    connection.Open();
                    MySqlCommand UPDATE = new MySqlCommand("UPDATE `TestRUZDI`.`notification` SET `ZipArchive`='" + mystr + "' WHERE  `NotificationId`='" + Check.NotificationId + "';", connection);
                    await UPDATE.ExecuteNonQueryAsync(); //заполняем таблицу результатом запроса
                    MessageBox.Show("Архив сохранен в БД");
                }
            }

            // удаляем созданные файлы, т.к. более не нужны
            DirectoryInfo dirInfo = new DirectoryInfo(TempNotificationId);
            dirInfo.Delete(true);
            File.Delete(compressedFile);


            ruzdiUploadNotificationPackageService_v1_1PortTypeClient request = new ruzdiUploadNotificationPackageService_v1_1PortTypeClient("ruzdiUploadNotificationPackageService_v1_1HttpSoap11Endpoint");



            uploadNotificationPackageResponse response = await request.uploadNotificationPackageAsync(package); // отправляем пакет 

            if (response.packageStateCode.code != "0")
            {
                MessageBox.Show($"код ошибки - {response.packageStateCode.code}");
                MessageBox.Show($"код ошибки - {response.packageStateCode.message}");
            }

            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                using (connection)
                {

                    connection.Open();
                    //записываю в БД ответный guid
                    MySqlCommand command = new MySqlCommand("UPDATE notification SET Packageid= \"" + response.registrationId + "\" WHERE  Packageguid = \"" + Check.guidp + "\"", connection);

                    command.ExecuteNonQuery();
                    connection.Close();

                }
            }


            MessageBox.Show($"Пакет отправлен успешно, рег № пакета -  {response.registrationId}");

        }

        private void NumberPasspTexbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }

        private void AplicantINN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }

        private void AplicantOGRN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }
    }
}
