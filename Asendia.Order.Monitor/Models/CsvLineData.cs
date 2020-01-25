namespace Asendia.Order.Monitor
{
    public class CsvLineData
    {
        public string OrderNumber { get; set; }
        public string ConsignmentNumber { get; set; }
        public string ParcelCode { get; set; }
        public string ConsigneeName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public int ItemQuantity { get; set; }
        public double ItemValue { get; set; }
        public double ItemWeight { get; set; }
        public string ItemDescription { get; set; }
        public string ItemCurrency { get; set; }
        
    }
}
