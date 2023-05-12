using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ZdravoCorp.ViewModels.PatientWindows;
using ZdravoCorp.ViewModels.ManagerWindows;
using ZdravoCorp.Classes.Users;
using ZdravoCorp.ViewModels.DoctorWindows;

namespace ZdravoCorp
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            UserRepository.Instance.ReadUsers();
            InventoryRepository.Instance.LoadInventory();
            RoomRepository.Instance.LoadRooms();
            TransferRepository.Instance.LoadTransfers();
            Schedule.Instance.ReadSchedule();
            new OrderRequest().IsDayPassedFromOrderDate();
            Archive.Instance.ReadArchive();
        }

        public bool LogInDoctor()
        {
            foreach (Doctor doctor in UserRepository.Instance.Doctors)
            {
                if (doctor.Credentials.Username == UsernameBox.Text)
                {
                    if (doctor.Credentials.IsPasswordCorrect(PasswordBox.Password))
                    {
                        DoctorWindow doctorWindow = new DoctorWindow(doctor);
                        doctorWindow.Show();
                        Close();
                        return true;
                    }
                    MessageBox.Show("Pogrešna šifra! Probajte opet.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            return false;
        }

        public bool LogInPatient()
        {
            foreach (Patient patient in UserRepository.Instance.Patients)
            {
                if (patient.Credentials.Username == UsernameBox.Text)
                {
                    if (patient.Credentials.IsPasswordCorrect(PasswordBox.Password))
                    {
                        List<ArchiveRecord> records;
                        string json1 = File.ReadAllText("..\\..\\..\\Data\\archive.json");
                        records = JsonConvert.DeserializeObject<List<ArchiveRecord>>(json1);
                        if (records == null)
                        {
                            records = new List<ArchiveRecord>();
                        }
                        if (patient.overLimit(records) == true)
                        {
                            MessageBox.Show("Pacijent je blokiran");
                        }
                        else
                        {
                            PatientWindow patientWindow = new PatientWindow(patient);
                            patientWindow.Show();
                            Close();
                        }
                        return true;
                    }
                    MessageBox.Show("Pogrešna šifra! Probajte opet.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            return false;
        }

        public bool LogInNurse()
        {
            foreach (Nurse nurse in UserRepository.Instance.Nurses)
            {
                if (nurse.Credentials.Username == UsernameBox.Text)
                {
                    if (nurse.Credentials.IsPasswordCorrect(PasswordBox.Password))
                    {
                        NurseWindow nurseWindow = new NurseWindow(nurse);
                        nurseWindow.Show();
                        Close();
                        return true;
                    }
                    MessageBox.Show("Pogrešna šifra! Probajte opet.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            return false;
        }

        public bool LogInManager()
        {
            UserRepository.Instance.Managers.Add(new Manager("1", "2", new Credentials("admin", "admin")));
            foreach (Manager manager in UserRepository.Instance.Managers)
            {
                if (manager.Credentials.Username == UsernameBox.Text)
                {
                    if (manager.Credentials.IsPasswordCorrect(PasswordBox.Password))
                    {
                        ManagerOverview managerOverview = new ManagerOverview();
                        this.Hide();
                        managerOverview.ShowDialog();
                        this.Show();
                        return true;
                    }
                    MessageBox.Show("Pogrešna šifra! Probajte opet.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return false;
        }
        private void LogIn(object sender, RoutedEventArgs e)
        {
            if (UsernameBox.Text == "")
            {
                MessageBox.Show("Unesite korisničko ime!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PasswordBox.Password == "")
            {
                MessageBox.Show("Unesite šifru!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (LogInDoctor()) return;

            if (LogInPatient()) return;

            if (LogInNurse()) return;

            if (LogInManager()) return;

            MessageBox.Show("Uneto korisničko ime ne postoji! Probajte opet.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void PasswordBoxOnChange(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "") PasswordOverlay.Visibility = Visibility.Visible;
            else PasswordOverlay.Visibility = Visibility.Collapsed;
        }

    }
}
