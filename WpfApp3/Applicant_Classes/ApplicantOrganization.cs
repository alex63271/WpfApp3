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
    public class ApplicantOrganization
    {
        public ApplicantOrganization()
        {
        }

        public ApplicantOrganization(string NameFull, ulong URN, ulong UINN, string Email)
        {
            this.NameFull = NameFull;
            this.UINN = UINN;
            this.URN = URN;
            this.Email = Email;

        }
        public string NameFull { get; set; }


        public ulong URN { get; set; }


        public ulong UINN { get; set; }


        public string Email { get; set; }


    }
}
