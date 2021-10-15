using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Applicant_Class;

namespace WpfApp3
{

        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
        public class FormUP1
        {

        public FormUP1()
        {
        }

        public FormUP1(string CreationReferenceNumber,  NotificationApplicant NotificationApplicant)
        {
            this.CreationReferenceNumber = CreationReferenceNumber;          
            this.NotificationApplicant = NotificationApplicant;

        }

            public string CreationReferenceNumber { get; set; }
            public NotificationApplicant NotificationApplicant { get; set; }
        }

      
    
}

