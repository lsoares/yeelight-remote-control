using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json;
using YeelightAPI;
using YeelightAPI.Models;

namespace my_lights
{
    public partial class MainWindow
    {
        private readonly Yeelights _yee = new Yeelights();

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            splMain.Children.Clear();
            _yee.discover((o, args) => { Dispatcher?.Invoke(() => { AddDevice(args.Device); }); });
        }

        private async void AddDevice(Device device) {
            LedLight led = new LedLight(device);
            var row = new StackPanel {Orientation = Orientation.Vertical};
            splMain.Children.Add(row);
            Console.WriteLine(JsonConvert.SerializeObject(device));

            // on/off
            var title = new Label {Content = device.Name, ToolTip = device.Hostname};
            row.Children.Add(title);
            row.Children.Add(new Separator {Height = 4});
            var content = new StackPanel {Orientation = Orientation.Horizontal};
            row.Children.Add(content);

            var toggleBt = new ToggleButton {
                Content = "⚡",
                Width = 32,
                IsChecked = await led.IsPowerOn(),
            };
            toggleBt.Click += async (sender1, eventArgs) => { await led.TogglePower(); };
            content.Children.Add(toggleBt);

            // increase/decrease brightness
            var brightnessSlider = new Slider {
                Minimum = 1,
                Maximum = 100,
                IsMoveToPointEnabled = true,
                IsSnapToTickEnabled = true,
                TickFrequency = 5,
                Value = await led.GetBrightness(),
                Width = 100,
            };
            brightnessSlider.MouseUp += async (sender, eventArgs) => {
                var value = Convert.ToInt32(Math.Round(brightnessSlider.Value));
                await led.SetBrightness(value);
            };
            content.Children.Add(brightnessSlider);

            // set mode (moonlight/daylight)
            var daylightButton = new Button { 
                Content = "☀",
                Width = 32,
                ToolTip = "Set sun mode",
                Margin = new Thickness(4)
            };
            daylightButton.Click += async (sender, args) => await led.SetDayLight();
            content.Children.Add(daylightButton);
            // TODO smooth if is turned off
            var moonlightButton = new Button {
                Content = "🌙",
                Width = 32,
                ToolTip = "Set moon mode", 
                Margin = new Thickness(4)
            };
            moonlightButton.Click += async (sender, args) => await led.SetMoonLight();
            content.Children.Add(moonlightButton);

            // set temp
            // set default
            // set name
            // no lights founds warning
        }
    }
}