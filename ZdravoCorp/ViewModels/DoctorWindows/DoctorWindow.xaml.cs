using System;
using System.Windows;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class DoctorWindow : Window
    {
        public Doctor Doctor { get; private set; }

        public DoctorWindow(Doctor doctor)
        {
            InitializeComponent();
            Doctor = doctor;
            UpdateList();
            if (Doctor.Notification != "")
            {
                MessageBox.Show(Doctor.Notification);
                Doctor.Notification = "";
                UserRepository.Instance.SaveDoctors();
            }
        }

        private void ScheduleExamination(object sender, RoutedEventArgs e)
        {
            ScheduleAppointmentWindow window = new ScheduleAppointmentWindow(Doctor);
            window.Show();
        }

        private void ScheduledAppointments(object sender, RoutedEventArgs e)
        {
            ScheduledAppointmentsWindow window = new ScheduledAppointmentsWindow(Doctor);
            window.Show();
        }

        private void UpdateAndDeleteAppointments(object sender, RoutedEventArgs e)
        {
            UpdateAndDeleteAppointmentsWindow window = new UpdateAndDeleteAppointmentsWindow(Doctor);
            window.Show();
        }

        private void SearchPatients(object sender, RoutedEventArgs e)
        {
            SearchPatientsWindow window = new SearchPatientsWindow(Doctor);
            window.Show();
        }

        private void UpdateListClick(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            AppointmentsListView.Items.Clear();
            TimeSlot overlap = new TimeSlot(DateTime.Now - TimeSpan.FromMinutes(10), TimeSpan.FromDays(1));
            foreach (Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.IsOperation == false &&
                    appointment.Finished == false &&
                    appointment.Doctor.Equals(Doctor) &&
                    appointment.TimeSlot.IsOverlapping(overlap)
                    )
                {
                    AppointmentsListView.Items.Add(appointment);
                }
            }
        }

        private void StartAppointment(object sender, RoutedEventArgs e)
        {
            if (AppointmentsListView.SelectedItem == null)
            {
                MessageBox.Show("Izaberite pregled!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Appointment appointment = (Appointment) AppointmentsListView.SelectedItem;

            if (!(appointment.TimeSlot.IsOverlapping(new TimeSlot(DateTime.Now, TimeSpan.FromMinutes(15))) &&
                appointment.TimeSlot.StartTime <= DateTime.Now + TimeSpan.FromMinutes(10)))
            {
                MessageBox.Show("Ne možete da započnete ovaj pregled! Sačekajte njegov termin!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Room room;
            DoctorsRoomWindow roomWindow = new DoctorsRoomWindow();
            if (roomWindow.ShowDialog() == true)
            {
                room = roomWindow.Room;
                if (room.Name == "")
                {
                    MessageBox.Show("Morate izabrati sobu u kojoj se pregled izvršava!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati sobu u kojoj se pregled izvršava!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StartedAppointmentWindow window = new StartedAppointmentWindow(appointment, room);
            window.ShowDialog();

            UpdateList();
        }
    }
}
