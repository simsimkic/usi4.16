using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ZdravoCorp
{
    public class Appointment : INotifyPropertyChanged
    {
        public string? proba { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public bool IsOperation { get; set; }
        public string Anamnesis { get; set; }
        public bool Finished { get; set; }

        public Appointment(TimeSlot timeSlot, Doctor doctor, Patient patient, bool isOperation = false)
        {
            TimeSlot = timeSlot;
            Doctor = doctor;
            Patient = patient;
            IsOperation = isOperation;
            Finished = false;
            Anamnesis = "";
        }
        public Appointment()
        {

        }
        public bool isAppointmentAvailable()
        {
            return (this.Patient.IsAvailable(this.TimeSlot) && this.Doctor.IsAvailable(this.TimeSlot));
           
        }
        public Appointment(string dateBegin,string doctor)
        {
            proba = "Proba";
            TimeSlot = new TimeSlot(dateBegin, "15");
            Doctor = new Doctor(doctor);
            Patient = new Patient();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MedicalRecord GetPatientInformation()
        {
            return Patient.MedicalRecord;
        }

    }
}
