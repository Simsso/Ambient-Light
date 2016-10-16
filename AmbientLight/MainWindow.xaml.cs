using System.Windows;
using System.Windows.Media;

namespace AmbientLight
{
    public partial class MainWindow : Window
    {
        private volatile AmbientLightControl control;

        public MainWindow()
        {
            InitializeComponent();
            control = new AmbientLightControl(this);
        }

        internal void UpdateColors(BasicColor screen, BasicColor output)
        {
            Canvas_AverageScreenColor.Background = new SolidColorBrush(Color.FromArgb(255, screen.R, screen.G, screen.B));
            Canvas_AmbientLightColor.Background = new SolidColorBrush(Color.FromArgb(255, output.R, output.G, output.B));
            Label_AverageScreenColorRGB.Content = screen.ToString();
            Label_AmbientLightColorRGB.Content = output.ToString();
        }

        private void Slider_Saturation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (control == null)
            {
                return;
            }
            Label_SaturationValue.Content = Slider_Saturation.Value.ToString();
            control.SetSaturation(Slider_Saturation.Value);
        }

        internal void UpdateDebuggingInformation(long executionTime)
        {
            Label_ExecutionTime.Content = (executionTime / 10 / 1000).ToString() + " ms";
        }
    }
}
