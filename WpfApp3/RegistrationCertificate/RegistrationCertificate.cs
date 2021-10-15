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
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3", IsNullable = false)]
    public class RegistrationCertificate
    {
        public string RegistrationCertificateId { get; set; }


        /// <remarks/>
        public string NotificationId { get; set; }

        /// <remarks/>
        public RegistrationCertificateData RegistrationCertificateData { get; set; }

        /// <remarks/>
      

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal version { get; set; }

    }
}
