using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PassDash
{

    [XmlRoot("password_list")]
    public class PasswordList
    {
        public PasswordList() { Items = new List<Password>(); }
        [XmlElement("password")]
        public List<Password> Items { get; set; }
    }


    [Serializable()]
    [XmlType("password")]
    public class Password
    {

        [XmlElement("nr")]
        public string nr { get; set; }

        [XmlElement("id")]
        public string id { get; set;}

        [XmlElement("category")]
        public string category { get; set; }

        [XmlElement("name")]
        public string name { get; set; }

        [XmlElement("website")]
        public string website { get; set; }

        [XmlElement("userName")]
        public string userName { get; set; }

        [XmlElement("userPassword")]
        public string userPassword { get; set; }

        [XmlElement("dateTime")]
        public string dateTime { get; set; }

        [XmlElement("note")]
        public string note { get; set; }

        [XmlElement("masterPassword")]
        public string masterPassword { get; set; }

        [XmlElement("strength")]
        public string strength { get; set; }

    }

}
