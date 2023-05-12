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

namespace ZdravoCorp.ViewModels.ManagerWindows
{
    /// <summary>
    /// Interaction logic for QuantityDialog.xaml
    /// </summary>
    public partial class QuantityDialog : Window
    {
        public int Quantity;
        public QuantityDialog()
        {
            InitializeComponent();
            Quantity = 0;
        }

        public void Add_Click(object sender, RoutedEventArgs e)
        {

            string inputQuantity = QuantityTextBox.Text;
            try
            {
                 Quantity = Convert.ToInt32(inputQuantity);
                if (Quantity <= 0)
                {
                    MessageBox.Show("Greska: Neispravan unos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Narudzbina je uspesno dodata u korpu");
                this.Close();
            }
            catch(Exception ex)
            {
                var message = ex.ToString();
                MessageBox.Show("Greska: Neispravan unos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
