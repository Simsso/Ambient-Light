using System;
using System.Windows;
using System.Windows.Controls;
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
            refreshPortList();
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

        internal void UpdateDebuggingInformation(long executionTime, double transformFunction)
        {
            Label_ExecutionTime.Content = (executionTime / 10 / 1000).ToString() + " ms";
            Label_TransformFunctionOutput.Content = Math.Round(transformFunction, 4);
        }

        private void Window_Main_Deactivated(object sender, System.EventArgs e)
        {
            if (CheckBox_StayOnTop.IsChecked == true)
            {
                Window window = (Window)sender;
                window.Topmost = true;
            }
        }

        private void CheckBox_PreventFlickering_Changed(object sender, RoutedEventArgs e)
        {
            if (CheckBox_PreventFlickering.IsChecked == null || control == null)
            {
                return;
            }
            control.setPreventFlickering((bool)CheckBox_PreventFlickering.IsChecked);
        }

        private void ComboBox_UpdateSpeed_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (control == null)
            {
                return;
            }

            switch (ComboBox_UpdateSpeed.SelectedIndex)
            {
                case 0:
                    control.setUpdatespeed(UpdateSpeed.Low);
                    break;
                case 2:
                    control.setUpdatespeed(UpdateSpeed.Maximum);
                    break;
                default:
                    control.setUpdatespeed(UpdateSpeed.Normal);
                    break;
            }
        }

        internal void ShowError(Exception exception)
        {
            if (exception == null)
            {
                Label_ErrorOutputTitle.Content = "";
                Label_ErrorOutputDetails.Content = "";
            }
            else
            {
                Label_ErrorOutputTitle.Content = exception.GetType();
                Label_ErrorOutputDetails.Content = exception.Message;
            }
        }

        private void refreshPortList()
        {
            ComboBox_SelectCOMPort.Items.Clear();
            ComboBox_SelectCOMPort.SelectedIndex = -1;
            foreach (string portName in SerialCommunication.GetPortNames())
            {
                ComboBox_SelectCOMPort.Items.Add(portName);
            }
        }

        private void Button_RefreshCOMPortList_Click(object sender, RoutedEventArgs e)
        {
            refreshPortList();
        }

        private void ComboBox_SelectCOMPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_SelectCOMPort.SelectedIndex != -1)
            {
               SerialCommunication.SetSelectedPortName((string)ComboBox_SelectCOMPort.SelectedValue);
            }
        }
    }
}
