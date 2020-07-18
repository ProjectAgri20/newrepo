using System.ComponentModel;

namespace HP.ScalableTest.Plugin.PrinterOnMobile
{
    public enum Option_Orientation
    {
        [Description("Portrait")]
        Portrait,

        [Description("Landscape")]
        Landscape
    }

    public enum Option_Duplex
    {
        [Description("None")]
        None,

        [Description("Short Edge")]
        ShortEdge,

        [Description("Long Edge")]
        LongEdge
    }

    public enum Option_Color
    {
        [Description("Black & White")]
        BlackWhite,

        [Description("Color")]
        Color
    }

    public enum Option_PaperSize
    {
        [Description("A4 - 210 x 297 mm")]
        A4,

        [Description("A3 - 297 x 420 mm")]
        A3,

        [Description("A5 - 148 x 210 mm")]
        A5,

        [Description("A6 - 105 x 148 mm")]
        A6,

        [Description("B4 (JIS) - 250 x 354")]
        B4_JIS,

        [Description("B5 (JIS) - 182 x 257 mm")]
        B5_JIS,

        [Description("Executive - 7 1/4 x 10 1/2 in")]
        Executive,

        [Description("Folio - 8 1/2 x 13 in")]
        Folio,

        [Description("Legal - 8 1/2 x 14 in")]
        Legal,

        [Description("Letter - 8 1/2 x 11 in")]
        Letter
    }   

}
