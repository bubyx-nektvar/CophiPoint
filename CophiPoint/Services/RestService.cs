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
                    Price = 250 * 0.2m,
                    ImageUrl = "https://mir-s3-cdn-cf.behance.net/project_modules/disp/c141c516761284.5603ad9d98d09.jpg"
                },
                new Product
                {
                    Name = "Tea",
                    Unit = Unit.MiliLiters,
                    DefaultSize = 100,
                    PricePerUnit = 0.1m,
                    Price = 0.1m * 100,
                    ImageUrl = "https://mir-s3-cdn-cf.behance.net/project_modules/disp/c141c516761284.5603ad9d98d09.jpg"
                }
            };
        }
    }
}
