using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Models
{
    public class ProductViewModel: Product
    {
        public int SelectedSizeIndex { get; set; }

        public decimal Price { get; set; }

        public decimal PricePerUnit { get; set; }

        public bool Favorite { get; set; }
        public bool NotFavorite => !Favorite;

        public ProductViewModel(Product product, bool isFavorite)
            :base(product)
        {
            Favorite = isFavorite;
            UseSize(DefaultSizeIndex);
        }

        public void UseSize(int index)
        {
            SelectedSizeIndex = index;
            Price = Sizes[SelectedSizeIndex].Price;
            PricePerUnit = Sizes[SelectedSizeIndex].PricePerUnit;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
