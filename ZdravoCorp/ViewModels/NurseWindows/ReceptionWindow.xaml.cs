using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZdravoCorp.ViewModels.NurseWindows
{
    /// <summary>
    /// Interaction logic for ReceptionWindow.xaml
    /// </summary>
    public partial class ReceptionWindow : Window
    {
        Patient patient;
        public ReceptionWindow(Patient patient)
        {
            this.patient = patient;
            InitializeComponent();
            PatientNameLabel.Content = patient.Name + " " + patient.Surname;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SymptomsTextbox.Text.Trim() == "")
            {
                MessageBox.Show("Morate uneti simptome", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string[] symptoms = SymptomsTextbox.Text.Split(",");
                patient.MedicalRecord.Symptoms=new List<string>(symptoms);

                if (PreviousIllnessesTextbox.Text.Trim() != "")
                {
                    string[] previousIlnesesses = PreviousIllnessesTextbox.Text.Split(",");
                    patient.MedicalRecord.PreviousIllnesses=new List<string>(previousIlnesesses);
                }

                if (AllergensTextbox.Text.Trim() != "")
                {
                    string[] allergens = AllergensTextbox.Text.Split(",");
                    patient.MedicalRecord.Allergies=new List<string>(allergens);
                }

                UserRepository.Instance.SavePatients();
                MessageBox.Show("Anamneza je uspesno dodata");
                Close();
            }
        }
    }
}
