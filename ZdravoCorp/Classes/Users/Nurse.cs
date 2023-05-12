using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
    {
        
    public class Nurse : User
        {
            public Nurse(string name, string surname, Credentials credentials) : base(name, surname, credentials) { }
            
            public Patient CreatePatientAccount(string firstName, string lastName, string insuranceNumber,double height, double weight, string email,
                string password,string tel, string address,DateTime birthDate)
            {
                Credentials credentials=new Credentials(email,password);

                Patient patient = new Patient(firstName, lastName, credentials,insuranceNumber);
                patient.Address = address;
                patient.Email = email;
                patient.PhoneNumber = tel;
                patient.BirthDate = birthDate;

                MedicalRecord medicalRecord = new MedicalRecord();
                medicalRecord.Height = height;
                medicalRecord.Weight = weight;
                patient.MedicalRecord = medicalRecord;
                return patient;
            }

        }

        
    }
