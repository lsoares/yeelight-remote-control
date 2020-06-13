using System;
using System.Windows;
using YeelightAPI;

namespace my_lights
{
    public partial class MainWindow
    {
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
    }
}