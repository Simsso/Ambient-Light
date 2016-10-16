using System;

namespace AmbientLight
{
    static class ColorManipulation
    {
        public static BasicColor SetMinBrightness(BasicColor input, double factor)
        {
            if (factor < 0 || factor > 1) {
                throw new ArgumentException();
            }

            byte outMin = (byte)(byte.MaxValue * factor);
            return input.Copy().Map(0, byte.MaxValue, outMin, byte.MaxValue);
        }

        public static BasicColor IncreaseSaturation(BasicColor input, double factor)
        {
            if (factor < 0 || factor > 1)
            {
                throw new ArgumentException();
            }

            if (input.IsGray())
            {
                return input;
            }

            byte inMin = input.GetMinRGB(), inMax = input.GetMaxRGB(), outMin, outMax;

            if (factor == 1)
            {
                outMin = 0;
                outMax = byte.MaxValue;
            }
            else
            {
                outMin = (byte)((double)inMin * (1 - factor));
                outMax = (byte)((double)inMax * (1.0 - factor) + (double)byte.MaxValue * (factor));
            }

            return input.Copy().Map(inMin, inMax, outMin, outMax);
        }
    }
}
