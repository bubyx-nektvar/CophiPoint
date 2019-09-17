using FFImageLoading.Svg.Forms;
using System.Collections.Generic;

namespace CophiPoint.Api.Models
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
                ImageSource = SvgImageSource.FromResource("CophiPoint.Resources.volume.svg")
            } },
            { Unit.Grams, new UnitViewInfo
            {
                Abbrevation = "g",
                ImageSource = SvgImageSource.FromResource("CophiPoint.Resources.weight.svg")
            } }
        };
        public static string ToAbbrevation(this Unit unit) => appearance[unit].Abbrevation;
        public static SvgImageSource ImageSource(this Unit unit) => appearance[unit].ImageSource;

        private struct UnitViewInfo
        {
            public string Abbrevation;
            public SvgImageSource ImageSource;
        }
    }
}