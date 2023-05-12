using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientEditWindow.xaml
    /// </summary>
    public partial class PatientEditWindow : Window
    {
        public Patient Patient;
        public PatientEditWindow(Patient patient)
        {
            Patient = patient;
            InitializeComponent();
            ImeTextBox.Text = Patient.Name;
            PrezimeTextBox.Text = Patient.Surname;
            InsuranceNumberTextBox.Text = Patient.InsuranceNumber;
            HeightTextBox.Text = Patient.MedicalRecord.Height.ToString();
            WeightTextBox.Text = Patient.MedicalRecord.Weight.ToString();
            TelephoneTextBox.Text = Patient.PhoneNumber;
            AddressTextBox.Text = Patient.Address;
            EmailTextBox.Text = Patient.Email;
            BirthDatePicker.SelectedDate = Patient.BirthDate;
            PasswordTextBox.Text = Patient.Credentials.Password;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = ImeTextBox.Text;
            string surname = PrezimeTextBox.Text;
            string insuranceNumber = InsuranceNumberTextBox.Text;
            DateTime birthDate = BirthDatePicker.SelectedDate.Value.Date;
            string bolesti = DiseasesTextBox.Text;
            if (name.Trim() == "" || surname.Trim() == "" || insuranceNumber.Trim() == "" || HeightTextBox.Text.Trim() == "" || WeightTextBox.Text.Trim() == "" || AddressTextBox.Text.Trim() == ""
                || EmailTextBox.Text.Trim() == "" || PasswordTextBox.Text.Trim() == "" || TelephoneTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Polja ne smeju biti prazna");
            }
            else
            {
                double height = Convert.ToDouble(HeightTextBox.Text);
                double weight = Convert.ToDouble(WeightTextBox.Text);
                NurseWindow nurseWindow = Application.Current.Windows.OfType<NurseWindow>().FirstOrDefault();
                MessageBox.Show("Uspesna izmena");

                Patient.Name = name;
                Patient.Surname = surname;
                Patient.InsuranceNumber = insuranceNumber;
                Patient.MedicalRecord.Height = height;
                Patient.MedicalRecord.Weight = weight;
                Patient.BirthDate = birthDate;
                Patient.Email = EmailTextBox.Text;
                Patient.Address = AddressTextBox.Text;
                Patient.PhoneNumber = TelephoneTextBox.Text;
                Patient.Credentials.Password = PasswordTextBox.Text;
                Patient.Credentials.Username = EmailTextBox.Text;

                Patient.NotifyPropertyChanged("Name");
                Patient.NotifyPropertyChanged("Surname");
                Patient.NotifyPropertyChanged("InsuranceNumber");
                Patient.NotifyPropertyChanged("Email");
                Patient.NotifyPropertyChanged("PhoneNumber");
                Patient.MedicalRecord.NotifyPropertyChanged("Height");
                Patient.MedicalRecord.NotifyPropertyChanged("Weight");
                string json = JsonConvert.SerializeObject(nurseWindow.Patients);
                File.WriteAllText("..\\..\\..\\Data\\patients.json", json);
                Close();
            }
        }
    }
}

