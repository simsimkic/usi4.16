using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Newtonsoft.Json;
using ZdravoCorp.ViewModels.NurseWindows;
using static System.Net.Mime.MediaTypeNames;

namespace ZdravoCorp
{
    public partial class NurseWindow : Window
    {
        public Nurse Nurse { get; private set; }
        public ObservableCollection<Patient> Patients
        { get; set; }
        public NurseWindow(Nurse nurse)
        {
            this.Nurse = nurse;
            InitializeComponent();
            this.DataContext = this;
            Patients = new ObservableCollection<Patient>(UserRepository.Instance.Patients);
            PatientsTable.ItemsSource = Patients;
            this.Title = this.Title + " - " + this.Nurse.Name + " " + this.Nurse.Surname;
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            PatientRegistrationWindow patientRegistrationWindow = new PatientRegistrationWindow();
            patientRegistrationWindow.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)PatientsTable.SelectedItem;
            if (selectedPatient != null)
            {
                Patients.Remove(selectedPatient);
                UserRepository.Instance.SavePatients();
            }
            else
            {
                MessageBox.Show("Pacijent nije izabran");
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)PatientsTable.SelectedItem;
            if (selectedPatient != null)
            {
                PatientEditWindow patientEditWindow = new PatientEditWindow(selectedPatient);
                patientEditWindow.Show();
            }
            else
            {
                MessageBox.Show("Pacijent nije izabran");
            }
        }

        private void DataGridRow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            if (row != null && row.IsSelected)
            {
                PatientsTable.SelectedIndex = -1;
                e.Handled = true;
            }
        }

        private void Reception_Click(object sender, RoutedEventArgs e)
        {
            if(PatientsTable.SelectedItem == null)
            {
                MessageBoxResult result = MessageBox.Show("Pacijent nije izabran, registrovati novog pacijenta?",
                    "Prijem novog pacijenta", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    PatientRegistrationWindow patientRegistrationWindow = new PatientRegistrationWindow();
                    patientRegistrationWindow.Show();
                }
            }
            else
            {
                TimeSlot timeSlot= new TimeSlot(DateTime.Now,TimeSpan.FromMinutes(15));
                Patient selectedPatient = (Patient)PatientsTable.SelectedItem;

                if (!selectedPatient.IsAvailable(timeSlot))
                {
                    ReceptionWindow receptionWindow = new ReceptionWindow(selectedPatient);
                    receptionWindow.Show();
                }
                else
                {
                    MessageBox.Show("Ne mozete primiti izabranog pacijenta");
                }
            }
        }

        private void UrgentExamination_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)PatientsTable.SelectedItem;

            if (selectedPatient != null)
            {
                UrgentExaminationWindow urgentExaminationWindow = new UrgentExaminationWindow(selectedPatient);
                urgentExaminationWindow.Show();
            }
            else
            {
                MessageBox.Show("Pacijent nije izabran");
            }
        }
    }
}
