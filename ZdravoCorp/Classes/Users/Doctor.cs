using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ZdravoCorp
{
    public enum Specialization
    {
        General,
        GeneralSurgery,
        Anesthesiology,
        Cardiology,
        Neurology,
        Otolaryngology,
        Pathology,
        Pediatrics,
        Radiology,
        Urology
    }

    public class Doctor : User, INotifyPropertyChanged
    {
        public Specialization Specialization { get; set; }
        public float AverageRating { get; set; }
        public string Notification { get; set; }
        public Doctor() { }
        public Doctor(string name, string surname, Credentials credentials, Specialization specialization) : base(name, surname, credentials)
        {
            Specialization = specialization;
            Notification = "";
        }
        public Doctor(string name) : base(name, "", new Credentials("", "")) {
            Notification = "";
        }

        public bool Equals(Doctor other)
        {
            return Name == other.Name && Surname == other.Surname && Specialization == other.Specialization;
        }

        public bool IsAvailable(TimeSlot timeSlot)
        {
            foreach (Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.Doctor.Equals(this) &&
                    appointment.Finished == false &&
                    appointment.TimeSlot.IsOverlapping(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public List<Appointment> GetAppointmentsFor(TimeSlot timeSlot)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.Doctor.Equals(this) && appointment.TimeSlot.IsOverlapping(timeSlot))
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

        public TimeSlot? GetNextAvailableTimeSlot(DateTime startTime, DateTime endTime, TimeSpan duration)
        {
            TimeSlot currentTimeSlot = new TimeSlot(DateTime.Now, duration);
            currentTimeSlot.StartTime = startTime;
            while (currentTimeSlot.StartTime < endTime)
            {
                if (!IsAvailable(currentTimeSlot))
                {
                    currentTimeSlot.StartTime = currentTimeSlot.StartTime.AddMinutes(10);
                }
                else
                {
                    return currentTimeSlot;
                }
            }
            return null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
