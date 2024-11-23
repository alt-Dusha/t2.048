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

namespace t2._048
{
    /// <summary>
    /// Логика взаимодействия для Gift.xaml
    /// </summary>
    public partial class Gift : Window
    {
        public Gift()
        {
            InitializeComponent();
        }
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] prize = {"Акция №1","Акция №2","Акция №3","ПРОМОКОД№1",
                "ПРОМОКОД№2","ПРОМОКОД№3" };

            Random random = new Random();
            MessageBox.Show(prize[random.Next(6) - 1]);

        }

    }
}
