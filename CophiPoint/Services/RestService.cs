using CophiPoint.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CophiPoint.Services
{
    public class RestService
    {
        public List<Product> GetProducts()
        {
            return new Product[]
            {
                new Product
                {
                    Name = "Coffee",
                    Unit = Unit.MiliLiters,
                    Sizes = new []{
                        new Size {
                            UnitsCount = 50,
                            TotalPrice = 10,
                        },new Size {
                            UnitsCount = 100,
                            TotalPrice = 15,
                        },new Size {
                            UnitsCount = 150,
                            TotalPrice = 20,
                        },new Size {
                            UnitsCount = 250,
                            TotalPrice = 25,
                        }
                    },
                    Image = new Uri("data:image/png;base64,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")
                },

                new Product
                {
                    Name = "Coffee",
                    Unit = Unit.MiliLiters,
                    Sizes = new []{
                        new Size {
                            UnitsCount = 50,
                            TotalPrice = 10,
                        },new Size {
                            UnitsCount = 100,
                            TotalPrice = 15,
                        },new Size {
                            UnitsCount = 150,
                            TotalPrice = 20,
                        },new Size {
                            UnitsCount = 200,
                            TotalPrice = 25,
                        },
                        new Size {
                            UnitsCount = 250,
                            TotalPrice = 30,
                        },new Size {
                            UnitsCount = 300,
                            TotalPrice = 35,
                        },new Size {
                            UnitsCount = 350,
                            TotalPrice = 40,
                        },new Size {
                            UnitsCount = 400,
                            TotalPrice = 45,
                        }
                    },
                    Image = new Uri("data:image/png;base64,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")
                },
                new Product
                {
                    Name = "Tea",
                    Unit = Unit.MiliLiters,
                    Sizes = new []{new Size {
                        UnitsCount = 100,
                        TotalPrice = 12,
                    } },
                    Image = new Uri("data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTcuMS4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgdmlld0JveD0iMCAwIDM0NS44NTUgMzQ1Ljg1NSIgc3R5bGU9ImVuYWJsZS1iYWNrZ3JvdW5kOm5ldyAwIDAgMzQ1Ljg1NSAzNDUuODU1OyIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSIgd2lkdGg9IjUxMnB4IiBoZWlnaHQ9IjUxMnB4Ij4KPGc+Cgk8cGF0aCBkPSJNMTE4LjgwMywxMjEuMzg1YzEuMDQyLDAuNzEsMi4yNjUsMS4wODUsMy41MzYsMS4wODVoMC4wMTZjMy40NzQsMCw2LjMwMS0yLjgyNSw2LjMwMS02LjI5NyAgIGMwLTAuNjA1LTAuMDg1LTEuMjAyLTAuMjUzLTEuNzc5Yy0zLjc4Ny0xNi41ODItMS45ODctMjkuOTQ0LDUuMzQ5LTM5LjcxOGM4LjU1MS0xMS4zOTMsMTIuMzM5LTI2LjAwOSwxMC4xMzMtMzkuMDk3ICAgYy0xLjk0Ny0xMS41ODgtOC4xNzctMjEuMjItMTguMDEtMjcuODQ5Yy0yLjEwNy0xLjQyOC01LjAwNi0xLjQxNS03LjEwNCwwLjAyOWMtMi4xNDQsMS40NzYtMy4xNTEsNC4wNzItMi41NjEsNi42MjggICBjNC4wODksMTcuNDQzLDAuMzc2LDI4LjQ4NC04LjkzNiw0NS4yMzZjLTUuNzU0LDEwLjM0OS04LjEwNCwyMS45NzItNi42MTgsMzIuNzI4QzEwMi4zMzYsMTA0LjUwMywxMDguNjIsMTE0LjU0OSwxMTguODAzLDEyMS4zODUgICB6IiBmaWxsPSIjMDAwMDAwIi8+Cgk8cGF0aCBkPSJNMTcyLjQ5NywxMTguNTg0YzAuOTg0LDAuNjczLDIuMTMzLDEuMDI4LDMuMzIzLDEuMDI4aDAuMDA4YzMuMjYxLDAsNS45MTQtMi42NTQsNS45MTQtNS45MTYgICBjMC0wLjU1Ni0wLjA3OS0xLjEwNy0wLjIzMy0xLjY0MmMtMi44NzUtMTIuNjM4LTEuNTI1LTIyLjc4OSw0LjAxNS0zMC4xNzFjNi44MzktOS4xMDgsOS44NjYtMjAuODA3LDguMS0zMS4yOTIgICBjLTEuNTY3LTkuMzItNi41NzktMTcuMDctMTQuNDk0LTIyLjQxMWMtMS45NzEtMS4zMzItNC42MzgtMS4zNDktNi42NzksMC4wNDFjLTEuOTkxLDEuMzgyLTIuOTQ1LDMuODkyLTIuMzgyLDYuMjA5ICAgYzMuMTE5LDEzLjMwNywwLjI2NiwyMS43NjctNi44ODQsMzQuNjIzYy00LjU5OSw4LjI3Mi02LjQ3NSwxNy41NjktNS4yODMsMjYuMTc2QzE1OS4yNTIsMTA1LjAwOCwxNjQuMzEsMTEzLjA5MSwxNzIuNDk3LDExOC41ODR6ICAgIiBmaWxsPSIjMDAwMDAwIi8+Cgk8cGF0aCBkPSJNMzQwLjQ3OCwxNTguOTg0Yy0zLjkyMy00LjYxOC05LjUzNi03LjE2MS0xNS44MDYtNy4xNjFoLTMwLjQ1MmMwLjQ2OS01LjI2MS0wLjM3My0xMi4xMzgtNC4yMjgtMTYuNjc2ICAgYy0yLjY5My0zLjE3LTYuNTMyLTQuODQ1LTExLjEwNC00Ljg0NUgxNy4wODljLTYuNzgsMC0xMC42ODIsMi45MTUtMTIuNzYxLDUuMzZjLTQuMjY5LDUuMDItNC44NTUsMTIuNTQ4LTMuOTgxLDE3LjkyICAgQzcuNTUzLDE5Ny44NTYsMjMuMTIsMjM4LjA3MSw0NC4xOCwyNjYuODJjMTIuNjczLDE3LjI5OSwyNy4xMTMsMzAuNTU2LDQzLjIxMywzOS43MjhjLTIuMzYxLDMuMzY5LTMuNzU5LDcuNDYxLTMuNzU5LDExLjg3OCAgIGMwLDExLjQ0Nyw5LjMxMywyMC43NiwyMC43NjEsMjAuNzZoODUuNTVjMTEuNDQ4LDAsMjAuNzYxLTkuMzEzLDIwLjc2MS0yMC43NmMwLTQuMzQyLTEuMzQ0LTguMzczLTMuNjMyLTExLjcwOSAgIGMxMi45NDUtNy4zLDI0Ljc4Ny0xNy4yMTUsMzUuNDctMjkuNzI4YzUxLjA1OC00LjczOSw5NS43NTMtNTMuNjgyLDEwMi45NjMtOTcuNjhDMzQ1Ljk3LDE3Ni40NzYsMzQ2Ljk3OSwxNjYuNjM4LDM0MC40NzgsMTU4Ljk4NCAgIHogTTI2Ny45OCwyMzcuODc1YzkuMzQxLTE4LjgwNiwxNS43NzktMzguMjk2LDIwLjIxNy01Ni4wNTFoMjYuMDA2QzMwNy45NTksMjAzLjAwNSwyODkuOTg4LDIyNS41MjIsMjY3Ljk4LDIzNy44NzV6IiBmaWxsPSIjMDAwMDAwIi8+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPC9zdmc+Cg==")
                }
            }
            .ToList();
        }
        public List<PurchasedItem> GetPurchases()
        {
            var l = new List<PurchasedItem>();

            l.AddRange(Enumerable.Range(1, 5).Select(x => new PurchasedItem
            {
                ProductName = "Coffee",
                Date = DateTime.Now.AddDays(-x),
                TotalPrice = 25.22m
            }));

            l.AddRange(Enumerable.Range(1, 7).Select(x => new PurchasedItem
            {
                ProductName = "Tea",
                Date = DateTime.Now.AddDays(-x),
                TotalPrice = 25.22m
            }));

            l.Sort((a, b) => a.Date.CompareTo(b.Date));
            return l;
        }
    }
}
