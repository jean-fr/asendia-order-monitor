using System.Xml.Linq;

namespace Asendia.Order.Monitor
{
    public class Item
    {
        private string _defaultCurrency = "GBP";
        public int Quantity { get; set; }
        public double Value { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
        public string Currency
        {
            get { return this._defaultCurrency; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this._defaultCurrency = value;
                }
            }
        }
        public string ParcelCode { get; set; }

        public XElement ToXElement(XNamespace xmns)
        {
            XElement xItem = new XElement(xmns + "Item");

            XElement xQuantity = new XElement(xmns + "Quantity") { Value = this.Quantity.ToString() };
            XElement xValue = new XElement(xmns + "Value") { Value = this.Value.ToString() };
            XElement xWeight = new XElement(xmns + "Weight") { Value = this.Weight.ToString() };
            XElement xDescription = new XElement(xmns + "Descrition") { Value = this.Description };
            XElement xCurrency = new XElement(xmns + "Currency") { Value = this.Currency };

            xItem.Add(xQuantity);
            xItem.Add(xValue);
            xItem.Add(xWeight);
            xItem.Add(xDescription);
            xItem.Add(xCurrency);

            return xItem;
        }
    }
}
