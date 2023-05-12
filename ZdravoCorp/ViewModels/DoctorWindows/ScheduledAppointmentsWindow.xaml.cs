using System;
using System.Reflection.Metadata.Ecma335;
using System.Windows;
using System.Windows.Controls;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class ScheduledAppointmentsWindow : Window
    {
        public Doctor Doctor { get; private set; }

        public ScheduledAppointmentsWindow(Doctor doctor)
        {
            InitializeComponent();
            Doctor = doctor;
        }

        private void ShowForNextDays(object sender, RoutedEventArgs e)
        {
            AppointmentsListView.Items.Clear();
            foreach (Appointment appointment in Schedule.Instance.Appointments){
                if (appointment.Doctor.Equals(Doctor) &&
                    appointment.Finished == false &&
                    appointment.TimeSlot.IsOverlapping(new TimeSlot(DateTime.Now, TimeSpan.FromDays(3))))
                {
                    AppointmentsListView.Items.Add(appointment);
                }
            }
        }

        private void ShowForSpecificDay(object sender, RoutedEventArgs e)
        {
            if (SpecificDay.SelectedDate == null)
            {
                MessageBox.Show("Izaberite dan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                SpecificDayRadioButton.IsChecked = false;
                return;
            }
            DateTime day = (DateTime) SpecificDay.SelectedDate;

            AppointmentsListView.Items.Clear();
            foreach(Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.Doctor.Equals(Doctor) &&
                    appointment.TimeSlot.IsOverlapping(new TimeSlot(day, TimeSpan.FromDays(1))))
                {
                    AppointmentsListView.Items.Add(appointment);
                }
            }
        } 

        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            SpecificDayRadioButton.IsChecked = true;
            ShowForSpecificDay(sender, e);
        }

        private void ShowPatientsRecord(object sender, RoutedEventArgs e)
        {
            if(AppointmentsListView.SelectedItem == null)
            {
                MessageBox.Show("Izaberite pregled!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Patient patient = ((Appointment) AppointmentsListView.SelectedItem).Patient;
            patient = GetFullInfoPatient(patient);

            ShowPatientRecordWindow window = new ShowPatientRecordWindow(patient);
            window.Show();
        }

        private Patient GetFullInfoPatient(Patient patientToSet)
        {
            foreach (Patient patient in UserRepository.Instance.Patients)
            {
                if (patient.Equals(patientToSet))
                {
                    return patient;
                }
            }
            return patientToSet;
        }

    }
}
