using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AmbientLight
{
    class AmbientLightControl
    {
        private volatile BasicColor color = new BasicColor(),
            outputColor = new BasicColor();
        private volatile Exception error = null;

        private double saturation = 0.5, 
            brightness = 1,
            transferFunctionFactor = 0.0; // will be set automatically
        private long startTicks = 0, executionTime = 0;
        private bool preventFlickering = true;
        private UpdateSpeed updateSpeed = UpdateSpeed.Normal;

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
                control.outputColor = ColorManipulation.IncreaseSaturation(color, factor).Multiply(brightness);

                try
                {
                    SerialCommunication.SendColor(control.outputColor); // update hardware
                    this.error = null;
                }
                catch (Exception e)
                {
                    this.error = e;
                }

                this.executionTime = DateTime.Now.Ticks - this.startTicks; // stop time measurement

                this.updateUI(); // update ui

                switch (this.updateSpeed)
                {
                    case UpdateSpeed.Low:
                        Thread.Sleep(400); // keep Windows very responsive
                        break;
                    case UpdateSpeed.Normal:
                        Thread.Sleep(100);
                        break;
                    case UpdateSpeed.Maximum:
                        // continue as quick as possible
                        break;
                    default:
                        Thread.Sleep(100);
                        break;
                }
            }
        }

        private void updateUI()
        {
            try
            {
                ui.Window_Main.Dispatcher.Invoke(new Action(() =>
                {
                    // running on UI thread
                    ui.UpdateColors(this.color, this.outputColor);
                    ui.UpdateDebuggingInformation(this.executionTime, transferFunctionFactor);
                    ui.ShowError(this.error);
                }));
            }
            catch (TaskCanceledException e)
            {
            }
        }

        internal void SetSaturation(double value)
        {
            this.saturation = value;
        }

        internal void SetBrightness(double value)
        {
            this.brightness = value;
        }

        private static double TransferFunction(double delta)
        {
            return 1 / (1 + Math.Pow(Math.E, -((delta - 0.01) * 500)));
        }

        internal void setPreventFlickering(bool preventFlickering)
        {
            this.preventFlickering = preventFlickering;
        }

        internal void setUpdatespeed(UpdateSpeed newSpeed)
        {
            this.updateSpeed = newSpeed;
        }
    }

    enum UpdateSpeed
    {
        Low,
        Normal,
        Maximum
    }
}
