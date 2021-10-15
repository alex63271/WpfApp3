using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
namespace WpfApp3
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class RussianOrganization
    {
        public RussianOrganization()
        {
        }

        public RussianOrganization(string NameFull, ulong OGRN, ulong INN, string Email, RussianOrganizationAddress Address)
        {
            this.NameFull = NameFull;
            this.INN = INN;
            this.OGRN = OGRN;
            this.Email = Email;
            this.Address = Address;

        }



        public string NameFull { get; set; }


        public ulong OGRN { get; set; }

        public ulong INN { get; set; }

        public RussianOrganizationAddress Address { get; set; }

 
        public string Email { get; set; }


    }
}
