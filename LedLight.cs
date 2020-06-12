using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YeelightAPI;
using YeelightAPI.Models;

namespace my_lights
{
    public delegate void PowerToggle();

    public class LedLight
    {
        private readonly Device _device;

        public LedLight(Device device) {
            _device = device;
            Console.WriteLine(JsonConvert.SerializeObject(device));
        }

        public object Name => _device.Name;
        public object Hostname => _device.Hostname;

        public event PowerToggle PowerToggled;

        public async Task TogglePower() {
            await Connect();
            await _device.Toggle();
            PowerToggled?.Invoke();
        }
        
        public async Task<bool> IsPowerOn() {
            await Connect();
            return (string) await _device.GetProp(PROPERTIES.power) == "on";
        }

        public async Task<int> GetBrightness() {
            await Connect();
            return Int32.Parse((string) await _device.GetProp(await IsSunLight() ? PROPERTIES.bright : PROPERTIES.nl_br));
        }

        public async Task<bool> IsSunLight() {
            await Connect();
            return (string) await _device.GetProp(PROPERTIES.active_mode) == "0";
        }
        
        public async Task<bool> IsMoonLight() {
            return (string) await _device.GetProp(PROPERTIES.active_mode) == "1";
        }

        public async Task SetBrightness(int value) {
            await Connect();
            if (!await IsPowerOn()) await TogglePower();
            await _device.SetBrightness(value, 10);
        }

        public async Task SetTemperature(int value) {
            await Connect();
            if (!await IsPowerOn()) await TogglePower();
            await _device.SetColorTemperature(value, 10);
        }

        public async Task<double> GetTemperature() { // 2700~5700
            await Connect();
            return Int32.Parse((string) await _device.GetProp(PROPERTIES.ct));
        }
        
        public async Task SetSunLight() {
            await Connect();
            await _device.SetPower(true, null, PowerOnMode.Ct);
            PowerToggled?.Invoke();
        }

        public async Task SetMoonLight() {
            await Connect();
            await _device.SetPower(true, null, PowerOnMode.Night);
            PowerToggled?.Invoke();
        }

        private async Task Connect() {
            if (!_device.IsConnected) {
                Console.WriteLine("Reconnecting..");
                await _device.Connect();
            }
        }
    }
}