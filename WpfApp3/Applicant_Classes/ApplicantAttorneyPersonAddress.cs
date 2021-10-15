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
    public class ApplicantAttorneyPersonAddress
    {
        public ApplicantAttorneyPersonAddress()
        {
        }

        public ApplicantAttorneyPersonAddress(ApplicantAttorneyPersonAddressAddressRF AddressRF)
        {
            this.AddressRF = AddressRF;


        }

        public ApplicantAttorneyPersonAddressAddressRF AddressRF { get; set; }
    }
}
