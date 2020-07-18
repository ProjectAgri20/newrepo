using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Collections.Specialized;
using System.Linq;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Enum that lists "moods" of colors.
    /// </summary>
    [Flags]
    public enum ColorMoods
    {
        /// <summary>
        /// All colors
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Success colors - greens and lighter blues
        /// </summary>
        Success = 0x1,

        /// <summary>
        /// Failure colors - reds, oranges, and yellows
        /// </summary>
        Failure = 0x2,

        /// <summary>
        /// Netural colors - blues, purples, and earth tones
        /// </summary>
        Neutral = 0x4,

        /// <summary>
        /// All colors
        /// </summary>
        All = Success | Failure | Neutral
    }

    /// <summary>
    /// Class that provides a list of colors.
    /// </summary>
    public class ColorWheel
    {
        #region Colors dictionary
        private static Dictionary<Color, ColorMoods> _colors = new Dictionary<Color, ColorMoods>()
        {
            {Color.Red,             ColorMoods.Failure},
            {Color.Green,           ColorMoods.Success},
            {Color.Blue,            ColorMoods.Neutral},
            {Color.Orange,          ColorMoods.Failure},
            {Color.Purple,          ColorMoods.Neutral},
            {Color.Yellow,          ColorMoods.Failure},
            {Color.DodgerBlue,      ColorMoods.Success},
            {Color.Olive,           ColorMoods.Neutral},
            {Color.YellowGreen,     ColorMoods.Success},
            {Color.BlueViolet,      ColorMoods.Neutral},
            {Color.DarkRed,         ColorMoods.Failure},
            {Color.GreenYellow,     ColorMoods.Success},
            {Color.RoyalBlue,       ColorMoods.Neutral},
            {Color.Gold,            ColorMoods.Failure},
            {Color.DarkKhaki,       ColorMoods.Neutral},
            {Color.Turquoise,       ColorMoods.Success},
            {Color.Coral,           ColorMoods.Failure},
            {Color.DeepSkyBlue,     ColorMoods.Success},
            {Color.Peru,            ColorMoods.Neutral},
            {Color.Goldenrod,       ColorMoods.Failure},
            {Color.SpringGreen,     ColorMoods.Success},
            {Color.IndianRed,       ColorMoods.Failure},
            {Color.MediumSlateBlue, ColorMoods.Neutral},
            {Color.SkyBlue,         ColorMoods.Success},
            {Color.Sienna,          ColorMoods.Neutral},
            {Color.DarkOrange,      ColorMoods.Failure},
            {Color.LimeGreen,       ColorMoods.Success},
            {Color.DarkSlateBlue,   ColorMoods.Neutral},
            {Color.Tomato,          ColorMoods.Failure},
            {Color.OliveDrab,       ColorMoods.Success},
            {Color.CadetBlue,       ColorMoods.Neutral},
            {Color.Crimson,         ColorMoods.Failure},
            {Color.MediumSeaGreen,  ColorMoods.Success},
            {Color.DeepPink,        ColorMoods.Failure},
            {Color.SteelBlue,       ColorMoods.Neutral},
            {Color.Chartreuse,      ColorMoods.Success}
        };
        #endregion

        private Collection<Color> _usedColors = new Collection<Color>();

        /// <summary>
        /// Selects the color with the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static Color Select(int offset)
        {
            return _colors.Keys.ElementAt(offset % _colors.Count);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            _usedColors.Clear();
        }

        /// <summary>
        /// Gets the next color.
        /// </summary>
        /// <returns></returns>
        public Color NextColor()
        {
            return NextColor(ColorMoods.All);
        }

        /// <summary>
        /// Gets the next color.
        /// </summary>
        /// <param name="preferredMood">The preferred mood.</param>
        /// <returns></returns>
        public Color NextColor(ColorMoods preferredMood)
        {
            return NextColor(preferredMood, ColorMoods.All);
        }

        /// <summary>
        /// Gets the next color.
        /// </summary>
        /// <param name="preferredMood">The preferred mood.</param>
        /// <param name="secondaryMood">The secondary mood.</param>
        /// <returns></returns>
        public Color NextColor(ColorMoods preferredMood, ColorMoods secondaryMood)
        {
            if (_usedColors.Count == _colors.Count)
            {
                // We're all out of colors.  Reset.
                Reset();
            }

            // First try to find a color in the preferred mood
            Color result = FindUnused(preferredMood);
            if (result != Color.Transparent)
            {
                return result;
            }
            else
            {
                // Try the secondary mood
                Color secondresult = FindUnused(secondaryMood);
                if (secondresult != Color.Transparent)
                {
                    return secondresult;
                }
                else
                {
                    // Just return something
                    return FindUnused(ColorMoods.All);
                }
            }
        }

        private Color FindUnused(ColorMoods mood)
        {
            foreach (var pair in _colors)
            {
                // Use bitwise AND to see if this color passes - also make sure we haven't used it yet
                if ((pair.Value & mood) != 0 && !_usedColors.Contains(pair.Key))
                {
                    _usedColors.Add(pair.Key);
                    return pair.Key;
                }
            }

            // If we get to this point, we have used all the colors
            return Color.Transparent;
        }
    }
}
