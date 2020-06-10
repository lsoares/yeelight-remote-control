using System;
using System.Globalization;
using System.Threading.Tasks;
using YeelightAPI;
using YeelightAPI.Models;

namespace my_lights
{
    class LedLight
    {
        private readonly Device _device;

        public LedLight(Device device) {
            this._device = device;
        }

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
            var isDayLight = (string) await _device.GetProp(PROPERTIES.active_mode) == "0";
            return Int32.Parse((string) await _device.GetProp(isDayLight ? PROPERTIES.bright : PROPERTIES.nl_br));
        }

        public async Task SetBrightness(int value) {
            await Connect();
            await _device.SetBrightness(value);
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
            if (!_device.IsConnected) await _device.Connect();
        }
    }
}