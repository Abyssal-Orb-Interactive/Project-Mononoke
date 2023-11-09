using System;
using UnityEngine;

namespace Base.Utils
{
    public static class ColorUtil
    {
        public static Color GetColorFromString(string color)
        {
            var red = Hex_to_Dec01(color.Substring(0,2));
            var green = Hex_to_Dec01(color.Substring(2,2));
            var blue = Hex_to_Dec01(color.Substring(4,2));
            var alpha = 1f;
            if (color.Length >= 8)
            {
                alpha = Hex_to_Dec01(color.Substring(6,2));
            }

            return new Color(red, green, blue, alpha);
        }
        
        public static float Hex_to_Dec01(string hex) {
            return Hex_to_Dec(hex)/255f;
        }
        
        public static int Hex_to_Dec(string hex) {
            return Convert.ToInt32(hex, 16);
        }
    }
}