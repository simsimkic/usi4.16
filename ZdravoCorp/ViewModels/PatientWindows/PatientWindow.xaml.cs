using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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
using System.Windows.Shapes;

namespace ZdravoCorp.ViewModels.PatientWindows
{
    /// <summary>
    /// Basic code for Patient window
    /// </summary>
    public partial class PatientWindow : Window
    {
        public Patient Patient { get; set; }
        PatientWindowViewModel PatientWindowViewModel { get; set; }
        public PatientWindow(Patient patient)
        {
            InitializeComponent();
            this.Patient = patient;
            PatientWindowViewModel = new PatientWindowViewModel(patient);
            this.DataContext = PatientWindowViewModel;
        }

        private void ScheduleAppointment(object sender, RoutedEventArgs e)
        {
            scheduleAppointmentWindow addAppointmentWindow = new scheduleAppointmentWindow(Patient);
            addAppointmentWindow.Show();
        }
        private void editAppointment(object sender, RoutedEventArgs e)
        {
            PatientWindowViewModel.editAppointment(sender, e);
        }

        private void deleteAppointment(object sender, RoutedEventArgs e)
        {
           PatientWindowViewModel.DeleteAppointment(sender, e);
        }

        private void SmartScheduleAppointment(object sender, RoutedEventArgs e)
        {

            ConditionalcheduleAppointmentWindow smartScheduleAppointment = new ConditionalcheduleAppointmentWindow(Patient);
            smartScheduleAppointment.Show();
        }

        private void ViewMedicalRecord(object sender, RoutedEventArgs e)
        {
            PatientMedicalRecordWindow patientMedicalRecordWindow = new PatientMedicalRecordWindow(Patient);
            patientMedicalRecordWindow.Show();
        }
    }
}
