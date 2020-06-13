using System;
using System.Windows;
using System.Windows.Input;
using YeelightAPI;

namespace my_lights
{
    public partial class MainWindow
    {
        // TODO turn on/off all
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            Dispatcher?.Invoke(async () =>
                (await DeviceLocator.Discover()).ForEach(
                    device => MainContent.Children.Add(new CeilingLedControl(new CeilingLed(device)))
                )
            );
        }

        private void Exit(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
    }
}