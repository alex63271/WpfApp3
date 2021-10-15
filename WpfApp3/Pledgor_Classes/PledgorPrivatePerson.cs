using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Pledgor_Class
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class PledgorPrivatePerson
    {
        public PledgorPrivatePerson()
        {
        }

        public PledgorPrivatePerson(PrivatePersonName Name, DateTime BirthDate, PledgorPrivatePersonPersonDocument PersonDocument, PrivatePersonPersonAddress PersonAddress)
        {
            this.Name = Name;
            this.BirthDate = BirthDate;
            this.PersonDocument = PersonDocument;
            this.PersonAddress = PersonAddress;



        }

        public PrivatePersonName Name { get; set; }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime BirthDate { get; set; }


        public PledgorPrivatePersonPersonDocument PersonDocument { get; set; }


        public PrivatePersonPersonAddress PersonAddress { get; set; }
    }
}
