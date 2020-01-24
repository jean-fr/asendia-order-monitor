using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Asendia.Order.Monitor
{
    public class FileGenerator : IFileGenerator
    {
        private string _outputDir;
        public int Execute(OrderOptions opts)
        {
            if (string.IsNullOrEmpty(opts.Source))
            {
                MessageHelper.WriteLine(LogLevel.Error, "Please provide the source directory FULL PATH");
                return -1;
            }

            if (!Directory.Exists(opts.Source))
            {
                MessageHelper.WriteLine(LogLevel.Error, "The provided source directory doesn't exist, please provide an existing one");
                return -1;
            }

            if (string.IsNullOrEmpty(opts.Output))
            {
                MessageHelper.WriteLine(LogLevel.Error, "Please provide the output directory FULL PATH");
                return -1;
            }

            if (!Directory.Exists(opts.Output))
            {
                MessageHelper.WriteLine(LogLevel.Error, "The provided output directory doesn't exist, please provide an existing one");
                return -1;
            }

            var files = Directory.GetFiles(opts.Source, "*.csv");
            if (files.Length == 0)
            {
                MessageHelper.WriteLine(LogLevel.Error, "No CSV file was found in the source directory, please make sure at least one CSV file exists");
                return -1;
            }

            this._outputDir = opts.Output;
            var proceeded = 0;

            foreach (var filePath in files)
            {
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    if (!File.Exists(filePath))
                    {
                        MessageHelper.WriteLine(LogLevel.Error, $"File {fileName}.csv doesn't exist");
                        continue;
                    }

                    MessageHelper.WriteLine(LogLevel.Info, $"Processing File {fileName}.csv ...");
                    var result = this.ProcessFile(filePath, fileName);

                    if (!result.Success)
                    {
                        MessageHelper.WriteLine(LogLevel.Error, $"File {fileName}.xml generation failed - ERROR | {result.Error}");
                    }
                    else
                    {
                        MessageHelper.WriteLine(LogLevel.Info, $"File {fileName}.xml has been generated successfully");
                    }

                    proceeded++;
                    MessageHelper.DisplayProgress(proceeded, files.Count());
                }
                catch (Exception ex)
                {
                    MessageHelper.WriteLine(LogLevel.Error, $"An Error has occured with File {filePath}. ERROR | {ex.Message}");
                    continue;
                }
            }

            return 0;
        }

        private FileGenResult ProcessFile(string filePath, string fileName)
        {
            var result = new FileGenResult { Success = true };

            var lines = File.ReadAllLines(filePath);
            var headersLine = lines.Length > 0 ? lines.First() : string.Empty;

            if (string.IsNullOrEmpty(headersLine))
            {
                result.Success = false;
                result.Error = $"File {filePath} is missing headers";
                return result;
            }

            var headers = headersLine.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            headers = headers.Select(x => x.Trim()).ToArray();

            if (!headers.All(h => DataHelper.HeaderMapping.ContainsValue(h)))
            {
                result.Success = false;
                result.Error = $"File {filePath} has wrong headers";
                return result;
            }


            var orders = lines.Skip(1);

            var ordersList = this.GetDataList(orders, headers);
            if (!ordersList.Any())
            {
                /*Shouldn't happen*/
                result.Success = false;
                result.Error = $"Something went wromg for File {fileName} | collection is empty";
                return result;
            }

            var orderCollection = new OrderCollection();

            /*Fill object collections*/
            var groupedOrders = ordersList.GroupBy(o => o.OrderNumber);
            foreach (var group in groupedOrders)
            {
                var k = group.Key;

                foreach (var o in group)
                {
                    if (orderCollection.FirstOrDefault(x => x.Number == k) != null)
                    {
                        //already in
                        this.FillOrder(o, orderCollection.FirstOrDefault(x => x.Number == k));
                    }
                    else
                    {
                        var order = new Order { Number = k, ShippingAddress = new ShippingAddress { Address1 = o.Address1, Address2 = o.Address2, City = o.City, CountryCode = o.CountryCode, State = o.State } };
                        this.FillOrder(o, order);
                        orderCollection.Add(order);
                    }
                }
            }

            orderCollection.Save(Path.Combine(this._outputDir, $"{fileName}.xml"));
            result.Success = true;
            return result;
        }

        private void FillOrder(CsvLineData orderData, Order order)
        {
            var cons = new Consignment { ConsigneeName = orderData.ConsigneeName, Number = orderData.ConsignmentNumber, OrderNumber = orderData.OrderNumber };
            var parcel = new Parcel { Code = orderData.ParcelCode, ConsignmentNumber = orderData.ConsignmentNumber };
            var item = new Item { Currency = orderData.ItemCurrency, Description = orderData.ItemDescription, ParcelCode = orderData.ParcelCode, Quantity = orderData.ItemQuantity, Value = orderData.ItemValue, Weight = orderData.ItemWeight };

            parcel.Items.Add(item);
            cons.Parcels.Add(parcel);
            order.Consignments.Add(cons);
        }

        private List<CsvLineData> GetDataList(IEnumerable<string> orders, string[] headers)
        {
            var ordersList = new List<CsvLineData>();
            foreach (var order in orders)
            {
                var dataArray = order.Split(new string[] { "," }, StringSplitOptions.None);
                dataArray = dataArray.Select(x => x.Trim()).ToArray();

                var dataLine = new CsvLineData();

                for (var i = 0; i < dataArray.Length; i++)
                {
                    var mappingKey = DataHelper.HeaderMapping.FirstOrDefault(h => h.Value == headers[i]).Key;

                    switch (mappingKey)
                    {
                        case nameof(dataLine.OrderNumber):
                            dataLine.OrderNumber = dataArray[i];
                            break;
                        case nameof(dataLine.ConsignmentNumber):
                            dataLine.ConsignmentNumber = dataArray[i];
                            break;
                        case nameof(dataLine.ConsigneeName):
                            dataLine.ConsigneeName = dataArray[i];
                            break;
                        case nameof(dataLine.ParcelCode):
                            dataLine.ParcelCode = dataArray[i];
                            break;
                        case nameof(dataLine.ItemValue):
                            if (!string.IsNullOrEmpty(dataArray[i]))
                            {
                                if (double.TryParse(dataArray[i], out double v))
                                {
                                    dataLine.ItemValue = v;
                                }
                            }
                            break;
                        case nameof(dataLine.ItemQuantity):
                            if (!string.IsNullOrEmpty(dataArray[i]))
                            {
                                if (int.TryParse(dataArray[i], out int v))
                                {
                                    dataLine.ItemQuantity = v;
                                }
                            }
                            break;
                        case nameof(dataLine.ItemDescription):
                            dataLine.ItemDescription = dataArray[i];
                            break;
                        case nameof(dataLine.ItemCurrency):
                            dataLine.ItemCurrency = dataArray[i];
                            break;
                        case nameof(dataLine.ItemWeight):
                            if (!string.IsNullOrEmpty(dataArray[i]))
                            {
                                if (double.TryParse(dataArray[i], out double v))
                                {
                                    dataLine.ItemWeight = v;
                                }
                            }
                            break;
                        case nameof(dataLine.Address1):
                            dataLine.Address1 = dataArray[i];
                            break;
                        case nameof(dataLine.Address2):
                            dataLine.Address2 = dataArray[i];
                            break;
                        case nameof(dataLine.City):
                            dataLine.City = dataArray[i];
                            break;
                        case nameof(dataLine.State):
                            dataLine.State = dataArray[i];
                            break;
                        case nameof(dataLine.CountryCode):
                            dataLine.CountryCode = dataArray[i];
                            break;

                    }
                }

                ordersList.Add(dataLine);
            }
            return ordersList;
        }
    }
}
