﻿using System;
using System.Reactive.Linq;
using Windows.Devices.Power;
using Windows.Foundation;
using Windows.System.Power;


namespace Plugin.DeviceInfo
{
    public class BatteryInfo : IBatteryInfo
    {
        public int Percentage
        {
            get
            {
                var report = Battery.AggregateBattery.GetReport();
                var remain = report.RemainingCapacityInMilliwattHours;
                var full = report.FullChargeCapacityInMilliwattHours;

                if (remain == null || full == null)
                    return -1;

                var value = (int)(remain.Value / (double)full.Value * 100);
                return value;
            }
        }


        public PowerStatus Status
        {
            get
            {
                var report = Battery.AggregateBattery.GetReport();
                switch (report.Status)
                {
                    case BatteryStatus.Charging:
                        return PowerStatus.Charging;

                    case BatteryStatus.Discharging:
                        return PowerStatus.Discharging;

                    case BatteryStatus.Idle:
                        return PowerStatus.Charged;

                    case BatteryStatus.NotPresent:
                        return PowerStatus.NoBattery;

                    default:
                        return PowerStatus.Unknown;
                }
            }
        }


        public IObservable<int> WhenBatteryPercentageChanged()
        {
            return Observable.Create<int>(ob =>
            {
                var handler = new TypedEventHandler<Battery, object>((sender, args) => ob.OnNext(this.Percentage));
                Battery.AggregateBattery.ReportUpdated += handler;
                return () => Battery.AggregateBattery.ReportUpdated -= handler;
            });
        }


        public IObservable<PowerStatus> WhenPowerStatusChanged()
        {
            return Observable.Create<PowerStatus>(ob =>
            {
                var handler = new TypedEventHandler<Battery, object>((sender, args) => ob.OnNext(this.Status));
                Battery.AggregateBattery.ReportUpdated += handler;
                return () => Battery.AggregateBattery.ReportUpdated -= handler;
            });
        }
    }
}
