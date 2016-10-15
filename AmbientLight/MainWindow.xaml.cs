using System.Windows;

namespace AmbientLight
{
    public partial class MainWindow : Window
    {
        AmbientLightControl control = new AmbientLightControl();

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
