using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace ZdravoCorp
{
    // Singleton class
    public sealed class Schedule
    {
        private static readonly Schedule instance = new Schedule();


        public static Schedule Instance
        {
            get
            {
                return instance;
            }
        }

        private Schedule()
        {
            Appointments = new List<Appointment>();
        }

        public List<Appointment> Appointments { get; private set; }

        public bool ScheduleAppointment(Appointment appointment)
        {
            if (appointment.Patient.IsAvailable(appointment.TimeSlot) && appointment.Doctor.IsAvailable(appointment.TimeSlot))
            {
                Appointments.Add(appointment);
                string json = JsonConvert.SerializeObject(Appointments);
                File.WriteAllText("..\\..\\..\\Data\\appointments.json", json);

                return true;
            }
            return false;
        }

        public bool ScheduleUrgentAppointment(Patient patient,Specialization specialization,TimeSpan duration,bool isOperation)
        {
            List<Doctor> doctors = UserRepository.Instance.GetDoctorsBy(specialization);
            DateTime now=DateTime.Now;
            DateTime after2hours = DateTime.Now.AddMinutes(120);
            TimeSlot timeSlot=new TimeSlot();

            foreach (Doctor doctor in doctors)
            {
                timeSlot = doctor.GetNextAvailableTimeSlot(now, after2hours, duration);

                if (timeSlot != null)
                {
                    Appointment appointment = new Appointment(timeSlot, doctor, patient, isOperation);
                    ScheduleAppointment(appointment);
                    return true;
                }
            }
            return false;
        }

        public void SaveAppointments()
        {
            string json = JsonConvert.SerializeObject(Appointments);
            File.WriteAllText("..\\..\\..\\Data\\apointments.json", json);
        }

        public void PostponeAppointment(Appointment appointment)
        {
            var appointments = Schedule.Instance.Appointments.OrderBy(a => a.TimeSlot.StartTime);

            DateTime currentTime = DateTime.Now;
            TimeSlot timeSlot = new TimeSlot(currentTime, appointment.TimeSlot.Duration);
            Doctor doctor = appointment.Doctor;

            while (true)
            {
                if (doctor.IsAvailable(timeSlot))
                {
                    appointment.TimeSlot = timeSlot;
                    break;
                }

                timeSlot.StartTime = timeSlot.StartTime.AddMinutes(7);
            }
        }

        public List<Appointment> GetCurrentAppointmentsBy(Specialization specialization)
        {
            var appointments = Schedule.Instance.Appointments
        .Where(a => a.Doctor.Specialization == specialization && a.TimeSlot.StartTime > DateTime.Now)
        .OrderBy(a => a.TimeSlot.StartTime)
        .ToList();

            return appointments;
        }

        public void DeleteAppointment(Appointment appointment)
        {
            Appointments.Remove(appointment);
            string json = JsonConvert.SerializeObject(Appointments);
            File.WriteAllText("..\\..\\..\\Data\\appointments.json", json);
        }

        public void ReadSchedule()
        {
            string json = File.ReadAllText("..\\..\\..\\Data\\appointments.json");
            Appointments = JsonConvert.DeserializeObject<List<Appointment>>(json);
        }

    }
}