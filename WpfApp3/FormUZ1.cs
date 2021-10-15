using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Applicant_Class;
//using WpfApp3.Pledgee_Classes;
using WpfApp3.Pledgor_Class;
using WpfApp3.Property_Class;

namespace WpfApp3
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://fciit.ru/eisn/ruzdi/types/2.3")]
    public class FormUZ1
    {
        public FormUZ1()
        {
        }

        public FormUZ1(List<PersonalProperty> PersonalProperties, List<Pledgor> Pledgors, List<Pledgee> Pledgee, PledgeContract PledgeContract, NotificationApplicant NotificationApplicant)
        {
            this.PersonalProperties = PersonalProperties;
            this.Pledgors = Pledgors;
            this.Pledgee = Pledgee;
            this.PledgeContract = PledgeContract;
            this.NotificationApplicant = NotificationApplicant;

        }
        [System.Xml.Serialization.XmlArrayItemAttribute("PersonalProperty", IsNullable = false)]
        public List<PersonalProperty> PersonalProperties { get; set; }


        [System.Xml.Serialization.XmlArrayItemAttribute("Pledgor", IsNullable = false)]
        public List<Pledgor> Pledgors { get; set; }


        [System.Xml.Serialization.XmlArrayItemAttribute("Pledgee", IsNullable = false)]
        public List<Pledgee> Pledgee { get; set; }

        public PledgeContract PledgeContract { get; set; }

        public NotificationApplicant NotificationApplicant { get; set; }
    }
}
