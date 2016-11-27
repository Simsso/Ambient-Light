using System;

namespace AmbientLight
{
    internal class BasicColor
    {
        public byte R = 0, G = 0, B = 0;

        public BasicColor()
        {

        }

        public BasicColor(byte r, byte g, byte b)
        {
            this.Set(r, g, b);
        }

        public BasicColor(BasicColor copyFrom) {
            this.Set(copyFrom.R, copyFrom.G, copyFrom.B);
        }

        public void Set(BasicColor color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public void Set(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public byte[] GetByteArray()
        {
            return new byte[] { this.R, this.G, this.B };
        }

        public byte GetMaxRGB()
        {
            return Math.Max(this.R, Math.Max(this.G, this.B));
        }

        public byte GetMinRGB()
        {
            return Math.Min(this.R, Math.Min(this.G, this.B));
        }

        public BasicColor Map(byte inMin, byte inMax, byte outMin, byte outMax)
        {
            this.R = this.R.Map(inMin, inMax, outMin, outMax);
            this.G = this.G.Map(inMin, inMax, outMin, outMax);
            this.B = this.B.Map(inMin, inMax, outMin, outMax);
            return this;
        }

        public BasicColor Copy()
        {
            return new BasicColor(this);
        }

        public bool IsGray()
        {
            return (this.R == this.G && this.G == this.B);
        }

        public override string ToString()
        {
            return "(" + this.R.ToString() + "," + this.G.ToString() + "," + this.B.ToString() + ")";
        }

        public double GetDelta()
        {
            return (double)(GetMaxRGB() - GetMinRGB()) / (double)byte.MaxValue;
        }

        public BasicColor Multiply(double factor)
        {
            return this.Map(0, byte.MaxValue, 0, (byte)((double)byte.MaxValue * factor));
        }
    }
}
