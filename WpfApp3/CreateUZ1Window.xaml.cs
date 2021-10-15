using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для CreateUZ1Window.xaml
    /// </summary>
    public partial class CreateUZ1Window : Window
    {
        public CreateUZ1Window()
        {
            InitializeComponent();
            FrameUZ1.Content = new PledgorPage();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
            
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).UpdateDataGrid();
                }
            }
           


        }
    }
}
