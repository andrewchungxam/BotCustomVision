using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisAndQandA1
{
    [Serializable]
    public class CustomerInfo
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfPurchase { get; set; }

    }
}