using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace my_lights
{
    public partial class CeilingLedControl
    {
        private readonly CeilingLed _led;

        public CeilingLedControl(CeilingLed led) {
            _led = led;
            InitializeComponent();
        }

        private async void LedControl_OnLoaded(object sender, RoutedEventArgs e) {
            await UpdateState();
            _led.PowerToggled += async () => await UpdateState();
        }

        private async Task UpdateState() {
            Label.Content = Coalesce(_led.Name, _led.Hostname);
            Label.ToolTip = _led.Hostname;
            Power.IsChecked = await _led.IsPowerOn();
            Brightness.Value = await _led.GetBrightness();
            MoonMode.IsChecked = await _led.IsMoonLight();
            SunMode.IsChecked = await _led.IsSunLight();
            Temperature.Value = await _led.GetTemperature();
        }
        
        static string? Coalesce(params string?[] strings) => strings.FirstOrDefault(s => !string.IsNullOrEmpty(s));

        private async void PowerButtonClick(object sender, RoutedEventArgs e) {
            await _led.SetPower(Power.IsChecked == true);
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
            LedHostname.Text = _led.Hostname;
            LedName.Text = _led.Name;
            ConfigurationPopup.IsOpen = true;
        }

        private async void Set_Name(object sender, RoutedEventArgs e) {
            await _led.SetName(LedName.Text);
            ConfigurationPopup.IsOpen = false;
        }
        
        private async void Set_default(object sender, RoutedEventArgs e) {
            await _led.SetDefault();
            ConfigurationPopup.IsOpen = false;
        }
    }
}