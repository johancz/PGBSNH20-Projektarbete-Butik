﻿////////////////////////// [START] REMOVE BEFORE "RELEASE"
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace Butik_User
{
    static class TEMPORARY_AND_PLACEHOLDER_STUFF
    {
        ////////////////////////// [START] REMOVE BEFORE "RELEASE"
        public static List<Product> CreatePlaceHolderProducts()
        {
            List<Product> products = new List<Product>();

            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });
            products.Add(new Product
            {
                Name = "Broccoli",
                Description = "Green thing, do not boil.",
                Price = 2.5,
                ImageUri = new Uri("/Images/broccoli-1238250_640.jpg", UriKind.Relative)
            });

            return products;
        }
        ////////////////////////// [END] REMOVE BEFORE "RELEASE"

        ////////////////////////// [START] REMOVE BEFORE "RELEASE"
        public static Color? GetChromeColor()
        {
            bool isEnabled;
            var hr1 = DwmIsCompositionEnabled(out isEnabled);
            if ((hr1 != 0) || !isEnabled) // 0 means S_OK.
                return null;

            DWMCOLORIZATIONPARAMS parameters;
            try
            {
                // This API is undocumented and so may become unusable in future versions of OSes.
                var hr2 = DwmGetColorizationParameters(out parameters);
                if (hr2 != 0) // 0 means S_OK.
                    return null;
            }
            catch
            {
                return null;
            }

            // Convert colorization color parameter to Color ignoring alpha channel.
            var targetColor = Color.FromRgb(
                (byte)(parameters.colorizationColor >> 16),
                (byte)(parameters.colorizationColor >> 8),
                (byte)parameters.colorizationColor);

            // Prepare base gray color.
            var baseColor = Color.FromRgb(217, 217, 217);

            // Blend the two colors using colorization color balance parameter.
            return BlendColor(targetColor, baseColor, (double)(100 - parameters.colorizationColorBalance));
        }

        private static Color BlendColor(Color color1, Color color2, double color2Perc)
        {
            if ((color2Perc < 0) || (100 < color2Perc))
                throw new ArgumentOutOfRangeException("color2Perc");

            return Color.FromRgb(
                BlendColorChannel(color1.R, color2.R, color2Perc),
                BlendColorChannel(color1.G, color2.G, color2Perc),
                BlendColorChannel(color1.B, color2.B, color2Perc));
        }

        private static byte BlendColorChannel(double channel1, double channel2, double channel2Perc)
        {
            var buff = channel1 + (channel2 - channel1) * channel2Perc / 100D;
            return Math.Min((byte)Math.Round(buff), (byte)255);
        }

        [DllImport("Dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled([MarshalAs(UnmanagedType.Bool)] out bool pfEnabled);

        [DllImport("Dwmapi.dll", EntryPoint = "#127")] // Undocumented API
        private static extern int DwmGetColorizationParameters(out DWMCOLORIZATIONPARAMS parameters);

        [StructLayout(LayoutKind.Sequential)]
        private struct DWMCOLORIZATIONPARAMS
        {
            public uint colorizationColor;
            public uint colorizationAfterglow;
            public uint colorizationColorBalance; // Ranging from 0 to 100
            public uint colorizationAfterglowBalance;
            public uint colorizationBlurBalance;
            public uint colorizationGlassReflectionIntensity;
            public uint colorizationOpaqueBlend;
        }
        ////////////////////////// [END] REMOVE BEFORE "RELEASE"
    }
}
////////////////////////// [END] REMOVE BEFORE "RELEASE"
