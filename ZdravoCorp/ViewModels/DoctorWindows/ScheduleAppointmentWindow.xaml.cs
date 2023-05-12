using System;
using System.Windows;
using System.Windows.Controls;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class ScheduleAppointmentWindow : Window
    {
        public Doctor Doctor { get; private set; }

        public ScheduleAppointmentWindow(Doctor doctor)
        {
            InitializeComponent();
            Doctor = doctor;

            UserRepository.Instance.Patients.ForEach(patient => { PatientsListView.Items.Add(patient); });
        }

        private void ScheduleDoctorsAppointment(object sender, RoutedEventArgs e)
        {
            if (PatientsListView.SelectedItem == null)
            {
                MessageBox.Show("Izaberite pacijenta!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AppointmentDate.SelectedDate == null)
            {
                MessageBox.Show("Izaberite datum!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int[] appointmentTime = CustomDatePicker.ExtractTime(AppointmentTime.Text);
            if (appointmentTime.Length == 0) return;

            DateTime date = CustomDatePicker.FormDateTime(appointmentTime, AppointmentDate.SelectedDate.Value.Date);
            if (date <= DateTime.Now)
            {
                MessageBox.Show("Ne možete da zakažete termin u prošlosti!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Patient? patient = FindPatient((Patient) PatientsListView.SelectedItem);
            if (patient == null) return;

            TimeSpan duration;
            bool isOperation = false;
            if (ExaminationRadioButton.IsChecked == true) duration = TimeSpan.FromMinutes(15);
            else
            {
                int[] operationDuration = CustomDatePicker.ExtractTime(OperationDuration.Text);
                if (operationDuration.Length == 0) return;
                int hours = operationDuration[0];
                int minutes = operationDuration[1];
                duration = TimeSpan.FromHours(hours).Add(TimeSpan.FromMinutes(minutes));
                isOperation = true;
            }

            TimeSlot appointmentTimeSlot = new TimeSlot(date, duration);

            if (Schedule.Instance.ScheduleAppointment(new Appointment(appointmentTimeSlot, Doctor, patient, isOperation)))
            {
                MessageBox.Show("Uspešno zakazan termin!", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }

            MessageBox.Show("Ne možete da zakažete termin za uneto vreme!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static Patient? FindPatient(Patient patientToFind)
        {
            Patient? foundPatient = null;
            foreach (Patient patient in UserRepository.Instance.Patients)
            {
                if (patient.Equals(patientToFind))
                {
                    foundPatient = patient;
                }
            }
            return foundPatient;
        }

    }
}
