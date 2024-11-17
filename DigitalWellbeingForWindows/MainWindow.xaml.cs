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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Threading;



namespace DigitalWellbeingForWindows
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId( IntPtr hWnd, out uint pdwProcessId);
        public MainWindow()
        {
            InitializeComponent();
            //MostrarProcesosActivos();
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2),
            };
            timer.Tick += (s,e) => ObtenerProcesoEnPrimerPlano();
            timer.Start();


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

        private void ObtenerProcesoEnPrimerPlano()
        {
            try
            {
                // Obtiene el identificador de la ventana activa
                IntPtr hwnd = GetForegroundWindow();

                // Obtiene el ID del proceso asociado
                GetWindowThreadProcessId(hwnd, out uint processId);

                // Obtiene el proceso por ID
                Process proceso = Process.GetProcessById((int)processId);

                // Muestra el nombre del proceso en un MessageBox
                txtProcesoActivo.Text = $"Proceso en primer plano: {proceso.ProcessName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el proceso en primer plano: {ex.Message}");
            }
        }
    }
}
