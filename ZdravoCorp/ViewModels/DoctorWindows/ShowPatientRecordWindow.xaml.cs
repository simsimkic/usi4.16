using System.Windows;
using System.Windows.Controls;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class ShowPatientRecordWindow : Window
    {

        public Patient Patient { get; private set; }

        public ShowPatientRecordWindow(Patient patient)
        {
            InitializeComponent();
            Patient = patient;
            NameBlock.Text = Patient.Name;
            SurnameBlock.Text = Patient.Surname;
            if (Patient.MedicalRecord.Height == 0.0)
            {
                HeightBlock.Text = "-";
            }
            else
            {
                HeightBlock.Text = Patient.MedicalRecord.Height.ToString();
            }
            if (Patient.MedicalRecord.Weight == 0.0)
            {
                WeightBlock.Text = "-";
            }
            else
            {
                WeightBlock.Text = Patient.MedicalRecord.Weight.ToString();
            }
            foreach (string illness in Patient.MedicalRecord.PreviousIllnesses)
            {
                PreviousIllnessesListView.Items.Add(illness);
            }
            foreach (string allergy in Patient.MedicalRecord.Allergies)
            {
                AllergiesListView.Items.Add(allergy);
            }
        }



    }
}
