using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Property_Class
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class OtherProperty
    {
        public OtherProperty()
        {
        }

        public OtherProperty(string ID, string Description)
        {
            this.ID = ID;
            this.Description = Description;

        }
        public string ID { get; set; }

        public string Description { get; set; }
    }
}
