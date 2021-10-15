using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WpfApp3.Pledgee_Class;


namespace WpfApp3
{


    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class PledgeePrivatePerson
    {
        public PledgeePrivatePerson()
        {
        }

        public PledgeePrivatePerson(PrivatePersonName Name, System.DateTime BirthDate, PledgeePrivatePersonPersonDocument PersonDocument, PrivatePersonPersonAddress PersonAddress, string Email)
        {
            this.Name = Name;
            this.BirthDate = BirthDate;
            this.PersonDocument = PersonDocument;
            this.PersonAddress = PersonAddress;
            this.Email = Email;
        }

        public PrivatePersonName Name { get; set; }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime BirthDate { get; set; }

        public PledgeePrivatePersonPersonDocument PersonDocument { get; set; }


        public PrivatePersonPersonAddress PersonAddress { get; set; }


        public string Email { get; set; }
    }
}
