using System;
using System.Threading;

namespace AmbientLight
{
    class AmbientLightControl
    {
        private volatile BasicColor color = new BasicColor(),
            outputColor = new BasicColor();
        private double saturation = 0.5, 
            transferFunctionFactor = 0.0; // will be set automatically
        private long startTicks = 0, executionTime = 0;
        private bool preventFlickering = true;

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

                this.startTicks = DateTime.Now.Ticks; // measure time
                color.Set(ScreenColor.GetAverageScreenColor(ScreenColor.DeterminationMethod.Partly)); // read screen color

                double factor = saturation;
                transferFunctionFactor = TransferFunction(color.GetDelta());
                if (preventFlickering)
                {
                    factor *= transferFunctionFactor;
                }
                control.outputColor = ColorManipulation.IncreaseSaturation(color, factor);

                SerialCommunication.SendColor(control.outputColor); // update hardware
                this.executionTime = DateTime.Now.Ticks - this.startTicks; // stop time measurement

                this.updateUI(); // update ui

                Thread.Sleep(100); // keep Windows responsive
            }
        }

        private void updateUI()
        {
            ui.Window_Main.Dispatcher.Invoke(new Action(() =>
            {
                // running on UI thread
                ui.UpdateColors(this.color, this.outputColor);
                ui.UpdateDebuggingInformation(this.executionTime, transferFunctionFactor);
            }));
        }

        internal void SetSaturation(double value)
        {
            this.saturation = value;
        }

        private static double TransferFunction(double delta)
        {
            return 1 / (1 + Math.Pow(Math.E, -((delta - 0.01) * 500)));
        }

        internal void setPreventFlickering(bool preventFlickering)
        {
            this.preventFlickering = preventFlickering;
        }
    }
}
