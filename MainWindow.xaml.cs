﻿using System;
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

            // on/off
            var title = new Label {Content = led.Name, ToolTip = led.Hostname};
            row.Children.Add(title);
            row.Children.Add(new Separator {Height = 4});
            var content = new StackPanel {Orientation = Orientation.Horizontal};
            row.Children.Add(content);

            var toggleBt = new ToggleButton {
                Content = "⚡",
                Width = 32,
                IsChecked = await led.IsPowerOn(),
            };
            toggleBt.Click += async (sender1, eventArgs) => await led.TogglePower();
            content.Children.Add(toggleBt);
            SplMain.Children.Add(row);

            // increase/decrease brightness
            var brightnessSlider = new Slider {
                Minimum = 0,
                Maximum = 100,
                IsMoveToPointEnabled = true,
                IsSnapToTickEnabled = true,
                TickFrequency = 5,
                Value = await led.GetBrightness(),
                Width = 100,
                ToolTip = "Set brightness",
            };
            brightnessSlider.MouseUp += async (sender, eventArgs) => 
                await led.SetBrightness(Convert.ToInt32(Math.Round(brightnessSlider.Value)));
            content.Children.Add(brightnessSlider);

            // TODO: get initial state & use toggle button
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

            Console.WriteLine("temp: " + await led.GetTemperature());
            // set temp
            var tempSlider = new Slider {
                Minimum = 1700,
                Maximum = 6500,
                IsMoveToPointEnabled = true,
                IsSnapToTickEnabled = true,
                TickFrequency = 5,
                Value = await led.GetTemperature(),
                Width = 100,
                ToolTip = "Set temperature",
            };
            tempSlider.MouseUp += async (sender, eventArgs) =>
                await led.SetTemperature(Convert.ToInt32(Math.Round(tempSlider.Value)));
            content.Children.Add(tempSlider);

            // set default
            // set name
            // set schedule
            // no lights founds warning
        }
    }
}