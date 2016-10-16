using System;
using System.Threading;

namespace AmbientLight
{
    class AmbientLightControl
    {
        private volatile BasicColor color = new BasicColor(),
            outputColor = new BasicColor();
        private double saturation = 0.5;
        private long startTicks = 0, executionTime = 0;

        private Thread updateColorThread;

        private MainWindow ui;

        public AmbientLightControl(MainWindow ui) {
            this.ui = ui;

            this.updateColorThread = new Thread(this.updateColor);
            this.updateColorThread.IsBackground = true;
            this.updateColorThread.Start(this);
        }

        private void updateColor(object obj)
        {
            while (true)
            {
                AmbientLightControl control = (AmbientLightControl)obj;
                BasicColor color = control.color;

                this.startTicks = DateTime.Now.Ticks;
                color.Set(ScreenColor.GetAverageScreenColor(ScreenColor.DeterminationMethod.Partly));
                control.outputColor = ColorManipulation.IncreaseSaturation(color, saturation);
                SerialCommunication.SendColor(control.outputColor);
                this.executionTime = DateTime.Now.Ticks - this.startTicks;

                this.updateUI();
                Thread.Sleep(100);
            }
        }

        private void updateUI()
        {
            ui.Window_Main.Dispatcher.Invoke(new Action(() =>
            {
                // running on UI thread
                ui.UpdateColors(this.color, this.outputColor);
                ui.UpdateDebuggingInformation(this.executionTime);
            }));
        }

        public void SetSaturation(double value)
        {
            this.saturation = value;
        }
    }
}
