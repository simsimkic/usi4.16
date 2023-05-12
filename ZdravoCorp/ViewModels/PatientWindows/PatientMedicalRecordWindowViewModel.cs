using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ZdravoCorp.ViewModels.PatientWindows
{
    internal class PatientMedicalRecordWindowViewModel : INotifyPropertyChanged
    {
        public Patient Patient { get; set; }
        public string PreviousIllnessesJoined { get; set; } = string.Empty;
        public string AllergiesJoined { get; set;} = string.Empty;
        public string Keyword { get; set; } 

        public ObservableCollection<Appointment> FinishedAppointments { get; set; }
        private void AddFinishedAppointments()
        {
            if(FinishedAppointments == null)
            {
                FinishedAppointments = new ObservableCollection<Appointment>();
            }
            foreach (Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.TimeSlot.StartTime.AddMinutes(15) < DateTime.Now && appointment.Patient.Equals(this.Patient))
                {
                    FinishedAppointments.Add(appointment);
                }
            }
        }
        public PatientMedicalRecordWindowViewModel(Patient patient)
        {
            this.Patient = patient;      
            foreach(string illness in this.Patient.MedicalRecord.PreviousIllnesses) 
            {
                PreviousIllnessesJoined +=illness + ", ";
            }
            foreach(string allergies in this.Patient.MedicalRecord.Allergies)
            {
                AllergiesJoined += allergies + ", ";
            }
            AddFinishedAppointments();

        }
        public void KeywordSearch()
        {
            FinishedAppointments.Clear();

            if(this.Keyword == null || this.Keyword == string.Empty)
            {
                AddFinishedAppointments();
            }
            else
            {
                foreach (Appointment appointment in Schedule.Instance.Appointments)
                {
                    string[] anamnesisWords = appointment.Anamnesis.Split(" ");

                    foreach(string word in anamnesisWords)
                    {
                        if (word.Equals(Keyword) && (appointment.TimeSlot.StartTime.AddMinutes(15) < DateTime.Now && appointment.Patient.Equals(this.Patient)))
                        {
                            FinishedAppointments.Add(appointment);
                            break;
                        }
                    }

                }
            }
        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =CollectionViewSource.GetDefaultView(FinishedAppointments);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        } 
        public void SortColumnOnHeaderClick(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction = ListSortDirection.Ascending;
            if (headerClicked != null)
            {
                if(headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;
                    Sort(sortBy, direction);

                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
