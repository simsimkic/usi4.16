using System.Collections.Generic;
using System.ComponentModel;

namespace ZdravoCorp
{
    public class MedicalRecord : INotifyPropertyChanged
    {
        public double Height { get; set; } //promena iz int u double
        public double Weight { get; set; } //promena iz int u double
        public List<string> PreviousIllnesses { get; set; }
        public List<string> Allergies { get; set; }
        public List<string> Symptoms { get; set; }
        public Dictionary<Appointment, string> Examinations { get; set; }

        public MedicalRecord()
        {
            PreviousIllnesses = new List<string>();
            Allergies = new List<string>();
            Examinations = new Dictionary<Appointment, string>();
            Symptoms = new List<string>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
