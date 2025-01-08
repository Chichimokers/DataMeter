using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Data_Counter
{
    public partial class Datametera : Form
    {
        private Timer timer;
        private NetworkInterface selectedNetworkInterface;
        private long initialBytesSent;
        private long initialBytesReceived;

        public Datametera()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 1000; // Actualizar cada 1 segundo
            timer.Tick += Timer_Tick;
        }

        public string[] GetAllNetworkInterfaceNames()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            string[] interfaceNames = new string[interfaces.Length];

            for (int i = 0; i < interfaces.Length; i++)
            {
                interfaceNames[i] = interfaces[i].Name;
            }

            return interfaceNames;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] networkInterfaces = GetAllNetworkInterfaceNames();

            if (networkInterfaces.Length > 0)
            {
                comboBox1.Items.AddRange(networkInterfaces);
                comboBox1.SelectedIndex = 0; // Selecciona el primer adaptador por defecto
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedInterfaceName = comboBox1.SelectedItem.ToString();
            selectedNetworkInterface = GetNetworkInterfaceByName(selectedInterfaceName);

            if (selectedNetworkInterface != null)
            {
                // Inicializa los valores de tráfico
                IPv4InterfaceStatistics stats = selectedNetworkInterface.GetIPv4Statistics();
                initialBytesSent = stats.BytesSent;
                initialBytesReceived = stats.BytesReceived;

                // Reinicia el contador y arranca el temporizador
                t.Text = "0 MB";
                timer.Start();
            }
            else
            {
                timer.Stop();
                t.Text = "No se puede monitorear.";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (selectedNetworkInterface != null)
            {
                IPv4InterfaceStatistics stats = selectedNetworkInterface.GetIPv4Statistics();

                // Calcula el tráfico actual desde que se inició el programa
                long currentBytesSent = stats.BytesSent - initialBytesSent;
                long currentBytesReceived = stats.BytesReceived - initialBytesReceived;
                long totalBytes = currentBytesSent + currentBytesReceived;

                // Convierte a MB y actualiza el label
                double totalMb = totalBytes / 1024.0 / 1024.0;
                t.Text = $"{totalMb:F2} MB";
            }
        }

        private NetworkInterface GetNetworkInterfaceByName(string name)
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.Name == name)
                {
                    return ni;
                }
            }
            return null;
        }
    }
}
