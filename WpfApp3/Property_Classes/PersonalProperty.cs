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
    public class PersonalProperty
    {
        public PersonalProperty()
        {
        }

        public PersonalProperty(VehicleProperty VehicleProperty)
        {
            this.VehicleProperty = VehicleProperty;
        }
        public PersonalProperty(OtherProperty OtherProperty)
        {
            this.OtherProperty = OtherProperty;
        }
        public VehicleProperty VehicleProperty { get; set; }

        public OtherProperty OtherProperty { get; set; }
    }
}
