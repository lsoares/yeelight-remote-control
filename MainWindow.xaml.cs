using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using YeelightAPI;

namespace my_lights
{
    public partial class MainWindow
    {
        // TODO turn on/off all
        // TODO refresh on show window
        // TODO set .exe icon
        // TODO disable controls for a second
        // TODO double click also works
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Dispatcher?.Invoke(async () =>
                (await DeviceLocator.Discover()).ForEach(
                    device => MainContent.Children.Add(new CeilingLedControl(new CeilingLed(device)))
                )
            );
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            using StreamWriter sw = File.AppendText("error.log");
            sw.WriteLine(e.ExceptionObject);
        }

        private void Exit(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
    }
}