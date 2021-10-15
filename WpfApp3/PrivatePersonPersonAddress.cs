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
    public class PrivatePersonPersonAddress
    {
        public PrivatePersonPersonAddress()
        {
        }

        public PrivatePersonPersonAddress(PrivatePersonPersonAddressAddressRF AddressRF)
        {
            this.AddressRF = AddressRF;
        }
        public PrivatePersonPersonAddressAddressRF AddressRF { get; set; }
    }
}
