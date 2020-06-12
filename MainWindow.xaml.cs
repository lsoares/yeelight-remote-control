using System.Windows;

namespace my_lights
{
    public partial class MainWindow
    {
        private readonly Yeelights _yee = new Yeelights();

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            _yee.discover((o, args) =>
                Dispatcher?.Invoke(() =>
                    SplMain.Children.Add(new CeilingLedControl(new CeilingLed(args.Device)))
                )
            );
        }
    }
}