using System;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using YeelightAPI;
using YeelightAPI.Models;

namespace my_lights
{
    public partial class MainWindow : Window
    {
        private readonly Yeelights _yee = new Yeelights();

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            Discover();
        }

        private void Discover() {
            splMain.Children.Clear();
            _yee.discover((o, args) => { Dispatcher?.Invoke(() => { AddDevice(args.Device); }); });
        }

        private async void AddDevice(Device device) {
            var row = new StackPanel();
            splMain.Children.Add(row);
            if (!device.IsConnected) await device.Connect();
            Console.WriteLine(JsonConvert.SerializeObject(device));

            // on/off
            var title = new TextBlock {Text = device.Name};
            row.Children.Add(title);
            var toggleBt = new Button {Content = "⚡"};
            toggleBt.Click += async (sender1, eventArgs) => { await device.Toggle(); };
            row.Children.Add(toggleBt);

            // increase/decrease brightness
            var currentBrightness = Convert.ToDouble((string) (await device.GetProp(PROPERTIES.bright)));
            var brightnessSlider = new Slider {
                Minimum = 1, Maximum = 100, IsMoveToPointEnabled = true, IsSnapToTickEnabled = true,
                TickFrequency = 5, Value = currentBrightness
            };
            Console.WriteLine(currentBrightness);
            brightnessSlider.ValueChanged += async (sender, eventArgs) => {
                var value = Convert.ToInt32(Math.Round(eventArgs.NewValue));
                await device.SetBrightness(value);
            };
            row.Children.Add(brightnessSlider);
            // set mode

            // set temp
        }
    }
}