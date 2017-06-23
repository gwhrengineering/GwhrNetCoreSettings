using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace GwhrSettings.Core
{
    public class GwhrSettings : GwhrJsonSettingsProvider<GwhrSettings>
    {

        #region Public properties

        public string UpdateEndPoint
        {
            get
            {
                return GetValue("http://localhost:1234");
            }
            set
            {
                SetValue(value);
            }
        }


        public int Timeout
        {
            get
            {
                return GetValue(5);
            }
            set
            {
                SetValue(value);
            }
        }

        #endregion

    }
}