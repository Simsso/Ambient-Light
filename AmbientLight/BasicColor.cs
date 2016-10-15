namespace AmbientLight
{
    class BasicColor
    {
        public byte R = 0, G = 0, B = 0;

        public BasicColor()
        {

        }

        public BasicColor(byte r, byte g, byte b)
        {
            this.Set(r, g, b);
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
    }
}
