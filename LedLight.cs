using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YeelightAPI;
using YeelightAPI.Models;

namespace my_lights
{
    class LedLight
    {
        private readonly Device _device;

        public LedLight(Device device) {
            _device = device;
            Console.WriteLine(JsonConvert.SerializeObject(device));
        }

        public object Name => _device.Name;
        public object Hostname => _device.Hostname;

        public async Task TogglePower() {
            await Connect();
            await _device.Toggle();
        }

        public async Task<bool> IsPowerOn() {
            await Connect();
            return (string) await _device.GetProp(PROPERTIES.power) == "on";
        }

        public async Task<int> GetBrightness() {
            await Connect();
            return Int32.Parse((string) await _device.GetProp(await IsDayLight() ? PROPERTIES.bright : PROPERTIES.nl_br));
        }

        public async Task<bool> IsDayLight() {
            await Connect();
            return (string) await _device.GetProp(PROPERTIES.active_mode) == "0";
        }
        
        public async Task<bool> IsMoonLight() {
            return (string) await _device.GetProp(PROPERTIES.active_mode) == "1";
        }

        public async Task SetBrightness(int value) {
            await Connect();
            if (!await IsPowerOn()) await TogglePower();
            // send event power toggled
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
        
        public async Task SetDayLight() {
            await Connect();
            await _device.SetPower(true, null, PowerOnMode.Ct);
        }

        public async Task SetMoonLight() {
            await Connect();
            await _device.SetPower(true, null, PowerOnMode.Night);
        }

        private async Task Connect() {
            if (!_device.IsConnected) {
                Console.WriteLine("Reconnecting..");
                await _device.Connect();
            }
        }
    }
}