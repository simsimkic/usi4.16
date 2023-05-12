using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZdravoCorp.ViewModels.PatientWindows
{
    internal class PatientWindowViewModel : INotifyPropertyChanged
    {
        public static ObservableCollection<Appointment> PatientAppointments { get; set; }

        public Appointment SelectedAppointment {get; set;}

        public int SelectedIndex { get; set;}

        public Patient Patient { get; set;}
        
        public PatientWindowViewModel(Patient patient)
        {
            Patient = patient;
            PatientAppointments = new ObservableCollection<Appointment>();
            foreach (Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.Patient.Equals(patient))
                {
                    PatientAppointments.Add(appointment);
                }
                
            }
        }
        public bool isAppointmentSelectionValid()
        {
            if (SelectedAppointment == null)
            {            
                return false;
            }
            //Appointment selectedAppointment = (Appointment)this.appointmentsListView.SelectedItem;
            string dateFormat = "dd/MM/yyyy HH:mm"; //Treba namestiti globalan dateformat
            DateTime beginDate = DateTime.ParseExact(SelectedAppointment.TimeSlot.StartTime.ToString(dateFormat), dateFormat, CultureInfo.InvariantCulture);
            if (beginDate.AddDays(-1) < DateTime.Now)
            {            
                return false;
            }
            return true;
        }
        public void editAppointment(object sender, RoutedEventArgs e)
        {
            if (this.isAppointmentSelectionValid() == true)
            {
                editAppointmentWindow _editAppointmentWindow = new editAppointmentWindow((Appointment)SelectedAppointment, SelectedIndex, Patient);
                _editAppointmentWindow.Show();
            }
        }

        public void DeleteAppointment(object sender, RoutedEventArgs e)
        {

            Appointment selectedAppointment = SelectedAppointment;
            if (SelectedAppointment != null)
            {
                List<ArchiveRecord> records;
                string json1 = File.ReadAllText("..\\..\\..\\Data\\archive.json");
                records = JsonConvert.DeserializeObject<List<ArchiveRecord>>(json1);
                if (records == null)
                {
                    records = new List<ArchiveRecord>();
                }
                ArchiveRecord archiveRecord = new ArchiveRecord(Patient, DateTime.Now, "delete");
                records.Add(archiveRecord);
                string json = JsonConvert.SerializeObject(records);
                File.WriteAllText("..\\..\\..\\Data\\archive.json", json);
                Schedule.Instance.DeleteAppointment(selectedAppointment);
                PatientAppointments.Remove(selectedAppointment);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }

    }
}
