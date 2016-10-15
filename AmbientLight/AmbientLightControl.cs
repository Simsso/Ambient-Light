using System.Threading;

namespace AmbientLight
{
    class AmbientLightControl
    {
        volatile private BasicColor color = new BasicColor();

        private Thread updateColorThread;

        public AmbientLightControl() {
            this.updateColorThread = new Thread(this.updateColor);
            this.updateColorThread.IsBackground = true;
            this.updateColorThread.Start(this.color);
        }

        private void updateColor(object obj)
        {
            while (true)
            {
                BasicColor color = (BasicColor)obj;
                color.Set(ScreenColor.GetAverageScreenColor(ScreenColor.DeterminationMethod.Partly));
                SerialCommunication.SendColor(color);
                Thread.Sleep(50);
            }
        }
    }
}
