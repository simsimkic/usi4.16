using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace ZdravoCorp.ViewModels.PatientWindows
{
    /// <summary>
    /// Interaction logic for editAppointmentWindow.xaml
    /// </summary>
    public partial class editAppointmentWindow : Window
    {
        public Appointment selectedAppointment;
        public Appointment changedSelectedAppointment;
        public int Index;
        public Patient Patient;
        public editAppointmentWindow(Appointment _selectedAppointment,int _Index, Patient patient)
        {
            InitializeComponent();
            Patient = patient;
            selectedAppointment = _selectedAppointment;
            Index = _Index;
            string dateFormat = "dd/MM/yyyy HH:mm";
            appointmentBeginTB.Text = selectedAppointment.TimeSlot.StartTime.ToString(dateFormat);
            appointmentDoctorTB.Text = selectedAppointment.Doctor.Name;
            
            
        }

        private void appointmentButton_Click(object sender, RoutedEventArgs e)
        {
            List<ArchiveRecord> records;
            string json1 = File.ReadAllText("..\\..\\..\\Data\\archive.json");
            records = JsonConvert.DeserializeObject<List<ArchiveRecord>>(json1);
            if (records == null)
            {
                records = new List<ArchiveRecord>();
            }
            string newAppointmentDoctor = appointmentDoctorTB.Text;
            string newAppointmentBegin = appointmentBeginTB.Text;
            if (newAppointmentDoctor == null || newAppointmentDoctor == "")
            {
                MessageBox.Show("Niste izabrali doktora");
                return;
            }
            if (newAppointmentBegin == null || newAppointmentBegin == "")
            {
                MessageBox.Show("Niste izabrali datum");
                return;
            }
            string dateFormat = "dd/MM/yyyy HH:mm";
            DateTime beginDate;
            if (!DateTime.TryParseExact(newAppointmentBegin, dateFormat, CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out beginDate))
            {
                MessageBox.Show("Neispravan format termina!");
                return;
            }
            Appointment newAppointment = new Appointment(newAppointmentBegin, newAppointmentDoctor);
            newAppointment.Patient = Patient;
            if (newAppointmentDoctor.Trim() == "")
            {
                MessageBox.Show("Niste izabrali doktora");
                return;
            }
            if (newAppointment.TimeSlot.StartTime < DateTime.Now)
            {
                MessageBox.Show("Novo vreme termina nije validno");
                return;
            }
            PatientWindow patientWindow = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
            foreach (Appointment currentAppointment in Schedule.Instance.Appointments)
            {
                TimeSlot currentTimeSlot = currentAppointment.TimeSlot;
                if (newAppointment.TimeSlot.IsOverlapping(currentTimeSlot) && newAppointment.Doctor.Name == currentAppointment.Doctor.Name)
                {
                    MessageBox.Show("Termin je vec popunjen");
                    return;
                }
            }
            ArchiveRecord archiveRecord = new ArchiveRecord(newAppointment.Patient, DateTime.Now, "edit");
            records.Add(archiveRecord);
            string json = JsonConvert.SerializeObject(records);
            File.WriteAllText("..\\..\\..\\Data\\archive.json", json);
            selectedAppointment.TimeSlot = new TimeSlot(newAppointmentBegin, "15");
            selectedAppointment.Doctor.Name = newAppointmentDoctor;
            changedSelectedAppointment = selectedAppointment;
            Schedule.Instance.Appointments[Index].TimeSlot = selectedAppointment.TimeSlot;
            Schedule.Instance.Appointments[Index].Doctor.Name = selectedAppointment.Doctor.Name;
            Schedule.Instance.Appointments[Index].NotifyPropertyChanged("TimeSlot");
            Schedule.Instance.Appointments[Index].NotifyPropertyChanged("Doctor");
            json = JsonConvert.SerializeObject(Schedule.Instance.Appointments);
            File.WriteAllText("..\\..\\..\\Data\\apointments.json", json);
            MessageBox.Show("Uspesno izmenjen");
            Close();
            return;

        }
    }
}
