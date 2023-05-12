using System;
using System.Collections.Generic;
using System.Windows;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class SearchPatientsWindow : Window
    {

        public Doctor Doctor { get; private set; }
        private List<Patient> DoctorsPatients { get; set; }

        public SearchPatientsWindow(Doctor doctor)
        {
            InitializeComponent();
            Doctor = doctor;
            DoctorsPatients = new List<Patient>();
            foreach (Appointment appointment in Schedule.Instance.Appointments)
            {
                if (appointment.Doctor.Equals(Doctor) && appointment.Finished == true)
                {
                    SetFullInfoPatient(appointment.Patient);
                }
            }
            UpdatePatientsView(UserRepository.Instance.Patients);
        }

        private void UpdatePatientsView(List<Patient> patientsToShow)
        {
            PatientsListView.Items.Clear();
            foreach (Patient patient in patientsToShow)
            {
                PatientsListView.Items.Add(patient);
            }
        }

        private void ShowOnlyDoctorsPatients(object sender, RoutedEventArgs e)
        {
            UpdatePatientsView(DoctorsPatients);
        }

        private void ShowAllPatients(object sender, RoutedEventArgs e)
        {
            UpdatePatientsView(UserRepository.Instance.Patients);
        }

        private void UpdateMedicalRecord(object sender, RoutedEventArgs e)
        {
            if (PatientsListView.SelectedItem == null)
            {
                MessageBox.Show("Izaberite pacijenta!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Patient patient = (Patient) PatientsListView.SelectedItem;
            bool patientFound = false;

            foreach (Patient fullInfoPatient in DoctorsPatients)
            {
                if (fullInfoPatient.Equals(patient))
                {
                    patient = fullInfoPatient;
                    patientFound = true;
                    break;
                }
            }

            if (!patientFound)
            {
                MessageBox.Show("Ne možete pogledati ili izmeniti zdravstveni karton pacijenta koji nije vaš!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UpdatePatientRecordWindow updateWindow = new UpdatePatientRecordWindow(patient);
            updateWindow.Show();
        }

        private void LookMedicalRecord(object sender, RoutedEventArgs e)
        {
            if (PatientsListView.SelectedItem == null)
            {
                MessageBox.Show("Izaberite pacijenta!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Patient patient = (Patient) PatientsListView.SelectedItem;
            bool patientFound = false;

            foreach (Patient fullInfoPatient in DoctorsPatients)
            {
                if (fullInfoPatient.Equals(patient))
                {
                    patient = fullInfoPatient;
                    patientFound = true;
                    break;
                }
            }

            if (!patientFound)
            {
                MessageBox.Show("Ne možete pogledati ili izmeniti zdravstveni karton pacijenta koji nije vaš!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ShowPatientRecordWindow patientMedicalRecordWindow = new ShowPatientRecordWindow(patient);
            patientMedicalRecordWindow.Show();
        }

        private void SetFullInfoPatient(Patient patientToSet)
        {
            foreach (Patient patient in UserRepository.Instance.Patients)
            {
                if (patient.Equals(patientToSet))
                {
                    if (!DoctorsPatients.Contains(patient)) DoctorsPatients.Add(patient);
                    break;
                }
            }
        }

    }
}
