using System.IO.Ports;

namespace AmbientLight
{
    class SerialCommunication
    {
        private static volatile SerialPort arduinoPort;
        private static int baudRate = 9600;
        private static string defaultPort = "COM3";

        static SerialCommunication()
        {
            // init serial port communication
            arduinoPort = new SerialPort(defaultPort, baudRate);
        }

        public static void SendColor(BasicColor color) {
            arduinoPort.Open();
            arduinoPort.Write(color.GetByteArray(), 0, 3);
            arduinoPort.Close();
        }

        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public static void SetSelectedPortName(string name)
        {
            arduinoPort = new SerialPort(name, baudRate);
        }
    }
}
