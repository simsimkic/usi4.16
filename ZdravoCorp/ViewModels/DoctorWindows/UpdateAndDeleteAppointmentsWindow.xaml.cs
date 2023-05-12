using System;
using System.Windows;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class UpdateAndDeleteAppointmentsWindow : Window
    {

        public Doctor Doctor { get; private set; }

        public UpdateAndDeleteAppointmentsWindow(Doctor doctor)
        {
            InitializeComponent();
            Doctor = doctor;

            Schedule.Instance.Appointments.ForEach(appointment =>
            {
                if (appointment.Doctor.Equals(Doctor) &&
                    appointment.TimeSlot.StartTime > DateTime.Now &&
                    appointment.Finished == false)
                {
                    AppointmentsListView.Items.Add(appointment);
                }
            });
        }

        private void UpdateAppointment(object sender, RoutedEventArgs e)
        {
            Appointment appointment = (Appointment) AppointmentsListView.SelectedItem;

            if (AppointmentsListView.SelectedItem == null)
            {
                MessageBox.Show("Izaberite termin!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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

            TimeSpan duration;
            if (!appointment.IsOperation) duration = TimeSpan.FromMinutes(15);
            else
            {
                int[] operationDuration = CustomDatePicker.ExtractTime(OperationDuration.Text);
                if (operationDuration.Length == 0) return;
                int hours = operationDuration[0];
                int minutes = operationDuration[1];
                duration = TimeSpan.FromHours(hours).Add(TimeSpan.FromMinutes(minutes));
            }

            TimeSlot newAppointmentTimeSlot = new TimeSlot(date, duration);

            appointment = new Appointment(newAppointmentTimeSlot, Doctor, appointment.Patient, appointment.IsOperation);

            Schedule.Instance.Appointments.Remove((Appointment) AppointmentsListView.SelectedItem);
            if (Schedule.Instance.ScheduleAppointment(appointment))
            {
                Schedule.Instance.Appointments.Add((Appointment) AppointmentsListView.SelectedItem);
                Schedule.Instance.DeleteAppointment((Appointment) AppointmentsListView.SelectedItem);
                MessageBox.Show("Uspešno promenjen termin!", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }

            Schedule.Instance.Appointments.Add((Appointment) AppointmentsListView.SelectedItem);
            MessageBox.Show("Termin je zauzet!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DeleteAppointment(object sender, RoutedEventArgs e)
        {
            if (AppointmentsListView.SelectedItem == null)
            {
                MessageBox.Show("Izaberite termin!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Schedule.Instance.DeleteAppointment((Appointment) AppointmentsListView.SelectedItem);
            MessageBox.Show("Uspešno obrisan termin!", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

    }
}
