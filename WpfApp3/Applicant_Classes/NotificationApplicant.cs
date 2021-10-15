using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Applicant_Class
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class NotificationApplicant
    {
        public NotificationApplicant()
        {
        }

        public NotificationApplicant(byte Role, ApplicantOrganization Organization, ApplicantAttorney Attorney)
        {
            this.Role = Role;
            this.Organization = Organization;
            this.Attorney = Attorney;
        }
        public byte Role { get; set; }


        public ApplicantOrganization Organization { get; set; }


        public ApplicantAttorney Attorney { get; set; }
    }
}
