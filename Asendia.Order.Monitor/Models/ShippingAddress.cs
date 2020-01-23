using System.Xml.Linq;

namespace Asendia.Order.Monitor
{
    public class ShippingAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }

        public XElement ToXElement(XNamespace xmns)
        {
            XElement xShippingAddress = new XElement(xmns + "ShippingAddress");

            XElement xAddress1 = new XElement(xmns + "Address1") { Value = this.Address1 };
            XElement xAddress2 = new XElement(xmns + "Address2") { Value = this.Address2 };
            XElement xCity = new XElement(xmns + "City") { Value = this.City };
            XElement xState = new XElement(xmns + "State") { Value = this.State };
            XElement xCountryCode = new XElement(xmns + "CountryCode") { Value = this.CountryCode };
           
            xShippingAddress.Add(xAddress1);
            xShippingAddress.Add(xAddress2);
            xShippingAddress.Add(xCity);
            xShippingAddress.Add(xState);
            xShippingAddress.Add(xCountryCode);

            return xShippingAddress;
        }
    }
}
