using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Asendia.Order.Monitor
{
    public class OrderCollection : List<Order>
    {
        private readonly XNamespace xnameSpace = XNamespace.Get(@"http://www.w3.org/TR/xml-names11");

        public void Save(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new Exception("XML File Path NOT provided");
            }
            this.GetXDocument().Save(filePath);
        }

        private XDocument GetXDocument()
        {
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            xdoc.Add(new XElement(xnameSpace + "Orders"));

            foreach (Order order in this)
            {
                if (xdoc.Root != null)
                    xdoc.Root.Add(order.ToXElement(xnameSpace));
            }
            return xdoc;
        }
    }
}
