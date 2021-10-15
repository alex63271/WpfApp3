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
    public class ApplicantAttorney
    {
        public ApplicantAttorney()
        {
        }

        public ApplicantAttorney(ApplicantAttorneyName Name, System.DateTime BirthDate, string Authority, ApplicantAttorneyPersonDocument PersonDocument, ApplicantAttorneyPersonAddress PersonAddress)
        {
            this.Name = Name;
            this.BirthDate = BirthDate;
            this.PersonDocument = PersonDocument;
            this.PersonAddress = PersonAddress;
            this.Authority = Authority;


        }

        public ApplicantAttorneyName Name { get; set; }


        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime BirthDate { get; set; }


        public string Authority { get; set; }


        public ApplicantAttorneyPersonDocument PersonDocument { get; set; }


        public ApplicantAttorneyPersonAddress PersonAddress { get; set; }
    }
}
