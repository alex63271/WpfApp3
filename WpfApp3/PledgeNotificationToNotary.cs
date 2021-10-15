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
    public class PledgeNotificationToNotary
    {
        public PledgeNotificationToNotary()
        {

        }
        public PledgeNotificationToNotary(string NotificationId, NotificationData NotificationData)
        {
            this.NotificationId = NotificationId;
            this.NotificationData = NotificationData;
            this.version = 2.3M;
        }
        public string NotificationId { get; set; }

        public NotificationData NotificationData { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal version { get; set; }
    }
}
