using System;

namespace Lockec.Models
{
    public class Service
    {
        public string service_type { get; set; }
        public string description { get; set; }
        public double rate_base { get; set; }
        public double rate_fraction { get; set; }
        public double rate_base_security { get; set; }
        public double rate_fraction_security { get; set; }
        public double rate_base_vehicle { get; set; }
        public double rate_fraction_vehicle { get; set; }
        public bool personal { get; set; }
        public bool vehiculo { get; set; }
        public bool armamento { get; set; }
        public bool candado_satelital { get; set; }
        public int hour_base { get; set; }

        public double ServicePrice(double duration, bool isGuardDriver, bool rentCar)
        {
            if (service_type.Equals("CHOFER"))
            {
                return DriverPrice(duration, isGuardDriver, rentCar);
            }
            else if (service_type.Equals("TRANSPORTER"))
            {
                return TransporterPrice(duration);
            }
            else if (service_type.Equals("GUARDIA"))
            {
                return GuardPrice(duration);
            }
            else
            {
                return 0;
            }
        }

        private double DriverPrice(double duration, bool isGuardDriver, bool rentCar)
        {
            double carService = 0.0;
            double carTime = 0.0;
            double serviceTypeBase = rate_base;
            double serviceTypeTime = (duration > 3) ? (rate_fraction * (RoundingUpHour(duration) - hour_base)) : 0;

            if (rentCar)
            {
                carService = rate_base_vehicle;
                carTime = (duration > 3) ? (rate_fraction_vehicle * (RoundingUpHour(duration) - hour_base)) : 0;
            }
            if (isGuardDriver)
            {
                serviceTypeBase = rate_base_security;
                serviceTypeTime = (duration > 3) ? (rate_fraction_security * (RoundingUpHour(duration) - hour_base)) : 0;
                return serviceTypeBase + serviceTypeTime + carService + carTime;
            }

            return serviceTypeBase + serviceTypeTime + carService + carTime;
        }

        private double TransporterPrice(double duration)
        {
            
            double serviceTime = (duration > 3) ? (rate_fraction * (RoundingUpHour(duration)-hour_base)) : 0;
            return rate_base + serviceTime;
        }

        private double GuardPrice(double duration)
        {
            double serviceTime = (duration > 3) ? (rate_fraction * (RoundingUpHour(duration) - hour_base)) : 0;
            return rate_base + serviceTime;
        }

        public double RoundingUpHour(double duration)
        {
            return Math.Ceiling(duration);
        }
    }
}
