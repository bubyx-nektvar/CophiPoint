namespace CophiPoint.Models
{
    public enum Unit
    {
        MiliLiters,
        Grams
    }

    public static class UnitExtensiongs
    {
        public static string ToAbbrevation(this Unit unit)
        {
            switch (unit)
            {
                case Unit.Grams: return "g";
                case Unit.MiliLiters: return "ml";
                default: return "";
            }
        }
    }
}