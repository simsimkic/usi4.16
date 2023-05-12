using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientRegistrationWindow.xaml
    /// </summary>
    public partial class PatientRegistrationWindow : Window
    {
        public PatientRegistrationWindow()
        {
            InitializeComponent();
            BirthDatePicker.SelectedDate = new DateTime(2001, 10, 20);
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
            string insuranceNumber = JMBGTextBox.Text;
            string email = EmailTextBox.Text;
            string tel = TelephoneTextBox.Text;
            string address = AddressTextBox.Text;
            string password= PasswordTextBox.Text;
            DateTime birthDate = BirthDatePicker.SelectedDate.Value.Date;
            string illnesses = IllnessesTextBox.Text;

            if (name.Trim() == "" || surname.Trim() == "" || insuranceNumber.Trim() == "" || HeightTextBox.Text.Trim() == "" || WeightTextBox.Text.Trim() == "" || address.Trim() == ""
                || email.Trim() == "" || password.Trim() == "" || tel.Trim() == "" )
            {
                MessageBox.Show("Polja ne smeju biti prazna");
            } else
            {
                double height = Convert.ToDouble(HeightTextBox.Text);
                double weight = Convert.ToDouble(WeightTextBox.Text);
                MessageBox.Show("Uspesna registracija");
                NurseWindow nurseWindow = Application.Current.Windows.OfType<NurseWindow>().FirstOrDefault();
                nurseWindow.Patients.Add(nurseWindow.Nurse.CreatePatientAccount(name, surname, insuranceNumber, height, weight, email, password, tel, address, birthDate));
                string json = JsonConvert.SerializeObject(nurseWindow.Patients);
                File.WriteAllText("..\\..\\..\\Data\\patients.json", json);
                Close();
            }         
        }
    }
}
