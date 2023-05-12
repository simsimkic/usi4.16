using Microsoft.VisualBasic;
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
    /// Interaction logic for scheduleAppointmentWindow.xaml
    /// </summary>

    public partial class scheduleAppointmentWindow : Window
    {
        public Patient Patient { get; set; }
        public scheduleAppointmentWindow(Patient patient)
        {
            InitializeComponent();
            UserRepository.Instance.Doctors.ForEach(doctor => { appointmentDoctors.Items.Add(doctor); });
            Patient = patient;
        }
        //private bool isNewAppointmentValid(Appointment appointment)
        //{
        //    string dateFormat = "dd/MM/yyyy HH:mm";
        //    string date = appointmentBeginTB.Text;
        //    string doctor = appointmentDoctorTB.Text;
        //    DateTime beginDate;
        //    if (appointment.Doctor.Name == null || appointment.Doctor.Name == "")
        //    {
        //        MessageBox.Show("Niste izabrali doktora");
        //        return false;
        //    }
        //    if (appointment.TimeSlot.StartTime == null)//Moguca greska(sta se desi kad za dateTime prosledimo ""
        //    {
        //        MessageBox.Show("Niste izabrali datum");
        //        return false;
        //    }
        //    if (!DateTime.TryParseExact(date, dateFormat, CultureInfo.InvariantCulture,
        //               DateTimeStyles.None, out beginDate))
        //    {
        //        MessageBox.Show("Neispravan format termina!");
        //        return false;
        //    }
        //}
        private void appointmentButton_Click(object sender, RoutedEventArgs e)
        {
            string[] time = appointmentBeginTB.Text.Split(":");
            if (!int.TryParse(time[0], out int hours) ||
                    !int.TryParse(time[1], out int minutes))
            {
                MessageBox.Show("Vreme mora biti u formatu hh:mm!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
            {
                MessageBox.Show("Pogrešno vreme!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DateTime date = AppointmentDate.SelectedDate.Value.Date;
            date = new DateTime(date.Year, date.Month, date.Day, hours, minutes, 0);
            string dateFormat = "dd/MM/yyyy HH:mm";
            //Appointment appointment = new Appointment(appointmentBeginTB.Text, appointmentDoctorTB.Text);
            DateTime beginDate;
            if (appointmentDoctors.SelectedItem == null)
            {
                MessageBox.Show("Niste izabrali doktora");
                return;
            }
            if (date == null)
            {
                MessageBox.Show("Niste izabrali datum");
                return;
            }
            Doctor doctor = appointmentDoctors.SelectedItem as Doctor;
            
            if(date < DateTime.Now)
            {
                MessageBox.Show("Ne mozete zakazati termin u proslosti");
                return;
            }
            TimeSpan duration = TimeSpan.FromMinutes(15);
            TimeSlot appointmentTime = new TimeSlot(date, duration);
            Appointment appointment = new Appointment(appointmentTime, doctor, Patient);

            //bool succesfulSchelude = Schedule.Instance.ScheduleAppointment(appointment);
            PatientWindow patientWindow = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
            foreach (Appointment currentAppointment in Schedule.Instance.Appointments)
            {
                TimeSlot currentTimeSlot = currentAppointment.TimeSlot;
                if (appointment.TimeSlot.IsOverlapping(currentTimeSlot) && Patient.Equals(currentAppointment.Patient))
                {
                    MessageBox.Show("Termin je vec popunjen");
                    Close();
                    return;
                }
            }
            ArchiveRecord archiveRecord = new ArchiveRecord(appointment.Patient, DateTime.Now, "add");
            Archive.Instance.createRecord(archiveRecord);
            string json = JsonConvert.SerializeObject(Archive.Instance.ArchiveRecords);
            File.WriteAllText("..\\..\\..\\data\\archive.json", json);
            Schedule.Instance.Appointments.Add(appointment);     
            json = JsonConvert.SerializeObject(Schedule.Instance.Appointments);
            File.WriteAllText("..\\..\\..\\data\\apointments.json", json);
            MessageBox.Show("Uspesno dodat termin");
            Close();
            return;
        }

        private void appointmentBeginTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
