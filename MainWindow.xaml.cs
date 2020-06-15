using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace my_lights
{
    public partial class MainWindow
    {
        // TODO set .exe icon
        // TODO disable controls for a second or avoid SetPower every time
        // TODO double click also works
        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            await CeilingLed.Discover(led => MainContent.Children.Add(new CeilingLedControl(led)));
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

        private async void TurnOffAll(object sender, RoutedEventArgs e) {
            await CeilingLed.Discover(async led => await led.SetPower(false));
        }

        private async void TurnOnAll(object sender, RoutedEventArgs e) {
            await CeilingLed.Discover(async led => {
                await led.SetSunLight();
                await led.SetBrightness(0);
            });
        }
        
        private async void TurnOnAllMoon(object sender, RoutedEventArgs e) {
            await CeilingLed.Discover(async led => {
                await led.SetMoonLight();
                await led.SetBrightness(50);
            });
        }

        private void About(object sender, RoutedEventArgs e) {
            Process.Start(
                new ProcessStartInfo("cmd", "/c start https://github.com/lsoares/yeelight-remote-control")
                    {CreateNoWindow = true}
            );
        }
    }
}