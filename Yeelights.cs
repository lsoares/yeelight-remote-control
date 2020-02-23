using YeelightAPI;

namespace my_lights
{
    public class Yeelights
    {
        public void discover(DeviceLocator.DeviceFoundEventHandler deviceFound) {
            DeviceLocator.OnDeviceFound += deviceFound;
            DeviceLocator.Discover();
        }
    }
}