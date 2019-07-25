using System.Collections.Generic;

namespace CophiPoint.Models
{
    public enum Unit
    {
        MiliLiters,
        Grams
    }

    public static class UnitExtensiongs
    {

        private static IDictionary<Unit, UnitViewInfo> appearance = new Dictionary<Unit, UnitViewInfo> {
            {Unit.MiliLiters, new UnitViewInfo
            {
                Abbrevation = "ml",
                ImageSource = "water_glass.png"
            } },
            { Unit.Grams, new UnitViewInfo
            {
                Abbrevation = "g",
                ImageSource = "weight.png"
            } }
        };
        public static string ToAbbrevation(this Unit unit) => appearance[unit].Abbrevation;
        public static string ImageSource(this Unit unit) => appearance[unit].ImageSource;

        private struct UnitViewInfo
        {
            public string Abbrevation;
            public string ImageSource;
        }
    }
}