using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;

namespace DigitalWellbeingForWindows
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MostrarProcesosActivos();
        }


        private void MostrarProcesosActivos()
        {
            try
            {
                ManagementScope scope = new ManagementScope(@"\\.\root\cimv2");
                scope.Connect();

                ObjectQuery query = new ObjectQuery("SELECT Name FROM Win32_Process");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                StringBuilder procesos = new StringBuilder();
                foreach (ManagementObject proceso in searcher.Get())
                {
                    procesos.AppendLine(proceso["Name"].ToString());
                }

                // Muestra la lista de procesos en un control TextBlock
                txtProcesos.Text = procesos.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener procesos: {ex.Message}");
            }
        }
    }
}
