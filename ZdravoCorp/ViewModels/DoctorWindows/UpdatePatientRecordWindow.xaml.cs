using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class UpdatePatientRecordWindow : Window
    {

        public Patient Patient { get; private set; }

        public UpdatePatientRecordWindow(Patient patient)
        {
            InitializeComponent();
            Patient = patient;
        }

        private void UpdateMedicalRecord(object sender, RoutedEventArgs e)
        {
            double height = 0.0;
            double weight = 0.0;

            if (HeightTextBox.Text.Trim() != "" && !double.TryParse(HeightTextBox.Text, out height))
            {
                MessageBox.Show("Za visinu morate uneti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (WeightTextBox.Text.Trim() != "" && !double.TryParse(WeightTextBox.Text, out weight))
            {
                MessageBox.Show("Za težinu morate uneti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (HeightTextBox.Text.Trim() != "") Patient.MedicalRecord.Height = height;
            
            if (WeightTextBox.Text.Trim() != "") Patient.MedicalRecord.Weight = weight;

            if (IllnessTextBox.Text.Trim() != "") Patient.MedicalRecord.PreviousIllnesses.Add(IllnessTextBox.Text);

            if (AllergieTextBox.Text.Trim() != "") Patient.MedicalRecord.Allergies.Add(AllergieTextBox.Text);
            
            foreach (Patient foundPatient in UserRepository.Instance.Patients)
            {
                if (foundPatient.Equals(Patient))
                {
                    UserRepository.Instance.RemovePatient(foundPatient);
                    break;
                }
            }
            UserRepository.Instance.AddPatient(Patient);
            string json = JsonConvert.SerializeObject(UserRepository.Instance.Patients);
            File.WriteAllText("..\\..\\..\\Data\\patients.json", json);

            if (HeightTextBox.Text.Trim() != "" &&
                WeightTextBox.Text.Trim() != "" &&
                IllnessTextBox.Text.Trim() != "" &&
                AllergieTextBox.Text.Trim() != "")
            {
                MessageBox.Show("Uspešno ažuriran zdravstveni karton!", "Uspešno", MessageBoxButton.OK, MessageBoxImage.Information);
            }
             Close();
        }
    }
}
