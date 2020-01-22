using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Asendia.Order.Monitor
{
    public class Order
    {
        public string Number { get; set; }
        public List<Consignment> Consignments { get; set; }

        private double TotalValue
        {
            get
            {
                return this.Consignments.SelectMany(c => c.Parcels).SelectMany(p => p.Items).Sum(i => i.Value);
            }
        }
        private double TotalWeight
        {
            get
            {
                return this.Consignments.SelectMany(c => c.Parcels).SelectMany(p => p.Items).Sum(i => i.Weight);
            }
        }
        public XElement ToXElement(XNamespace xmns)
        {
            XElement xOrder = new XElement(xmns + "Order");

            XElement xNumber = new XElement(xmns + "Number") { Value = this.Number };
            XElement xTotalValue = new XElement(xmns + "TotalValue") { Value = this.TotalValue.ToString() };
            XElement xTotalWeight = new XElement(xmns + "TotalWeight") { Value = this.TotalWeight.ToString() };

            xOrder.Add(xTotalValue);
            xOrder.Add(xTotalWeight);

            XElement xConsignments = new XElement(xmns + "Consignments");

            foreach (var cons in this.Consignments)
            {
                xConsignments.Add(cons.ToXElement(xmns));
            }

            xOrder.Add(xNumber);
            xOrder.Add(xConsignments);
            return xOrder;
        }
    }
}
