using GwhrSettings.Core;

namespace GwhrSettings.ConsoleClient
{
    public class ApplicationSettings : GwhrJsonSettingsProvider<ApplicationSettings>
    {

        #region Public properties

        public string UpdateEndPoint
        {
            get { return GetValue("http://localhost:1234"); }
            set { SetValue(value); }
        }

        public int Timeout
        {
            get { return GetValue(5); }
            set { SetValue(value); }
        }

        #endregion

    }
}