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
    public class VehicleProperty
    {
        public VehicleProperty()
        {
        }

        public VehicleProperty(string VIN, string Description)
        {
            this.VIN = VIN;
            this.Description = Description;
            //BodyNumber = "123";

        }
        public string VIN { get; set; }


        public string ChassisNumber { get; set; }


        public string BodyNumber { get; set; }
        public string Description { get; set; }

        public bool ShouldSerializeChassisNumber() //метод, сообщающий сериализатору нужно ли сериализовать это поле
        {
            return ChassisNumber != "";
        }

        public bool ShouldSerializeBodyNumber() //метод, сообщающий сериализатору нужно ли сериализовать это поле
        {
            return BodyNumber != "";
        }
    }
}
