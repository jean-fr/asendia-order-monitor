using System.Collections.Generic;
using System.Xml.Linq;

namespace Asendia.Order.Monitor
{
    public class Consignment
    {
        public string OrderNumber { get; set; }
        public string ConsigneeName { get; set; }
        public string Number { get; set; }
        public List<Parcel> Parcels { get; set; } = new List<Parcel>();

        public XElement ToXElement(XNamespace xmns)
        {
            XElement xConsignment = new XElement(xmns + "Consignment");

            XElement xConsNumber = new XElement(xmns + "Number") { Value = this.Number };
            XElement xConsigneeName = new XElement(xmns + "ConsigneeName") { Value = this.ConsigneeName };
            xConsignment.Add(xConsNumber);
            xConsignment.Add(xConsigneeName);
            XElement xParcels = new XElement(xmns + "Parcels");

            foreach (var parcel in this.Parcels)
            {
                xParcels.Add(parcel.ToXElement(xmns));
            }
            xConsignment.Add(xParcels);

            return xConsignment;
        }
    }
}
