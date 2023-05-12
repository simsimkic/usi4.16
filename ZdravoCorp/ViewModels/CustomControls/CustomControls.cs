using System.Globalization;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System;

namespace ZdravoCorp.ViewModels
{
    public class PatientListViewItem : ListViewItem
    {
        public static readonly DependencyProperty PatientProperty =
            DependencyProperty.Register("Patient", typeof(Patient), typeof(PatientListViewItem), new PropertyMetadata(null));

        public Patient Patient
        {
            get { return (Patient)GetValue(PatientProperty); }
            set { SetValue(PatientProperty, value); }
        }
    }

    public class CustomDatePicker : DatePicker
    {
        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        public static readonly DependencyProperty WatermarkTextProperty =
                DependencyProperty.Register("WatermarkText", typeof(string), typeof(CustomDatePicker));

        public override void OnApplyTemplate()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            base.OnApplyTemplate();
            if (base.GetTemplateChild("PART_TextBox") is not DatePickerTextBox box) return;
            box.ApplyTemplate();
            if (box.Template.FindName("PART_Watermark", box) is not ContentControl watermark) return;
            watermark.Content = WatermarkText;
        }

        public static int[] ExtractTime(string timeString)
        {
            try
            {
                string[] time = timeString.Split(":");
                if (!int.TryParse(time[0], out int hours) ||
                    !int.TryParse(time[1], out int minutes))
                {
                    MessageBox.Show("Vreme mora biti u formatu hh:mm!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return Array.Empty<int>();
                }
                if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59)
                {
                    MessageBox.Show("Pogrešno vreme!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return Array.Empty<int>(); ;
                }
                return new int[] { hours, minutes };
            }
            catch (Exception) { return Array.Empty<int>(); }
        }

        public static DateTime FormDateTime(int[] time, DateTime date)
        {
            int hours = time[0];
            int minutes = time[1];
            date = new DateTime(date.Year, date.Month, date.Day, hours, minutes, 0);
            return date;
        }

    }

}

