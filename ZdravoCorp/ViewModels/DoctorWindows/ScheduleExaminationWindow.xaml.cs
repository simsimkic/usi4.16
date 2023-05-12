using System;
using System.Windows;
using System.Windows.Controls;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class ScheduleExaminationWindow : Window
    {
        public Doctor Doctor { get; private set; }

        public ScheduleExaminationWindow(Doctor doctor)
        {
            InitializeComponent();
            Doctor = doctor;

            UserRepository.Instance.Patients.ForEach(patient =>
            {
                PatientListViewItem item = new PatientListViewItem();
                item.Patient = patient;
                item.Content = patient.InsuranceNumber + " | " + patient.Name + " " + patient.Surname;
                item.FontSize = 18;
                item.Padding = new Thickness(4);
                PatientsListView.Items.Add(item);

                Separator separator = new Separator();
                PatientsListView.Items.Add(separator);
            });
        }

        private void ScheduleExamination(object sender, RoutedEventArgs e)
        {
            if (PatientsListView.SelectedItem == null)
            {
                MessageBox.Show("Izaberite pacijenta!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ExaminationDate.SelectedDate == null)
            {
                MessageBox.Show("Izaberite datum!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string[]? time = CheckTime();
            if (time == null) return;
            int hours = int.Parse(time[0]);
            int minutes = int.Parse(time[1]);

            DateTime date = ExaminationDate.SelectedDate.Value.Date;
            date = new DateTime(date.Year, date.Month, date.Day, hours, minutes, 0);
            if (date <= DateTime.Now)
            {
                MessageBox.Show("Ne možete da zakažete pregled za termin pre sada!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Patient patientToFind = ((PatientListViewItem) PatientsListView.SelectedItem).Patient;
            Patient? patient = ScheduleAppointmentWindow.FindPatient(patientToFind);
            if (patient == null) return;

            TimeSlot examinationTimeSlot = new TimeSlot(date, TimeSpan.FromMinutes(15));

            if (Schedule.Instance.ScheduleAppointment(new Appointment(examinationTimeSlot, Doctor, patient)))
            {
                MessageBox.Show("Uspešno zakazan pregled!", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }
            MessageBox.Show("Ne možete da zakažete pregled za uneti termin!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public string[]? CheckTime()
        {
            try
            {
                string[] time = ExaminationTime.Text.Split(":");
                if (!int.TryParse(time[0], out int hours) ||
                !int.TryParse(time[1], out int minutes))
                {
                    MessageBox.Show("Vreme mora biti u formatu hh:mm!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
                {
                    MessageBox.Show("Pogrešno vreme!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                return time;
            }
            catch (Exception) { return null; }
        }

    }
}
