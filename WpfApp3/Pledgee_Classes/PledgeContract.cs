using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class PledgeContract
    {
        public PledgeContract()
        {
        }

        public PledgeContract(string Name, System.DateTime Date, string Number, System.DateTime TermOfContract)
        {
            this.Name = Name;
            this.Number = Number;
            this.Date = Date;
            this.TermOfContract = TermOfContract;


        }


        public string Name { get; set; }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Date { get; set; }


        public string Number { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime TermOfContract { get; set; }
    }

}
