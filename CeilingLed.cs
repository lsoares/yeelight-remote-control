using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YeelightAPI;
using YeelightAPI.Models;

// https://www.yeelight.com/download/Yeelight_Inter-Operation_Spec.pdf
// https://github.com/roddone/YeelightAPI
namespace my_lights
{
    public delegate void PowerToggle();

    public class CeilingLed
    {
        private readonly Device _device;
        private const int SmoothMs = 500;

        private CeilingLed(Device device) {
            _device = device;
            Console.WriteLine(JsonConvert.SerializeObject(device));
        }

        public string? Name => _device.Name;
        public string Hostname => _device.Hostname;

        public static async Task Discover(Action<CeilingLed> perLed) {
            (await DeviceLocator.Discover()).ForEach(
                device => perLed(new CeilingLed(device))
            );
        }

        public async Task<bool> IsPowerOn() {
            await Connect();
            return (string) await _device.GetProp(PROPERTIES.power) == "on";
        }

        public async Task<int> GetBrightness() {
            await Connect();
            return Int32.Parse(
                (string) await _device.GetProp(await IsSunLight() ? PROPERTIES.bright : PROPERTIES.nl_br) ?? "50"
            );
        }

        public async Task<bool> IsSunLight() {
            await Connect();
            return (string) await _device.GetProp(PROPERTIES.active_mode) == "0";
        }

        public async Task<bool> IsMoonLight() {
            return (string) await _device.GetProp(PROPERTIES.active_mode) == "1";
        }

        public event PowerToggle? PowerToggled;

        public async Task SetPower(bool power) {
            await Connect();
            await _device.SetPower(power, SmoothMs);
            PowerToggled?.Invoke();
        }

        public async Task SetBrightness(int value) { // 1~100
            await Connect();
            await SetPower(true);
            await _device.SetBrightness(value, SmoothMs);
        }

        public async Task SetTemperature(int value) { // 2700~5700
            await Connect();
            await SetPower(true);
            await _device.SetColorTemperature(value, SmoothMs);
        }

        public async Task<double> GetTemperature() {
            await Connect();
            return Int32.Parse((string) await _device.GetProp(PROPERTIES.ct) ?? "4200");
        }

        public async Task SetSunLight() {
            await Connect();
            await _device.SetPower(true, SmoothMs, PowerOnMode.Ct);
            PowerToggled?.Invoke();
        }

        public async Task SetMoonLight() {
            await Connect();
            await _device.SetPower(true, SmoothMs, PowerOnMode.Night);
            PowerToggled?.Invoke();
        }

        public async Task SetName(String name) {
            await Connect();
            await _device.SetName(name);
            PowerToggled?.Invoke();
        }
        
        public async Task SetDefault() {
            await Connect();
            await _device.SetDefault();
        }

        private async Task Connect() {
            if (!_device.IsConnected) await _device.Connect();
        }
    }
}