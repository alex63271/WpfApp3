using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    [Serializable]
    public class Conf
    {
        public Conf()
        { 
        }

        public Conf(string adress, string DBName, string login, string password, int port, string pathCSP)
        {
            this.adress = adress;
            this.DBName = DBName;
            this.login = login;
            this.password = password;
            this.port = port;
            this.pathCSP = pathCSP;
        }

        public string adress { get; set; }
        public string DBName { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int port { get; set; }
        public string pathCSP { get; set; }

    }
}
