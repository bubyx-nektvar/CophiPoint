using System;
using System.Collections.Generic;
using System.Text;
using CophiPoint.Converters;
using Xamarin.Forms;

namespace CophiPoint.Models
{
    public class ProductViewModel: Product
    {
        public int SelectedSizeIndex { get; set; }

        public decimal Price { get; set; }

        public decimal PricePerUnit { get; set; }

        public Size SelectedSize { get; set; }
        public SizeInfoConverter.SizeDefinition SelectedSizeDefinition { get; private set; }
        public bool Favorite { get; set; }
        public static ProductViewModel Empty => new ProductViewModel(new Product()
        {
            Sizes = new[]
            {
                new Size()
                {
                    Price = 1,
                    UnitsCount = 1
                }
            },
            DefaultSizeIndex = 0,
            Unit = Unit.MiliLiters,
            Name="",
            ImageUrl = new Uri("data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNCIgaGVpZ2h0PSIyNCIgdmlld0JveD0iMCAwIDI0IDI0Ij48cGF0aCBkPSJNMCAwaDI0djI0SDB6IiBmaWxsPSJub25lIi8+PHBhdGggZD0iTTIxIDE5VjVjMC0xLjEtLjktMi0yLTJINWMtMS4xIDAtMiAuOS0yIDJ2MTRjMCAxLjEuOSAyIDIgMmgxNGMxLjEgMCAyLS45IDItMnpNOC41IDEzLjVsMi41IDMuMDFMMTQuNSAxMmw0LjUgNkg1bDMuNS00LjV6Ii8+PC9zdmc+")
        }, false);

        public ProductViewModel(ProductViewModel mode)
            : this(mode, mode.Favorite)
        { }

        public ProductViewModel(Product product, bool isFavorite)
            :base(product)
        {
            Favorite = isFavorite;
            UseSize(DefaultSizeIndex);
        }
        public void UseSize(decimal unitCount)
        {
            for(int i =0; i <Sizes.Length; ++i)
            {
                if(Sizes[i].UnitsCount == unitCount)
                {
                    UseSize(i);
                    return;
                }
            }
            throw new ArgumentException($"Size of {unitCount} not found");
        }
        public void UseSize(int index)
        {
            SelectedSizeIndex = index;
            SelectedSize = Sizes[SelectedSizeIndex];
            SelectedSizeDefinition = new SizeInfoConverter.SizeDefinition(SelectedSize, Unit);
            Price = SelectedSize.Price;
            PricePerUnit = SelectedSize.PricePerUnit;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
