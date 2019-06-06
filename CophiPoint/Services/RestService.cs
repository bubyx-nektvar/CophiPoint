using CophiPoint.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Services
{
    public class RestService
    {
        public List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Name = "Coffee",
                    Unit = Unit.MiliLiters,
                    DefaultSize = 250,
                    PricePerUnit = 0.2m,
                    ImageUrl = "https://png.pngtree.com/svg/20160714/d51538fd9c.svg"
                }
            };
        }
    }
}
