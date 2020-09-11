using System;
using CsvHelper.Configuration;
using System.ComponentModel.DataAnnotations;
namespace Batch.Models
{
    public class StoreOrder
    {
        public int ID {get;set;}
        public string ORDER_ID { get; set; }
        public DateTime ORDER_DATE { get; set; }
        public DateTime SHIP_DATE { get; set; }
        public string SHIP_MODE { get; set; }
        public int QUANTITY { get; set; }
        public double DISCOUNT { get; set; }
        public double PROFIT { get; set; }
        public string PRODUCT_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CATEGORY { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
    }
    public sealed class  StoreOrderMap : ClassMap<StoreOrder>
    {
        public StoreOrderMap()
        {
            Map(m=> m.ID).Ignore();
            Map(m=> m.ORDER_ID).Name("Order ID");
            Map(m=> m.ORDER_DATE).Name("Order Date");
            Map(m=> m.SHIP_DATE).Name("Ship Date");
            Map(m=> m.SHIP_MODE).Name("Ship Mode");
            Map(m=> m.QUANTITY).Name("Quantity");
            Map(m=> m.DISCOUNT).Name("Discount");
            Map(m=> m.PROFIT).Name("Profit");
            Map(m=> m.PRODUCT_ID).Name("Product ID");
            Map(m=> m.CUSTOMER_NAME).Name("Customer Name");
            Map(m=> m.CATEGORY).Name("Category");
            Map(m=> m.CUSTOMER_ID).Name("Customer ID");
            Map(m=> m.PRODUCT_NAME).Name("Product Name");
       }
    }
}