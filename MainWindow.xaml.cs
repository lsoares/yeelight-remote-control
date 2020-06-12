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

        private async void AddDevice(LedLight led) {
            var row = new StackPanel {Orientation = Orientation.Vertical};
            var content = new StackPanel {Orientation = Orientation.Horizontal};
            row.Children.Add(content);

            // title
            var title = new Label {Content = led.Name, ToolTip = led.Hostname};
            row.Children.Add(title);
            row.Children.Add(new Separator {Height = 2, Margin = new Thickness(4)});

            // on/off
            var toggleBt = new ToggleButton {
                Content = "⚡",
                Width = 32,
                IsChecked = await led.IsPowerOn(),
            };
            led.PowerToggled += async() => toggleBt.IsChecked = await led.IsPowerOn();  
            toggleBt.Click += async (sender1, eventArgs) => await led.TogglePower();
            content.Children.Add(toggleBt);
            SplMain.Children.Add(row);

            // increase/decrease brightness
            var brightnessSlider = new Slider {
                Minimum = 1,
                Maximum = 100,
                IsMoveToPointEnabled = true,
                IsSnapToTickEnabled = true,
                TickFrequency = 5,
                Value = await led.GetBrightness(),
                Width = 120,
                ToolTip = "Set brightness",
            };
            brightnessSlider.PreviewMouseUp += async (sender, eventArgs) =>
                await led.SetBrightness(Convert.ToInt32(Math.Round(brightnessSlider.Value)));
            led.PowerToggled += async() => brightnessSlider.Value = await led.GetBrightness();
            content.Children.Add(brightnessSlider);

            // set mode (moonlight/daylight)
            // TODO smooth if is turned off
            var moonlightButton = new ToggleButton {
                Content = "🌙",
                Width = 32,
                ToolTip = "Set moon mode",
                Margin = new Thickness(4),
                IsChecked = await led.IsMoonLight(),
            };
            moonlightButton.Click += async (sender, args) => await led.SetMoonLight();
            led.PowerToggled += async() => moonlightButton.IsChecked = await led.IsMoonLight();
            content.Children.Add(moonlightButton);
            var daylightButton = new ToggleButton {
                Content = "☀",
                Width = 32,
                ToolTip = "Set sun mode",
                Margin = new Thickness(4),
                IsChecked = await led.IsDayLight(),
            };
            daylightButton.Click += async (sender, args) => await led.SetDayLight();
            led.PowerToggled += async() => daylightButton.IsChecked = await led.IsDayLight();
            content.Children.Add(daylightButton);

            // set temp
            var tempSlider = new Slider {
                Minimum = 2700,
                Maximum = 5700,
                IsMoveToPointEnabled = true,
                IsSnapToTickEnabled = true,
                TickFrequency = 20,
                Value = await led.GetTemperature(),
                Width = 80,
                ToolTip = "Set temperature",
            };
            tempSlider.PreviewMouseUp += async (sender, eventArgs) =>
                await led.SetTemperature(Convert.ToInt32(Math.Round(tempSlider.Value)));
            led.PowerToggled += async() => tempSlider.Value = await led.GetTemperature();
            content.Children.Add(tempSlider);

            // settings
            var configButton = new Button {
                Content = "🛠️",
                Width = 32,
                ToolTip = "Setup",
                Margin = new Thickness(4),
            };
            // configButton.Click += async (sender, args) =>
            content.Children.Add(configButton);


            // TODO set name
            // set default
            // set schedule
            // no lights founds warning

            // TODO: update UI automatically
            // TODO: update UI when UI control is used
        }
    }
}