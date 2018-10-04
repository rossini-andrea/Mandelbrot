/*
    Mandelbrot.exe - A Mandelbrot Set generator.
    Copyright (C) 2017 Andrea Rossini

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot
{
    static class PaletteGenerator
    {
        private static Dictionary<string, Func<int[]>> m_palettes = new Dictionary<string, Func<int[]>>
        {
            { "Default" , LoadDefaultPalette },
            { "BloodRed" , LoadBloodPalette },
            { "DeepBlue", LoadDeepBlue },
            { "BlueNeon", LoadBlueNeon }
        };

        public static IReadOnlyDictionary<string, Func<int[]>> Palettes
        {
            get => m_palettes;
        }

        private static int[] LoadDeepBlue()
        {
            var result = new int[65536];

            for (var i = 0; i < 65536; ++i)
            {
                var a = (i * Math.PI) / 131072;
                var rg = (int)((1 - Math.Cos(a)) * 255);
                var b = (int)(Math.Sin(a) * 255);
                result[i] = b | (rg << 8) | (rg << 16);
            }

            result[65535] = 0;

            return result;
        }
        private static int[] LoadBlueNeon()
        {
            var result = new int[65536];

            for (var i = 0; i < 65536; ++i)
            {
                var a = (i * Math.PI) / 131072;
                var r = (int)((1 - Math.Cos(a)) * 255);
                var g = (int)((Math.Sin(a * 2 - Math.PI / 2) / 2 + 0.5) * 255);
                var b = (int)(Math.Sin(a) * 255);
                result[i] = b | (g << 8) | (r << 16);
            }

            result[65535] = 0;

            return result;
        }

        private static int[] LoadBloodPalette()
        {
            var result = new int[128 * 5];

            for (var i = 0; i < 128 * 5; ++i)
            {
                result[i] = (128 - (i & 0x0000007f)) << 16;
            }

            return result;
        }

        private static int[] LoadDefaultPalette()
        {
            var result = new int[1536];

            for (var i = 0; i < 256; ++i)
            {
                result[i] = 255 | (i << 8) | (i << 16);
            }

            for (var i = 0; i < 256; ++i)
            {
                result[i + 256] = (255 - i) | ((255 - i) << 8) | (0 << 16);//0 | ((255 - i) << 8) | ((255 - i) << 16);
            }

            for (var i = 0; i < 256; ++i)
            {
                result[i + 512] = 255 | (i << 8) | (i << 16);
            }

            for (var i = 0; i < 256; ++i)
            {
                result[i + 768] = (255 - i) | ((255 - i) << 8) | (0 << 16);//0 | ((255 - i) << 8) | ((255 - i) << 16);
            }

            for (var i = 0; i < 256; ++i)
            {
                result[i + 1024] = 255 | (i << 8) | (i << 16);
            }

            for (var i = 0; i < 256; ++i)
            {
                result[i + 1280] = (255 - i) | ((255 - i) << 8) | (0 << 16);//0 | ((255 - i) << 8) | ((255 - i) << 16);
            }

            return result;
        }
    }
}
