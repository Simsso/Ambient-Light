using System.IO.Ports;

namespace AmbientLight
{
    class SerialCommunication
    {
        private static SerialPort arduinoPort;

        static SerialCommunication()
        {
            // init serial port communication
            arduinoPort = new SerialPort("COM3", 9600);
        }

        public static void SendColor(BasicColor color) {
            arduinoPort.Open();
            arduinoPort.Write(color.GetByteArray(), 0, 3);
            arduinoPort.Close();
        }
    }
}
