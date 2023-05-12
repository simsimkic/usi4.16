using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class StartedAppointmentWindow : Window
    {
        public Appointment Appointment { get; private set; }
        public Room Room { get; private set; }

        public StartedAppointmentWindow(Appointment appointment, Room room)
        {
            InitializeComponent();
            Appointment = appointment;
            Room = room;
        }
        private void ShowMedicalRecord(object sender, RoutedEventArgs e)
        {
            ShowPatientRecordWindow window = new ShowPatientRecordWindow(Appointment.Patient);
            window.Show();
        }

        private void UpdateMedicalRecord(object sender, RoutedEventArgs e)
        {
            UpdatePatientRecordWindow window = new UpdatePatientRecordWindow(Appointment.Patient);
            window.Show();
        }

        private void InsertAnamnesis(object sender, RoutedEventArgs e)
        {
            Schedule.Instance.DeleteAppointment(Appointment);
            Appointment.Anamnesis = AnamnesisTextBlock.Text;
            Schedule.Instance.Appointments.Add(Appointment);
            string json = JsonConvert.SerializeObject(Schedule.Instance.Appointments);
            File.WriteAllText("..\\..\\..\\Data\\appointments.json", json);

            MessageBox.Show("Uneta anamneza!", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EndAppointment(object sender, RoutedEventArgs e)
        {
            Schedule.Instance.DeleteAppointment(Appointment);
            Appointment.Finished = true;
            Schedule.Instance.Appointments.Add(Appointment);
            string json = JsonConvert.SerializeObject(Schedule.Instance.Appointments);
            File.WriteAllText("..\\..\\..\\Data\\appointments.json", json);

            DoctorsRoomEquipmentWindow window = new DoctorsRoomEquipmentWindow(Room);
            window.ShowDialog();

            MessageBox.Show("Završen pregled!", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }
    }
}
