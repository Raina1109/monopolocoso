using System;

public class TemperatureFinder {
  public String FetchTemperature(int cityID) {
    int temperature = 0;
    bool success = false;
    string provider = null;

    //First find temperature from the cheap provider.
    try {
      temperature = new com.provider1.TemperatureProvider().GetTemperature(cityID);      
      provider = "Provider1";
      success = true;
    } catch(ApplicationException) {
      //Time to try another service      
    }
    
    if (!success) {
      //Let's try a provider who charges now.
      try {
        temperature = new com.provider2.TemperatureFinder().FindTemperature(cityID);      
        provider = "Provider2";
        success = true;
      } catch(ApplicationException) {
        //Time to try another service      
      }      
    }
    
    if (!success) {
      //OK, it's time to use the service that is available.
      try {
        temperature = new com.provider3.QueryTemperature().QueryForTemperature(cityID);      
        provider = "Provider3";
        success = true;
      } catch(ApplicationException) {
        //Time to try another service      
      }      
    }
    
    return string.Format("Temperature (as reported by {0}) is {1}", provider, temperature);
  }
}
