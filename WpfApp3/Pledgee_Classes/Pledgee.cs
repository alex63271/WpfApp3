using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using WpfApp3.Pledgee_Class;

namespace WpfApp3
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class Pledgee
    {
        public Pledgee()
        {
        }

        public Pledgee(PledgeePrivatePerson PrivatePerson)
        {
            this.PrivatePerson = PrivatePerson;

        }
        public Pledgee(PledgeeOrganization Organization)
        {
            this.Organization = Organization;
        }


        public PledgeePrivatePerson PrivatePerson { get; set; }


        public PledgeeOrganization Organization { get; set; }
    }
}
