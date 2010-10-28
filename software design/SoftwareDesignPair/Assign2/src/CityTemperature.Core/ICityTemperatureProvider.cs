using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityTemperature.Core
{
    public interface ICityTemperatureProvider
    {
        string Name { get; }
        Reliability ServiceReliability { get; }
        int? TryGetTemperature(int cityID, out Nullable<int> temperature);
        bool Equals(object other);
    }
}
