using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZdravoCorp.ViewModels.PatientWindows
{
    /// <summary>
    /// Interaction logic for ConditionalcheduleAppointmentWindow.xaml
    /// </summary>
    public partial class ConditionalcheduleAppointmentWindow : Window
    {
        public Patient Patient { get; set; }

        ConditionalScheduleAppointmentWindowViewModel smartScheduleAppointmentWindowViewModel { get; set; }

        public ConditionalcheduleAppointmentWindow(Patient patient)
        {
            InitializeComponent();
            this.Patient = patient;
            smartScheduleAppointmentWindowViewModel= new ConditionalScheduleAppointmentWindowViewModel(Patient);
            this.DataContext = smartScheduleAppointmentWindowViewModel;
        }
        private void ScheduleOptimalAppointment(object sender, RoutedEventArgs e)
        {
            smartScheduleAppointmentWindowViewModel.ScheduleAppointmentConditionally(sender, e);
        }

        private void AppointmentConfirmation(object sender, RoutedEventArgs e)
        {
            smartScheduleAppointmentWindowViewModel.ScheduleConfirmation(sender, e);
            this.Close();
        }

        private void returnButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
