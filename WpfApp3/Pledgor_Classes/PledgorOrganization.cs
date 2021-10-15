using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Pledgor_Class
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class PledgorOrganization
    {
        public PledgorOrganization()
        {
        }

        public PledgorOrganization(RussianOrganization RussianOrganization)
        {
            this.RussianOrganization = RussianOrganization;
        }

        public RussianOrganization RussianOrganization { get; set; }
    }

}
