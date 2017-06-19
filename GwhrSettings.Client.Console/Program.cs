using System;
using GwhrSettings.Core;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace GwhrSettings.ConsoleClient
{
    class Program
    {
        private static Core.GwhrSettings Settings;
        static void Main(string[] args)
        {

            //Load the settings
            Settings = new Core.GwhrSettings();//.SetBasePath("").Build("AppConfig.json");
            Settings.SetBasePath(AppContext.BaseDirectory);
            Settings.Build("AppConfig.json");

            Console.WriteLine("Settings initialized");

            DisplayMenu();

            Settings.Timeout = 10;
            Settings.UpdateEndPoint = "http://www.google.com";
            Settings.Save();

            Console.WriteLine("");

            Console.ReadLine();
        }

        private static void DisplayMenu()
        {
            while (true)
            {
                Console.Clear();
                DisplayBanner();

                string strCommand = Console.ReadLine();

                switch (strCommand)
                {
                    case "1":
                        Console.WriteLine($"Setting Name: Timeout.  Type: int.  Value: {Settings.Timeout}.");
                        Console.WriteLine("Please enter new value followed by the enter key");

                        int intNewValue = 0;
                        if (int.TryParse(Console.ReadLine(), out intNewValue))
                        {
                            Settings.Timeout = intNewValue;
                            Settings.Save();
                            Console.WriteLine("Setting saved.  Please check you AppConfig.json file to verify.  Press enter.");
                        }
                        else
                        {
                            Console.WriteLine("Error paring input type.  Please make sure to input a number.");
                        }
                        Console.ReadLine();
                        break;

                    case "2":
                        Console.WriteLine($"Setting Name: UpdateEndPoint.  Type: string.  Value: {Settings.UpdateEndPoint}.");
                        Console.WriteLine("Please enter new value followed by the enter key");

                        Settings.UpdateEndPoint = Console.ReadLine();
                        Settings.Save();
                        Console.WriteLine("Setting saved.  Please check you AppConfig.json file to verify.  Press enter.");
                        Console.ReadLine();
                        break;

                }
            }

        }

        private static void DisplayBanner()
        {
            Console.WriteLine("Gregory's .NETCore settings project." + Environment.NewLine);

            Console.WriteLine("DISCLAIMER:");
            Console.WriteLine("This console app was thrown together just as a proof of concept and as a result is not designed to be pretty or all that useful.  " +
                              "All the awesome work went into the GwhrSettings.Core dll.  If you want to see how everything works, look into the GwhrSettings.Core project (dll)" + Environment.NewLine);

            Console.WriteLine("INTRODUCTION:");
            Console.WriteLine($"DotNETCore does not currently have a proper settings mechanism." +
                              "It lacks the ConfigurationManager class so currently reading and writing settings " +
                              "from an App.Config or a Web.config file is not easy to do." + Environment.NewLine);

            Console.WriteLine("The mechanism I designed, is both modular (can be extended to use a database instead of a json file), strongly type, and easy to use." +
                              "The logic resides in the GwhrSettings.Core dll and this console application is merely a wrapper around the dll intended to showcase the " +
                              "functionality contained in the dll.  To incorporate the settings project into your project, just reference the dll, and initialize it as " +
                              "I have done in this console client." + Environment.NewLine);

            Console.WriteLine("INSTRUCTIONS:  ");
            Console.WriteLine("To use this application, please select one of the available settings below, by entering the corresponding number, and press enter." +
                              "Then enter the new value of the setting and press enter again to save" + Environment.NewLine);
            Console.WriteLine();
            Console.WriteLine($"1)  Property Name:  Timeout.  Type: int.  Current value: {Settings.Timeout}");
            Console.WriteLine($"2)  Property Name:  UpdateEnPoint.  Type: string.  Current value: {Settings.UpdateEndPoint}");
        }
    }
}
