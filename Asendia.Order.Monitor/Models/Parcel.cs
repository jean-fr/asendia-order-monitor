using System.Collections.Generic;
using System.Xml.Linq;

namespace Asendia.Order.Monitor
{
    public class Parcel
    {
        public string Code { get; set; }
        public string ConsignmentNumber { get; set; }
        public List<Item> Items { get; set; }

        public XElement ToXElement(XNamespace xmns)
        {
            XElement xParcel = new XElement(xmns + "Parcel");

            XElement xCode = new XElement(xmns + "Code") { Value = this.Code };

            XElement xItems = new XElement(xmns + "Items");          

            foreach (var item in this.Items)
            {            
                xItems.Add(item.ToXElement(xmns));
            }

            xParcel.Add(xCode);
            xParcel.Add(xItems);
           
            return xParcel;
        }
    }
}
