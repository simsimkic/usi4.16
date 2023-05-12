using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZdravoCorp.ViewModels.NurseWindows
{
    /// <summary>
    /// Interaction logic for UrgentExaminationWindow.xaml
    /// </summary>
    public partial class UrgentExaminationWindow : Window
    {
        Patient patient;
        bool isOperation;
        int specialization;
        bool urgentAppointmentAvailable;
        ObservableCollection<Appointment> currentAppointments;
        public UrgentExaminationWindow(Patient patient)
        {
            InitializeComponent();
            List<string> specializations = new List<string>();

            foreach (string i in Enum.GetNames(typeof(Specialization)))
            {
                specializations.Add(i);
            }

            SpecializationComboBox.ItemsSource = specializations;
            SpecializationComboBox.SelectedIndex = 0;
            this.patient = patient;
            isOperation = false;
            specialization = 0;
            urgentAppointmentAvailable = true;
        }

        private void ScheduleUrgentAppointment_Click(object sender, RoutedEventArgs e)
        {
            specialization = SpecializationComboBox.SelectedIndex;

            if (!int.TryParse(durationTextBox.Text, out int durationMinutes))
            {
                MessageBox.Show("Unesite validnu dužinu termina");
                return;
            }

            TimeSpan duration = TimeSpan.FromMinutes(durationMinutes);

            if (urgentAppointmentAvailable)
            {
                if (Schedule.Instance.ScheduleUrgentAppointment(patient, (Specialization)specialization, duration, isOperation))
                {
                    MessageBox.Show("Uspesno zakazan hitan termin");
                    Close();
                }
                else
                {
                    MessageBox.Show("Hitan termin nije zakazan");
                    urgentAppointmentAvailable = false;
                    currentAppointments = new ObservableCollection<Appointment>(Schedule.Instance.GetCurrentAppointmentsBy((Specialization)specialization));
                    CurrentAppointmentsTable.ItemsSource = currentAppointments;
                    CurrentAppointmentsTable.IsEnabled = true;
                }
            }
            else
            {
                Appointment selectedAppointment = (Appointment)CurrentAppointmentsTable.SelectedItem;

                if (selectedAppointment == null)
                {
                    MessageBox.Show("Niste izabrali termin");
                    return;
                }

                TimeSlot newTimeSlot = new TimeSlot(selectedAppointment.TimeSlot.StartTime, duration);
                Schedule.Instance.PostponeAppointment(selectedAppointment);
                Appointment newAppointment = new Appointment(newTimeSlot, selectedAppointment.Doctor, patient, isOperation);
                Schedule.Instance.ScheduleAppointment(newAppointment);
                MessageBox.Show("Uspesno zakazan hitan termin");

                Patient selectedPatient = UserRepository.Instance.GetPatientBy(selectedAppointment.Patient.Name, selectedAppointment.Patient.Surname);
                Doctor selectedDoctor = UserRepository.Instance.GetDoctorBy(selectedAppointment.Doctor.Name, selectedAppointment.Doctor.Surname);

                selectedDoctor.Notification = $"Termin u {newAppointment.TimeSlot.StartTime} je odložen na {selectedAppointment.TimeSlot.StartTime}";
                selectedPatient.Notification = $"Termin u {newAppointment.TimeSlot.StartTime} je odložen na {selectedAppointment.TimeSlot.StartTime}";

                UserRepository.Instance.SavePatients();
                UserRepository.Instance.SaveDoctors();
                Close();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if(operationRB.IsChecked == true) {
                durationTextBox.IsEnabled = true;
                isOperation = true;
            } else
            {
                durationTextBox.Text = "15";
                durationTextBox.IsEnabled = false;
                isOperation = false;
            }
        }

    }
}
