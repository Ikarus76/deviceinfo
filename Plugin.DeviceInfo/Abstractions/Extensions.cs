﻿using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;


namespace Plugin.DeviceInfo
{
    public static class Extensions
    {
        public static Task<int> ReadPercentage(this IBatteryInfo battery) => battery
            .WhenBatteryPercentageChanged()
            .Take(1)
            .ToTask();


        public static Task<PowerStatus> ReadPowerStatus(this IBatteryInfo battery) => battery
            .WhenPowerStatusChanged()
            .Take(1)
            .ToTask();
    }
}
