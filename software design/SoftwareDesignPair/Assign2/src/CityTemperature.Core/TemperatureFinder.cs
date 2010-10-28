using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityTemperature.Core
{
    public enum Reliability { Low, Moderate, High,};

    public class TemperatureFinder
    {
        List<ICityTemperatureProvider> _cityTemperatureproviderList = new List<ICityTemperatureProvider>();

        public TemperatureFinder()
        {
        }

        public int ProviderCount 
        {
            get 
            {
                return _cityTemperatureproviderList.Count;
            }
        }

        public int GetTemperture(int cityID)
        {
            Nullable<int> temp = null;

            foreach (ICityTemperatureProvider provider in _cityTemperatureproviderList)
            {
                provider.TryGetTemperature(cityID, out temp);

                if (temp.HasValue)
                    break;

                continue;
            }

            return temp.Value;
        }

        public void AddProvider(ICityTemperatureProvider provider)
        {
            int index; 

            if (_cityTemperatureproviderList.Contains(provider))
                throw new TemperatureFinderProviderListException();

            index = GetNextInsertIndex(provider);
        
            if(index == -1)
                _cityTemperatureproviderList.Add(provider);
            else
                _cityTemperatureproviderList.Insert(index, provider);
        }

        private int GetNextInsertIndex(ICityTemperatureProvider provider)
        {
            int insertIndex = -1;

            for (int index = 0; index < _cityTemperatureproviderList.Count; index++)
            {
                if (provider.ServiceReliability < _cityTemperatureproviderList[index].ServiceReliability)
                {
                    insertIndex = index;
                }
            }

            return insertIndex;
        }

        public void RemoveProvider(ICityTemperatureProvider provider)
        {
            _cityTemperatureproviderList.Remove(provider);
        }

        public bool ContainsProvider(ICityTemperatureProvider  provider2)
        {
            return true;
        }

        public void RemoveAllProviders()
        {
            if (_cityTemperatureproviderList.Count == 0)
                throw new TemperatureFinderProviderListException();

            _cityTemperatureproviderList.Clear();
        }

        public bool IsProviderListSorted()
        {
            bool isListSortedByServiceReliability = true;

            for (int i = 1; i < _cityTemperatureproviderList.Count; i++)
            {
                if (_cityTemperatureproviderList[i - 1].ServiceReliability > _cityTemperatureproviderList[i].ServiceReliability)
                    return false;
            }

            return isListSortedByServiceReliability;
        }
    }
}
