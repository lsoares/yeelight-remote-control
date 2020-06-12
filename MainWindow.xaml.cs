using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace my_lights
{
    public partial class MainWindow
    {
        private readonly Yeelights _yee = new Yeelights();

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            SplMain.Children.Clear();
            _yee.discover((o, args) => Dispatcher?.Invoke(() => AddDevice(new LedLight(args.Device))));
        }

        private void AddDevice(LedLight led) {
            SplMain.Children.Add(new LedControl(led));
        }
    }
}