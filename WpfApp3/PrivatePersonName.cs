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
    public class PrivatePersonName
    {
        public PrivatePersonName()
        {
        }

        public PrivatePersonName(string Last, string First, string Middle)
        {
            this.First = First;
            this.Middle = Middle;
            this.Last = Last;


        }
        public string Last { get; set; }


        public string First { get; set; }


        public string Middle { get; set; }
    }

}
