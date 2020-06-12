using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace my_lights
{
    public partial class CeilingLedControl : UserControl
    {
        private readonly CeilingLed _led;

        public CeilingLedControl(CeilingLed led) {
            _led = led;
            InitializeComponent();
        }

        private async void LedControl_OnLoaded(object sender, RoutedEventArgs e) {
            await UpdateState();
            _led.PowerToggled += async () => await UpdateState();

            // TODO set name
        }

        private async Task UpdateState() {
            Label.Content = _led.Name;
            Label.ToolTip = _led.Hostname;
            Power.IsChecked = await _led.IsPowerOn();
            Brightness.Value = await _led.GetBrightness();
            MoonMode.IsChecked = await _led.IsMoonLight();
            SunMode.IsChecked = await _led.IsSunLight();
            Temperature.Value = await _led.GetTemperature();
        }

        private async void PowerButtonClick(object sender, RoutedEventArgs e) {
            await _led.TogglePower();
        }

        private async void Brightness_OnPreviewMouseUp(object sender, MouseButtonEventArgs e) {
            await _led.SetBrightness(Convert.ToInt32(Math.Round(Brightness.Value)));
        }

        private async void MoonMode_OnClick(object sender, RoutedEventArgs e) {
            await _led.SetMoonLight();
        }

        private async void SunMode_OnClick(object sender, RoutedEventArgs e) {
            await _led.SetSunLight();
        }

        private async void Temperature_OnPreviewMouseUp(object sender, MouseButtonEventArgs e) {
            await _led.SetTemperature(Convert.ToInt32(Math.Round(Temperature.Value)));
        }

        private void Configuration_OnClick(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }
    }
}