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
                    Button button = new Button {Content = args.Device.Hostname.ToString()};
                    splMain.Children.Add(button);
                    button.Click += (sender1, eventArgs) => {
                        Dispatcher?.Invoke(async () => {
                            await args.Device.TurnOff();
                            args.Device.SupportedOperations.ForEach((el) => Console.WriteLine(el));
                        });
                    };
                });
            });
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e) { }
    }
}