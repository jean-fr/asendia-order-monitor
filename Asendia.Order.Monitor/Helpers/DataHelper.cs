using System.Collections.Generic;

namespace Asendia.Order.Monitor
{
    public static class DataHelper
    {       
        public static Dictionary<string, string> HeaderMapping
        {
            get
            {
                return new Dictionary<string, string> {
                    {"OrderNumber","Order No" },
                    {"ConsignmentNumber","Consignment No" },
                    {"ParcelCode","Parcel Code" },
                    {"ConsigneeName","Consignee Name" },
                    {"Address1","Address 1" },
                    {"Address2","Address 2" },
                    {"City","City" },
                    {"State","State" },
                    {"CountryCode","Country Code" },
                    {"ItemQuantity","Item Quantity" },
                    {"ItemValue","Item Value" },
                    {"ItemWeight","Item Weight" },
                    {"ItemDescription","Item Description" },
                    {"ItemCurrency","Item Currency" }
                };
            }
        }


    }
}
