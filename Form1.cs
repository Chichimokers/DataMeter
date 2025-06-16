using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Data_Counter
{
    public partial class Datametera : Form
    {
        private Timer timer;

        private Dictionary<string, long> previousTotals = new Dictionary<string, long>();
        private Dictionary<string, double> accumulatedMb = new Dictionary<string, double>();

        // Para controlar las alertas y que no se repitan para el mismo umbral
        private Dictionary<string, int> lastAlertThreshold = new Dictionary<string, int>();

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
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            if (interfaces.Length > 0)
            {
                int y = 10;

                foreach (var ni in interfaces.OrderBy(i => i.Name))
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.Font = new Font("Segoe UI", 15, FontStyle.Regular);
                    label.Text = ni.Name + " -> 0.00 MB";
                    label.Location = new Point(10, y);
                    this.Controls.Add(label);
                    y += 30;

                    // Inicializar alertas
                    lastAlertThreshold[ni.Name] = 0;
                }
            }
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Inicializar previousTotals y accumulatedMb si faltan
            foreach (var na in interfaces)
            {
                if (!previousTotals.ContainsKey(na.Name))
                {
                    IPv4InterfaceStatistics stats = na.GetIPv4Statistics();
                    previousTotals[na.Name] = stats.BytesSent + stats.BytesReceived;
                    accumulatedMb[na.Name] = 0;
                    lastAlertThreshold[na.Name] = 0;
                }
            }

            foreach (var ni in interfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up && previousTotals.ContainsKey(ni.Name))
                {
                    IPv4InterfaceStatistics stats = ni.GetIPv4Statistics();
                    long currentTotal = stats.BytesSent + stats.BytesReceived;
                    long previousTotal = previousTotals[ni.Name];

                    long deltaBytes = currentTotal - previousTotal;
                    if (deltaBytes < 0)
                    {
                        deltaBytes = 0; // Reinicio contador
                    }

                    previousTotals[ni.Name] = currentTotal;
                    accumulatedMb[ni.Name] += deltaBytes / 1024.0 / 1024.0;

                    // Actualiza label
                    foreach (Control control in this.Controls)
                    {
                        if (control is Label label && label.Text.StartsWith(ni.Name))
                        {
                            label.Text = $"{ni.Name} -> {accumulatedMb[ni.Name]:F2} MB";
                            break;
                        }
                    }

                    // Verificar si llegó a nuevo múltiplo de 100 MB
                    int currentThreshold = (int)(accumulatedMb[ni.Name] / 100);
                    if (currentThreshold > lastAlertThreshold[ni.Name])
                    {
                        lastAlertThreshold[ni.Name] = currentThreshold;
                        ShowAlert($"{ni.Name} ha consumido {currentThreshold * 100} MB de datos.");
                    }
                }
            }
        }

        // Método para mostrar alerta TopMost
        private void ShowAlert(string message)
        {
            Form alertForm = new Form()
            {
                Size = new Size(400, 150),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Alerta de Consumo",
                TopMost = true
            };

            Label msgLabel = new Label()
            {
                Text = message,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            alertForm.Controls.Add(msgLabel);

            // Botón para cerrar alerta
            Button closeBtn = new Button()
            {
                Text = "Cerrar",
                Dock = DockStyle.Bottom,
                Height = 35
            };
            closeBtn.Click += (s, e) => alertForm.Close();

            alertForm.Controls.Add(closeBtn);

            alertForm.Show();
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
