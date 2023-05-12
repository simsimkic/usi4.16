using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ZdravoCorp
{
    // Singleton class
    public sealed class UserRepository
    {
        private static readonly UserRepository instance = new UserRepository();

        public static UserRepository Instance
        {
            get
            {
                return instance;
            }
        }

        private UserRepository()
        {
            Doctors = new List<Doctor>();
            Patients = new List<Patient>();
            Nurses = new List<Nurse>();
            Managers = new List<Manager>();
        }

        public List<Doctor> Doctors { get; private set; }
        public List<Patient> Patients { get; private set; }
        public List<Nurse> Nurses { get; private set; }
        public List<Manager> Managers { get; private set; }

        public void AddDoctor(Doctor doctor)
        {
            Doctors.Add(doctor);
        }

        public void AddPatient(Patient patient)
        {
            Patients.Add(patient);
        }

        public void AddManager(Manager manager)
        {
            Managers.Add(manager);
        }
        public void RemoveDoctor(Doctor doctor)
        {
            Doctors.Remove(doctor);
        }

        public void RemovePatient(Patient patient)
        {
            Patients.Remove(patient);
        }

        public void AddNurse(Nurse nurse)
        {
            Nurses.Add(nurse);
        }

        public void RemoveNurse(Nurse nurse)
        {
            Nurses.Remove(nurse);
        }
        public void RemoveManager(Manager manager)
        {
            Managers.Remove(manager);
        }

        public void SavePatients()
        {
            string json = JsonConvert.SerializeObject(Patients);
            File.WriteAllText("..\\..\\..\\Data\\patients.json", json);
        }

        public void SaveDoctors()
        {
            string json = JsonConvert.SerializeObject(Doctors);
            File.WriteAllText("..\\..\\..\\Data\\doctors.json", json);
        }

        public List<Doctor> GetDoctorsBy(Specialization specialization)
        {
            List<Doctor> doctors = new List<Doctor>();
            foreach (Doctor doctor in Doctors)
            {
                if(doctor.Specialization == specialization)
                    doctors.Add(doctor);
            }
            return doctors;
        }

        public Doctor GetDoctorBy(string name,string surname)
        {
            Doctor newDoctor = new Doctor();
            foreach (Doctor doctor in Doctors)
            {
                if (doctor.Name == name && doctor.Surname==surname)
                    newDoctor= doctor;
            }
            return newDoctor;
        }

        public Patient GetPatientBy(string name, string surname)
        {
            Patient patient = new Patient();
            foreach (Patient p in Patients)
            {
                if (p.Name == name && p.Surname == surname)
                    patient = p;
            }
            return patient;
        }

        public void ReadUsers()
        {
            // testing purposes
            string json = File.ReadAllText("..\\..\\..\\Data\\patients.json");
            Patients = JsonConvert.DeserializeObject<List<Patient>>(json);
            AddDoctor(new Doctor("Vuk", "Dimitrov", new Credentials("doktor1", "doktor"), Specialization.General));
            AddDoctor(new Doctor("Miodrag", "Lekic", new Credentials("doktor2", "doktor"), Specialization.Cardiology));
            AddNurse(new Nurse("Milica", "Milicevic", new Credentials("mica", "mica")));
            string jsonContent = File.ReadAllText("..\\..\\..\\Data\\managers.json");
            Managers = JsonConvert.DeserializeObject<List<Manager>>(jsonContent) ?? new List<Manager>();    
            AddManager(new Manager("Aleksa", "Janjic", new Credentials("aleksa", "1234")));


        }

    }
}
