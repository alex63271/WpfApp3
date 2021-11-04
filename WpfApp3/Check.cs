using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using WpfApp3.Pledgor_Class;
using WpfApp3.Property_Class;
using WpfApp3.Applicant_Class;

namespace WpfApp3
{
    public static class Check
    {
        // Если методы возвращают True - значит создаем запись в таблице. Если False - значит в таблице уже есть такая запись.

        public static string Temp = "Temp/"; // папка для временных файлов
        public static int HashPledgor;
        public static int HashPledgee;
        public static int HashContract;
        public static string NotificationId;
        public static string guidp;
        public static string AdressMysql;
        public static string login;
        public static string password;
        public static int port;
        public static string DBName;
        public static string connectionString;// = "server=" + AdressMysql + ";user id=" + login + ";persistsecurityinfo=True;database=" + DBName + ";allowuservariables=True;Password="+ password+ "port="+ port;
        public static string sql = "SELECT Packageid, CONCAT(Person.Last , ' ', Person.First, ' ',Person.Middle) AS ФИО, DATE_FORMAT(notification.DataTime, '%d.%m.%Y  %H.%i') as DataTime, notification.NumberNotification, Contracts.Number, notification.Status, notification.TypeNotification  FROM notification LEFT OUTER  join Person ON Person.Hash = notification.Pledgor LEFT OUTER join Contracts ON Contracts.Hash = notification.PledgeContract;";  // sql-запрос для создания кэша таблицы notification


        public static byte[] array;




        public static bool CheckPerson(string hash)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))  //создаем объект подключения к mysql
            {

                string sql = "SELECT * FROM Person WHERE HASH=" + hash;
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                {


                    using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                    {
                        adapter.FillAsync(ds);  // метод адаптера заполняет кэш


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Check.HashPledgor = Convert.ToInt32(hash);
                            return false;
                        }

                        else
                        {
                            return true;
                        }


                    }
                }
            }

        }

        public static bool CheckOrganization(string hash)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))  //создаем объект подключения к mysql
            {

                string sql = "SELECT * FROM Organization WHERE HASH=" + hash;
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                {


                    using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                    {
                        adapter.FillAsync(ds);  // метод адаптера заполняет кэш


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Check.HashPledgee = Convert.ToInt32(hash);
                            return false;
                        }

                        else
                        {
                            return true;
                        }


                    }
                }
            }

        }




        public static bool CheckContracts(string hash)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))  //создаем объект подключения к mysql
            {

                string sql = "SELECT * FROM Contracts WHERE HASH=" + hash;
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection))    //создаем адаптер из подключения mysql для заполнения кэша dataset
                {


                    using (DataSet ds = new DataSet())  //создаем объект - кэш для хранения данных из БД
                    {
                        adapter.FillAsync(ds);  // метод адаптера заполняет кэш


                        if (ds.Tables[0].Rows.Count > 0) //если в БД найдена запись, то записываем в переменную ее хэш
                        {
                            Check.HashContract = Convert.ToInt32(hash);
                            return false;
                        }

                        else
                        {
                            return true;
                        }


                    }
                }
            }

        }




        //формирование уведомления


        //создаем объект договора залога
                public static PledgeContract CreateContract()
                {
                    using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
                    {
                        
                            connection.Open();
                            MySqlCommand SELECTContr = new MySqlCommand("SELECT * FROM Contracts WHERE Hash = " + Check.HashContract, connection);
                            DbDataReader read = SELECTContr.ExecuteReader(); // получаем из БД список 
                            read.Read();

                            PledgeContract PledgeContract = new PledgeContract(read["Name"].ToString(), Convert.ToDateTime(read["Data"]), read["Number"].ToString(), Convert.ToDateTime(read["TermOfContract"]));    //СОЗДАЕМ ОБЪЕКТ договора залога
                            read.Close();
                            connection.Close();
                            return PledgeContract;
                       
                    }
                }


        // создаем объект залогодержатедя
        public static List<Pledgee> CreatePledgee()
        {
            List<Pledgee> Pledgee = new List<Pledgee>(); //создаем список залогодержателей

            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT * FROM Organization JOIN Regions ON Organization.Region = Regions.Region WHERE Organization.Hash =" + Check.HashPledgee.ToString(), connection);
                DbDataReader reader = SELECT.ExecuteReader(); 
                reader.Read();

                RussianOrganizationAddress Address = new RussianOrganizationAddress(reader["Code"].ToString(), reader["Region"].ToString()); //объект-адрес
                RussianOrganization RussianOrganization = new RussianOrganization(reader["NameFull"].ToString(), Convert.ToUInt64(reader["OGRN"]), Convert.ToUInt64(reader["INN"]), reader["E-mail"].ToString(), Address); //объект российская организация
                PledgeeOrganization Organization = new PledgeeOrganization(RussianOrganization); // объект организация

                reader.Close();
                Pledgee.Add(new Pledgee(Organization)); // добавлдяем организацию в список залогодержателей
                return Pledgee;
            }
        }



        // создаем объект залогодатедя
        public static List<Pledgor> CreatePledgor()
        {
            List<Pledgor> Pledgors = new List<Pledgor>();    //создаем список залогодаелей


            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {


                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT * FROM Person JOIN Regions ON Person.Region = Regions.Region WHERE Person.Hash = " + Check.HashPledgor.ToString(), connection);

                DbDataReader reader = SELECT.ExecuteReader(); // получаем из БД залогодателя по его хэшу


                reader.Read();
                PrivatePersonName Name = new PrivatePersonName(reader["Last"].ToString(), reader["First"].ToString(), reader["Middle"].ToString()); // объект ФИО
                PledgorPrivatePersonPersonDocument PersonDocument = new PledgorPrivatePersonPersonDocument(21, "паспорт", reader["PersonDocument"].ToString()); //объект паспорт
                PrivatePersonPersonAddressAddressRF AddressRF = new PrivatePersonPersonAddressAddressRF(reader["Code"].ToString(), reader["Region"].ToString(), true); // объект- адрес
                PrivatePersonPersonAddress PersonAddress = new PrivatePersonPersonAddress(AddressRF); // объект адрес физ. лица
                PledgorPrivatePerson PrivatePerson = new PledgorPrivatePerson(Name, Convert.ToDateTime(reader["BirthDate"]), PersonDocument, PersonAddress); // объект залогодатель

                reader.Close();

                Pledgors.Add(new Pledgor(PrivatePerson));         // добавляем залогодателя в список



                return Pledgors;
            }
        }


        // создаем имущество
        public static List<PersonalProperty> CreatePersonalProperty()
        {
            List<PersonalProperty> PersonalProperties = new List<PersonalProperty>();   //создаем список имущества
            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT VIN, Description FROM VehicleProperty WHERE Notificationid = '" + Check.NotificationId.ToString() + "'", connection);
                DbDataReader reader = SELECT.ExecuteReader(); // получаем из БД имущество

                reader.Read();
                VehicleProperty VehicleProperty = new VehicleProperty(reader["VIN"].ToString(), reader["Description"].ToString());
                PersonalProperties.Add(new PersonalProperty(VehicleProperty));
                reader.Close();

                return PersonalProperties;
            }

        }
        //создаем заявителя
        public static NotificationApplicant CreateApplicant(string Familiya, string Imya, string Ot4estvo, ulong NumberPassport, DateTime BirthDate, string Authority, string mail)
        {
            ApplicantAttorneyName AttorneyName = new ApplicantAttorneyName(Familiya, Imya, Ot4estvo);


            ApplicantAttorneyPersonDocument ApplicantPersonDocument = new ApplicantAttorneyPersonDocument(21, "паспорт", NumberPassport);

            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT * FROM Organization JOIN Regions ON Organization.Region = Regions.Region WHERE Organization.Hash = '" + Check.HashPledgee.ToString() + "'", connection);
                DbDataReader reader = SELECT.ExecuteReader(); // получаем из БД
                reader.Read();
                ApplicantAttorneyPersonAddressAddressRF ApplicantAddressRF = new ApplicantAttorneyPersonAddressAddressRF(reader["Code"].ToString(), reader["Region"].ToString(), true);
                ApplicantAttorneyPersonAddress ApplicantPersonAddress = new ApplicantAttorneyPersonAddress(ApplicantAddressRF);
                ApplicantAttorney Attorney = new ApplicantAttorney(AttorneyName, BirthDate, Authority, ApplicantPersonDocument, ApplicantPersonAddress);
               
             

                ApplicantOrganization ApplicantOrganization = new ApplicantOrganization(reader["NameFull"].ToString(), Convert.ToUInt64(reader["OGRN"]), Convert.ToUInt64(reader["INN"]), mail);
                NotificationApplicant NotificationApplicant = new NotificationApplicant(2, ApplicantOrganization, Attorney);
                reader.Close();
                return NotificationApplicant;
            }
        }



        //ниже перегруженный метод создания заявителя для UP1
        public static NotificationApplicant CreateApplicant(string NameORG, string OGRN, string INN, string Region, string Familiya, string Imya, string Ot4estvo, ulong NumberPassport, DateTime BirthDate, string Authority, string mail)
        {
            ApplicantAttorneyName AttorneyName = new ApplicantAttorneyName(Familiya, Imya, Ot4estvo);


            ApplicantAttorneyPersonDocument ApplicantPersonDocument = new ApplicantAttorneyPersonDocument(21, "паспорт", NumberPassport);
            using (MySqlConnection connection = new MySqlConnection(Check.connectionString))  //создаем объект подключения к mysql
            {
                connection.Open();
                MySqlCommand SELECT = new MySqlCommand("SELECT Code FROM Regions WHERE Region = '" + Region + "'", connection);
                DbDataReader reader = SELECT.ExecuteReader(); // получаем из БД
                reader.Read();

                ApplicantAttorneyPersonAddressAddressRF ApplicantAddressRF = new ApplicantAttorneyPersonAddressAddressRF(reader["Code"].ToString(), Region, true);
                ApplicantAttorneyPersonAddress ApplicantPersonAddress = new ApplicantAttorneyPersonAddress(ApplicantAddressRF);
                ApplicantAttorney Attorney = new ApplicantAttorney(AttorneyName, BirthDate, Authority, ApplicantPersonDocument, ApplicantPersonAddress);



                ApplicantOrganization ApplicantOrganization = new ApplicantOrganization(NameORG, Convert.ToUInt64(OGRN), Convert.ToUInt64(INN), mail);
                NotificationApplicant NotificationApplicant = new NotificationApplicant(2, ApplicantOrganization, Attorney);

                return NotificationApplicant;
            }
        }



    }

}







