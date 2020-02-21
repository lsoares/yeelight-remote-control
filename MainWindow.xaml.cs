using System;
using System.Windows;

namespace my_lights
{
    public partial class MainWindow : Window
    {
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked");
        }
    }
}