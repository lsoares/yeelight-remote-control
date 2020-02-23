using System;
using System.Windows;
using System.Windows.Controls;

namespace my_lights
{
    public partial class MainWindow : Window
    {
        private readonly Yeelights _yee = new Yeelights();

        public MainWindow() {
            _yee.discover((o, args) => {
                Dispatcher?.Invoke(() => {
                    var device = args.Device;
                    Button button = new Button {Content = device.Hostname};
                    splMain.Children.Add(button);
                    button.Click += async (sender1, eventArgs) => {
                        await device.Connect();
                        await device.Toggle();
                        device.Disconnect();
                    };
                });
            });
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e) { }
    }
}