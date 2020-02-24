using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace my_lights
{
    public partial class MainWindow : Window
    {
        private readonly Yeelights _yee = new Yeelights();

        public MainWindow() {
            _yee.discover((o, args) => {
                Dispatcher?.Invoke(async () => {
                    var row = new StackPanel();
                    splMain.Children.Add(row);
                    // on/off
                    var device = args.Device;
                    if (!device.IsConnected) await device.Connect();

                    Console.WriteLine(JsonConvert.SerializeObject(device));
                    Button toggleBt = new Button {Content = device.Hostname + "(" + device.Name + ")"};
                    toggleBt.Click += async (sender1, eventArgs) => { await device.Toggle(); };
                    row.Children.Add(toggleBt);
                    // increase/decrease brightness
                    Slider brightnessSlider = new Slider {
                        Minimum = 0, Maximum = 100, IsMoveToPointEnabled = true, IsSnapToTickEnabled = true,
                        TickFrequency = 10
                    };
                    brightnessSlider.ValueChanged += async (sender, eventArgs) => {
                        var value = Convert.ToInt32(Math.Round(eventArgs.NewValue));
                        await device.SetBrightness(value);
                    };
                    row.Children.Add(brightnessSlider);
                    // set mode

                    // set temp
                });
            });
        }

        private void btnAddMore_Click(object sender, RoutedEventArgs e) { }
    }
}