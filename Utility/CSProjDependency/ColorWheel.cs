using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HP.ScalableTest.Utility.VisualStudio
{
    public static class ColorWheel
    {
        static List<string> _color = new List<string>()
            {
                "lawngreen",
                "lightblue",
                "lightcoral",
                "magenta",
                "lightseagreen",
                "maroon",
                "limegreen",
                "lightpink",
                "mediumorchid",
                "mediumpurple",
                "mediumspringgreen",
                "darkblue",
                "yellow3",
                "red",
                "green",
                "blue",
                "orange",
                "olivedrab1",
                "orangered",
                "purple",
                "skyblue",
                "dodgerblue",
                "firebrick",
                "violetred",
                "tomato",
                "wheat",
                "yellowgreen",
                "tan",
                "royalblue",
                "lightblue2",
                "lightcyan2",
                "magenta2",
                "maroon2",
                "lightpink2",
                "mediumorchid2",
                "mediumpurple2",
                "yellow2",
                "red2",
                "green2",
                "blue2",
                "orange2",
                "olivedrab2",
                "orangered2",
                "purple2",
                "skyblue2",
                "dodgerblue2",
                "firebrick2",
                "violetred2",
                "tomato2",
                "wheat2",
                "yellowgreen2",
                "tan2",
                "royalblue2",
            };

        public static string Select(int offset)
        {
            return _color[offset % _color.Count];
        }
    }
}
