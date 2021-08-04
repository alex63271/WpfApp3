using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Логика взаимодействия для aplicant.xaml
    /// </summary>
    public partial class aplicant : Page
    {
        public aplicant()
        {
            InitializeComponent();

            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            { 

                                store.Open(OpenFlags.ReadOnly);
                           // ArrayList list = new ArrayList();
                //Проходим по всем сертификатам и что то с ними делаем
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.NotAfter > DateTime.Now)
                    {


                        string otvet = cert.SubjectName.Name + ","; //добавляем запятую в конец чтобы искался последний элемент в строке
                        string zap = ",";

                        //MessageBox.Show(otvet);
                        if (otvet.Contains("CN=") && otvet.Contains("SN=") && otvet.Contains("G=") && otvet.Contains("ИНН="))
                        {
                            string s = "O=";
                            string O = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));

                            s = "SN=";
                            string SN = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));                         

                            s = "E=";
                            string mail = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));

                            s = "ОГРН=";
                            string ogrn = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));

                            s = "G=";
                            string G = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));

                            string[] words = G.Split(new char[] { ' ' });


                            s = "ИНН=";
                            string INN = otvet.Substring(otvet.IndexOf(s) + s.Length, otvet.IndexOf(zap, otvet.IndexOf(s)) - (otvet.IndexOf(s) + s.Length));

                            long q = Convert.ToInt64(INN); //преобразование в int чтобы убрать нули в начале инн
                            INN = Convert.ToString(q);

                            // string stroka = SN + " " + G + ", Действителен до " + cert.NotAfter;    //создаем строку для записи её в лист

                            if ("7708237747" == INN)
                            {
                                // list.Add(stroka);   //записываем строку в лист
                                Nameorg.Text = O;
                                inn.Text = INN;
                                ogrnxaml.Text = ogrn;
                                email.Text = mail;
                                Familiya.Text = SN;
                                imya.Text = words[0];
                                ot4estvo.Text = words[1];

                            }
                        }
                    }
                }
                //В данном участке кода будут все сертификаты из указанног охранилища

            }
            //comboBox1.DataSource = list;






        }

        private  void but2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new glavnaya());
        }
    }
}
