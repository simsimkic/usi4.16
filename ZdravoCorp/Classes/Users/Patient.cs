using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ZdravoCorp
{
    public class Patient : User, INotifyPropertyChanged
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string InsuranceNumber { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public DateTime BirthDate { get; set; }
        public string Notification { get; set;}
        public Patient() { }
        public Patient(string name, string surname, Credentials credentials, string insuranceNumber) : base(name, surname, credentials)
        {
            InsuranceNumber = insuranceNumber;
            MedicalRecord = new MedicalRecord();
            Notification = "";
        }
        public bool Equals(Patient other)
        {
            return Name == other.Name && Surname == other.Surname && InsuranceNumber == other.InsuranceNumber;
        }

        public bool IsAvailable(TimeSlot timeSlot)
        {
            foreach (Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.Patient.Equals(this) &&
                    appointment.Finished == false &&
                    appointment.TimeSlot.IsOverlapping(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }
        public bool overLimit(List<ArchiveRecord> records)
        {
            return false;
            int numberOfAppointments = 0;
            int numberOfChanges = 0;
            foreach(ArchiveRecord record in records)
            {
                if(DateTime.Now.AddDays(-30) < record.Date)
                {
                    if(record.ChangeType == "add" && record.User.Name == this.Name)
                    {
                        numberOfAppointments++;
                    }
                    else if((record.ChangeType == "edit" || record.ChangeType == "delete") && record.User.Name == this.Name)
                    {
                        numberOfChanges++;
                    }
                }
            }
            if(numberOfAppointments > 8 || numberOfChanges >= 5)
            {
                return true;
            }
            return false;
        }
        public List<Appointment> GetAppointmentsFor(TimeSlot timeSlot)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.Patient.Equals(this) && appointment.TimeSlot.IsOverlapping(timeSlot))
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
