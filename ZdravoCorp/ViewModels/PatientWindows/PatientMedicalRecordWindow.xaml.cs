﻿using System;
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

namespace ZdravoCorp.ViewModels.PatientWindows
{
    /// <summary>
    /// Interaction logic for PatientMedicalRecordWindow.xaml
    /// </summary>
    public partial class PatientMedicalRecordWindow : Window
    {
        public Patient Patient { get; set; }
        PatientMedicalRecordWindowViewModel PatientMedicalRecordWindowViewModel { get; set; }
        public PatientMedicalRecordWindow(Patient patient)
        {
            InitializeComponent();
            this.Patient = patient;
            PatientMedicalRecordWindowViewModel = new PatientMedicalRecordWindowViewModel(Patient);
            this.DataContext = PatientMedicalRecordWindowViewModel;
        }

        private void SortColumnOnHeaderClick(object sender, RoutedEventArgs e)
        {
            this.PatientMedicalRecordWindowViewModel.SortColumnOnHeaderClick(sender, e);
        }

        private void KeywordSearchClick(object sender, RoutedEventArgs e)
        {
            this.PatientMedicalRecordWindowViewModel.KeywordSearch();
        }
    }
}
