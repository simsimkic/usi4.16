using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZdravoCorp.ViewModels.PatientWindows
{
    internal class ConditionalScheduleAppointmentWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Doctor> AvailableDoctors { get; set; } 

        public Doctor SelectedDoctor { get; set; }

        public string SelectedPriority { get; set; } 

        public ObservableCollection<string> PriorityOptions { get; set; }

        public ObservableCollection<Appointment> FoundAppointments { get; set; } 

        public Appointment SelectedAppointment { get; set; }

        public string RawLowerBoundTime { get; set; }

        public string RawUpperBoundTime { get; set; }

        public DateTime SelectedDate { get; set; } = DateTime.Now;

        public Patient Patient { get; set; }

        public ConditionalScheduleAppointmentWindowViewModel(Patient patient) 
        {
            this.Patient = patient;
            PriorityOptions = new ObservableCollection<string>
            {
                "Doktor",
                "Vreme",
                "Nesto"
            };
            FoundAppointments = new ObservableCollection<Appointment>();
            AvailableDoctors = new ObservableCollection<Doctor>();
            foreach(Doctor doctor in UserRepository.Instance.Doctors) 
            {
                AvailableDoctors.Add(doctor);
            }
        }
        public bool ShowAppointmentIfPossible(DateTime date,Doctor doctor)
        {
            TimeSpan duration = TimeSpan.FromMinutes(15);
            TimeSlot appointmentTime = new TimeSlot(date, duration);
            Appointment appointment = new Appointment(appointmentTime, doctor, this.Patient);
            if (appointment.isAppointmentAvailable())
            {
                FoundAppointments.Add(appointment);
                return true;
            }
            return false;
        }
        public bool ScheduleInGivenTimeSpan(DateTime spanBegin, DateTime spanEnd, Doctor doctor)
        {
            for (DateTime date = spanBegin; date <= spanEnd; date = date.AddMinutes(15))
            {
                if (ShowAppointmentIfPossible(date, doctor)) return true;
            }
            return false;
        } 
        public int ScheduleOutsideGivenTimeSpanMultiple(DateTime spanBegin, DateTime spanEnd,Doctor doctor, int numberOfAppointments)
        {
            DateTime dateDayBeggining = new DateTime(spanBegin.Year, spanBegin.Month, spanBegin.Day, 0, 0, 0);
            DateTime dateDayEnding = new DateTime(spanBegin.Year, spanBegin.Month, spanBegin.Day, 23, 59, 0);
            int scheduledAppointments = 0;

            while (spanBegin > dateDayBeggining || spanEnd < dateDayEnding)
            {
                if (ShowAppointmentIfPossible(spanBegin, doctor)) scheduledAppointments++;
                if (scheduledAppointments >= numberOfAppointments) break;

                if (ShowAppointmentIfPossible(spanEnd, doctor)) scheduledAppointments++;
                if (scheduledAppointments >= numberOfAppointments) break;

                if (spanBegin > dateDayBeggining)
                    spanBegin = spanBegin.AddMinutes(-15);
                if (spanEnd < dateDayEnding)
                    spanEnd = spanEnd.AddMinutes(15);
            }
            return numberOfAppointments - scheduledAppointments;
        }
        public bool ScheduleOutsideGivenTimeSpan(DateTime spanBegin, DateTime spanEnd, Doctor doctor)
        {
            DateTime dateDayBeggining = new DateTime(spanBegin.Year, spanBegin.Month, spanBegin.Day, 0, 0, 0);
            DateTime dateDayEnding = new DateTime(spanBegin.Year, spanBegin.Month, spanBegin.Day, 23, 59, 0);

            while(spanBegin > dateDayBeggining || spanEnd < dateDayEnding)
            {
                if(ShowAppointmentIfPossible(spanBegin,doctor)) return true;

                if (ShowAppointmentIfPossible(spanEnd, doctor)) return true;

                if (spanBegin > dateDayBeggining)
                    spanBegin = spanBegin.AddMinutes(-15);
                if (spanEnd < dateDayEnding)
                    spanEnd.AddMinutes(15);
            }
            return false;
        }
        public void ScheduleClosestPossible(Doctor doctor)
        {
        
            int numberOfScheduledAppointments = 3;
            for (DateTime currentDate = DateTime.Now; currentDate <= SelectedDate.AddDays(1); currentDate = currentDate.AddDays(1))
            {        
                DateTime dateLowerBound = CreateDate(currentDate, RawLowerBoundTime);
                DateTime dateUpperBound = CreateDate(currentDate, RawUpperBoundTime);
                numberOfScheduledAppointments = ScheduleOutsideGivenTimeSpanMultiple(dateLowerBound, dateUpperBound, doctor, numberOfScheduledAppointments);
                if (numberOfScheduledAppointments == 0) break;
            }
        }
        private bool ScheduleByDatePriority(Doctor doctor, DateTime dateLowerBound, DateTime dateUpperBound)
        {
            bool scheduledWithDoctorPriority = ScheduleInGivenTimeSpan(dateLowerBound, dateUpperBound,doctor);
            if (scheduledWithDoctorPriority) return true;
            foreach( Doctor differentDoctor in UserRepository.Instance.Doctors)
            {
                bool succesfulSchedule = ScheduleInGivenTimeSpan(dateLowerBound, dateUpperBound, differentDoctor);
                if(succesfulSchedule) return true;
            }
            return false;
        }
        private bool ScheduleByDoctorPriority(Doctor doctor, DateTime dateLowerBound, DateTime dateUpperBound)
        {      
            bool scheduledInsideTimeSpan = ScheduleInGivenTimeSpan(dateLowerBound, dateUpperBound, doctor);
            if(!scheduledInsideTimeSpan)
            {
                bool scheduledOutsideTimeSpan = ScheduleOutsideGivenTimeSpan(dateLowerBound,dateUpperBound, doctor);
                if(scheduledOutsideTimeSpan) return true;
            }
            return scheduledInsideTimeSpan;
         
        }
        private int[] ExtractTime(string rawTime)
        {
            if(rawTime == null)
            {
                MessageBox.Show("Polje za vreme je prazno");
                return new int[] { -1 };
            }
            string[] time = rawTime.Split(":");
            if(time.Length != 2 ) 
            {
                MessageBox.Show("Neispravan format");
                return new int[] { -1 };
            }
            if (!int.TryParse(time[0], out int hours) || !int.TryParse(time[1], out int minutes))
            {
                MessageBox.Show("Neispravan format");
                return new int[] { -1 };
            }
            if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
            {
               MessageBox.Show("Neispravan format");
                return new int[] { -1 };
            }
            return new int[] { hours, minutes };
        }
        private DateTime CreateDate(DateTime date,string rawTime)
        {
            int[] time = ExtractTime(rawTime);
            if (time[0] == -1)
            {
                return DateTime.MinValue;
            }
            date = new DateTime(date.Year, date.Month, date.Day, time[0], time[1], 0);
            return date;
        }
        public bool CheckEmptyFields()
        {
            return (SelectedPriority == null || RawLowerBoundTime == null
                || RawUpperBoundTime == null || SelectedDoctor == null) ;
        }
        public void ScheduleAppointmentConditionally(object sender, RoutedEventArgs e)
        {
            if (CheckEmptyFields())
            {
                MessageBox.Show("Sva polja moraju biti popunjena!");
                return;
            }

            FoundAppointments.Clear();
            bool successfulSchedule = false;
            for (DateTime currentDate = DateTime.Now;currentDate<=SelectedDate.AddDays(1);currentDate = currentDate.AddDays(1))
            {
                DateTime dateLowerBound = CreateDate(currentDate,RawLowerBoundTime);
                DateTime dateUpperBound = CreateDate(currentDate,RawUpperBoundTime);               
                if (dateLowerBound == DateTime.MinValue || dateUpperBound == DateTime.MinValue)
                {
                    MessageBox.Show("Datum nije unet");
                    return;
                }          
                if (SelectedPriority == "Doktor")
                {
                    successfulSchedule = ScheduleByDoctorPriority(SelectedDoctor, dateLowerBound, dateUpperBound);
                }
                else
                {
                   successfulSchedule = ScheduleByDatePriority(SelectedDoctor, dateLowerBound, dateUpperBound);
                }
                if(successfulSchedule)
                {
                    break;
                }
            }
            if(!successfulSchedule)
            {
                ScheduleClosestPossible(SelectedDoctor);
            }
        }
        public void ScheduleConfirmation(object sender, RoutedEventArgs e)
        {
            Appointment chosenAppointment;
            if (FoundAppointments.Count == 0)
            {
                MessageBox.Show("Nema ponudjenih datuma");
                return;
            }
            if (SelectedAppointment == null && FoundAppointments.Count == 1)
            {
                chosenAppointment = (Appointment)FoundAppointments[0];
            }
            else if (SelectedAppointment == null && FoundAppointments.Count != 1)
            {
                MessageBox.Show("Morate izabrati jedan od ponudjenih termina");
                return;
            }
            else
            {
                chosenAppointment = SelectedAppointment;
            }
            Schedule.Instance.ScheduleAppointment(chosenAppointment);
            PatientWindowViewModel.PatientAppointments.Add(chosenAppointment);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
