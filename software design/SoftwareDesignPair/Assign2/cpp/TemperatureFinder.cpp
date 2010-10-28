#include <iostream>
#include <stdlib.h>
#include "lib/TemperatureProvider.h"
#include "lib/TemperatureFinder.h"
#include "lib/QueryTemperature.h"

using namespace std;

class TemperatureFinder {
  public: 
  
  const char* fetchTemperature(int cityID) {
    int temperature = 0;
    bool success = false;
    int provider = 0;

    //First find temperature from the cheap provider.
    try {
      provider1::TemperatureProvider theProvider;
      temperature = theProvider.getTemperature(cityID);      
      provider = 1;
      success = true;
    } catch(...) {
      //Time to try another service      
    }
     
    if (!success) {
      //Let's try a provider who charges now.
      try {
        provider2::TemperatureFinder theProvider;
        temperature = theProvider.findTemperature(cityID);      
        provider = 2;
        success = true;
      } catch(...) {
        //Time to try another service      
      }      
    }
    
    if (!success) {
      //OK, it's time to use the service that is available.
      try {
        provider3::QueryTemperature theProvider;
        temperature = theProvider.queryForTemperature(cityID);      
        provider = 3;
        success = true;
      } catch(...) {
        //Time to try another service      
      }      
    }
    
    char* result = new char[100];
    sprintf(result, "Temperature (as reported by provider%d) is %d", provider, temperature);
    return result;

    return "yo";
  }
};

int main() {
  TemperatureFinder finder;
  cout << finder.fetchTemperature(1) << endl;
  return 0;
}
