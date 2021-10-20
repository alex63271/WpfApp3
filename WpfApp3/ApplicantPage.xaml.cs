using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Serialization;
using WpfApp3.UploadNotification;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для ApplicantPage.xaml
    /// </summary>
    public partial class ApplicantPage : Page
    {


        ArrayList list2 = new ArrayList();
        ArrayList list = new ArrayList();
        public ApplicantPage()
        {
            InitializeComponent();
            
            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT * FROM Organization JOIN Regions ON Organization.Region = Regions.Region WHERE Organization.Hash =" + Check.HashPledgee.ToString(), connection);
                DbDataReader reader = SELECT.ExecuteReader();
                reader.Read();


                //Заполнение лэйблов заявителя-залогодержателя

                NameOrg.Content = reader["NameFull"].ToString();
                AplicantINN.Content = Convert.ToUInt64(reader["INN"]);
                AplicantOGRN.Content = Convert.ToUInt64(reader["OGRN"]);
                AplicantRegion.Content = reader["Region"].ToString();
                reader.Close();

            }


                                using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                                {

                                    store.Open(OpenFlags.ReadOnly);
                                    
                
                                    // Проходим по всем сертификатам 
                                    foreach (X509Certificate2 cert in store.Certificates)
                                    {
                                        if (cert.NotAfter > DateTime.Now) // выбираем сертификаты с действующим сроком
                                        {
                                            string zap = ",";
                                            string otvet = cert.SubjectName.Name + zap; //добавляем запятую в конец чтобы искался последний элемент в строке


                                            if (otvet.Contains("CN=") && otvet.Contains("SN=") && otvet.Contains("G=") && otvet.Contains("ОГРН="))  //отсеиваем сертификаты без нужных атрибутов
                                            {

                                                string s = "CN=";
                                                string CN = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));


                                                s = "SN=";
                                                string SN = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));

                                                s = "G=";
                                                string G = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));



                                                string stroka = CN + ", " + SN + " " + G;    //создаем строку для записи её в лист
                            



                                                list.Add(stroka);   //записываем строку в лист для отображения в интерфейсе
                                                list2.Add(cert.Thumbprint);  // лист2 для программного выбора сертификата(содержит отпечатки сертификатов)


                                            }
                                        }
                                    }

                ComboCert.ItemsSource = list;
            }






        }

        private async void SendNotification_Click(object sender, RoutedEventArgs e)
        {
            // проверяем поля на пустоту
            if (string.IsNullOrEmpty(FamiliyaTexbox.Text) || string.IsNullOrEmpty(NameTexbox.Text) || string.IsNullOrEmpty(Ot4estTexbox.Text) || string.IsNullOrEmpty(BirthDatePicker.Text) || string.IsNullOrEmpty(Polnomo4Texbox.Text) || string.IsNullOrEmpty(NumberPasspTexbox.Text) || string.IsNullOrEmpty(mailTextbox.Text) || string.IsNullOrEmpty(ComboCert.Text))
            {
                MessageBox.Show("нужно заполнить все поля");
                return;
            }

            





            string Temp = "Temp/"; // папка для временных файлов

            string TempNotificationId = Temp + Check.NotificationId;    //папка для хранения уведомления и подписи
            string TempNotificationIdXml = TempNotificationId + "/" + Check.NotificationId + ".xml";
            Directory.CreateDirectory(Temp);
            Directory.CreateDirectory(TempNotificationId);  // создаем папку куда потом сохраним уведомление



            //создаем уведомление о возникновении, в конструктор передаем методы, возвращающие нужные значения
            FormUZ1 FormUZ1 = new FormUZ1(Check.CreatePersonalProperty(), Check.CreatePledgor(), Check.CreatePledgee(), Check.CreateContract(), Check.CreateApplicant(FamiliyaTexbox.Text, NameTexbox.Text, Ot4estTexbox.Text, Convert.ToUInt64(NumberPasspTexbox.Text), Convert.ToDateTime(BirthDatePicker.SelectedDate), Polnomo4Texbox.Text, mailTextbox.Text));

            //создание объекта для тега NotificationData
            NotificationData NotificationData = new NotificationData(FormUZ1, 2.3M);

            //объект корневого тега xml
            PledgeNotificationToNotary UZ1 = new PledgeNotificationToNotary(Check.NotificationId, NotificationData);

            //сериализация xml
            using (FileStream fs = new FileStream(TempNotificationIdXml, FileMode.OpenOrCreate))
            {

                //SymmetricAlgorithm key = SymmetricAlgorithm.Create();
                //using (CryptoStream cs = new CryptoStream(fs, key.CreateEncryptor(), CryptoStreamMode.Write))
                //{
                //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(PledgeNotificationToNotary));
                //    xmlSerializer.Serialize(cs, UZ1);
                //}




                XmlSerializer formatter = new XmlSerializer(typeof(PledgeNotificationToNotary));
                formatter.Serialize(fs, UZ1);

                MessageBox.Show("Объект сериализован");
            }




           
            
            




            //подписание уведомления
            string csp = "\"C:/Program Files/Crypto Pro/CSP/csptest.exe\"";
            string putxml = Temp + Check.NotificationId + "/" + Check.NotificationId;


            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C " + csp + " -sfsign -sign -detached -add -in " + putxml + ".xml -out " + putxml + ".xml.sig -my " + list2[ComboCert.SelectedIndex].ToString(), 
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
                        newRow["TypeNotification"] = "Возникновение";
                        newRow["Pledgor"] = Check.HashPledgor.ToString();
                        newRow["PledgeContract"] = Check.HashContract.ToString();
                        //XmlDocument xDoc = new XmlDocument();
                        //xDoc.Load(putxml + ".xml");                                      
                        //newRow["XML"] = xDoc.OuterXml;
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

        private void ApplicantBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PledgeContractPage());
        }

        private void NumberPasspTexbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Char.IsDigit(e.Text, 0);
        }
    }
    
}
