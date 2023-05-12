using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace ZdravoCorp
{
    public class TimeSlot : INotifyPropertyChanged
    {
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public TimeSlot(DateTime startTime, TimeSpan duration)
        {
            StartTime = startTime;
            Duration = duration;
        }
        public TimeSlot(string startTime, string duration)
        {
            string dateFormat = "dd/MM/yyyy HH:mm";
            DateTime beginDate = DateTime.ParseExact(startTime, dateFormat, CultureInfo.InvariantCulture);
            TimeSpan span = TimeSpan.FromMinutes(Convert.ToDouble(duration));
            StartTime= beginDate;
            Duration = span;
        }
        public TimeSlot() { }
        public DateTime GetEndTime()
        {
            return StartTime + Duration;
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsOverlapping(TimeSlot other)
        {
            return StartTime < other.GetEndTime() && GetEndTime() > other.StartTime;
        }

    }
}
