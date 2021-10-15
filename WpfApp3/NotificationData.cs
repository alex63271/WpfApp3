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
    public class NotificationData
    {
        public NotificationData()   //конструктор для сериализации
        {
        }

        public NotificationData(FormUZ1 FormUZ1, decimal version)   //конструктор для FormUZ1
        {
            this.FormUZ1 = FormUZ1;
            this.version = 2.3M;


        }

        public NotificationData(FormUP1 FormUP1, decimal version)//конструктор для FormUP1
        {
            this.FormUP1 = FormUP1;
            this.version = 2.3M;


        }
        public FormUP1 FormUP1 { get; set; }
        public FormUZ1 FormUZ1 { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal version { get; set; }
    }
}
