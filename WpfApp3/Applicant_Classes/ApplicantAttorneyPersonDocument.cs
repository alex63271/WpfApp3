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
    public class ApplicantAttorneyPersonDocument
    {
        public ApplicantAttorneyPersonDocument()
        {
        }

        public ApplicantAttorneyPersonDocument(byte Code, string Name, ulong SeriesNumber)
        {
            this.Code = Code;
            this.Name = Name;
            this.SeriesNumber = SeriesNumber;

        }

        public byte Code { get; set; }


        public string Name { get; set; }


        public ulong SeriesNumber { get; set; }
    }
}
