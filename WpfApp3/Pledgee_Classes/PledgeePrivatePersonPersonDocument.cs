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
    public class PledgeePrivatePersonPersonDocument
    {
        public PledgeePrivatePersonPersonDocument()
        {
        }

        public PledgeePrivatePersonPersonDocument(byte Code, string Name, string SeriesNumber)
        {
            this.Code = Code;
            this.Name = Name;
            this.SeriesNumber = SeriesNumber;
        }


        public byte Code { get; set; }


        public string Name { get; set; }


        public string SeriesNumber { get; set; }
    }

}
