# .Net Core Settings
A settings/config file implementation for .NET core

# Intro
.Net Core does not currently have settings mechanism similar to App.Config and Web.Config.  The ConfigurationBuilder currently can read the application settings, but it cannot write back to the settings.  Also, it seems that the configuration builder is restricted to using JSON files.  

This project provides the ability for stronly typed settings to be saved to any location (database, json file, xml) as long as the settings class inherits from the correct provider.  Currently the only included provider is a JSON provider that allows for the settings to be read and save back to a JSON file.  

# Usage

Create a new class that will hold your settings as POCO properties.  This class must inherit from a settings provider.  To add a setting, just declare a property for it and call the GetValue method in the getter and the SetValue method in the setter.


```cs
public class ApplicationSettings : GwhrJsonSettingsProvider<ApplicationSettings>
    {

        #region Public properties

        public string UpdateEndPoint
        {
            get{return GetValue("http://localhost:1234");}
            set{SetValue(value);}
        }

        public int Timeout
        {
            get{ return GetValue(5); }
            set{ SetValue(value); }
        }

        #endregion

    }
```

Notice how the class inherits from GwhrJsonSettingsProvider<GwhrSettings>.  This is the settings provider.

To save the settings, simply call the save method on your settings class.  

```cs

//Creates a new instance of the settings.  This should be done at application startup.

ApplicationSettings objSettings = new ApplicationSettings().SetBasePath(AppContext.BaseDirectory).Build("AppConfig.json");

//Read a setting value
Debug.WriteLine(objSettings.EndPoint);//Returns "https://localhost:1234";

//Set a setting value
objSettings.EndPoint = "www.gwhrengineering.com";

//Save the settings
objSettings.Save();

```