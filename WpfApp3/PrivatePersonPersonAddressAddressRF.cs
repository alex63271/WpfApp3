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
    public class PrivatePersonPersonAddressAddressRF
    {



        public PrivatePersonPersonAddressAddressRF(string RegionCode, string Region, bool registration)
        {
            this.RegionCode = RegionCode;
            this.Region = Region;
            this.registration = registration;

        }

        public PrivatePersonPersonAddressAddressRF()
        {
        }

        public string RegionCode { get; set; }

        public string Region { get; set; }


        public string District { get; set; }


        public string City { get; set; }


        public string Locality { get; set; }


        public string Street { get; set; }


        public string House { get; set; }


        public string Building { get; set; }


        public string Apartment { get; set; }


        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool registration { get; set; }
    }

}
