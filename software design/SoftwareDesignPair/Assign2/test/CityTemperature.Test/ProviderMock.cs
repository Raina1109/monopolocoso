using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityTemperature.Core;

namespace CityTemperature.Test
{
    class ProviderMock : ICityTemperatureProvider
    {
        string _providerName;
        Reliability _reliabilityLevel;

        public ProviderMock(string name, Reliability reliabilityLevel)
        {
            _providerName = name;
            _reliabilityLevel = reliabilityLevel;
        }

        public int? TryGetTemperature(int cityID, out int? temperature)
        {
            temperature = 75;

            return 75;
        }

        public string Name
        {
            get
            {
                return _providerName;
            }
        }

        public Reliability ServiceReliability
        {
            get
            {
                return _reliabilityLevel;
            }

        }

        public override bool Equals(object other)
        {
            if (other is ICityTemperatureProvider)
            {
                return (((ICityTemperatureProvider)other).Name == Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
