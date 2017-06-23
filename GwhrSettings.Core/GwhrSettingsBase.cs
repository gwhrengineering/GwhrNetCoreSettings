using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace GwhrSettings.Core
{
    public abstract class GwhrSettingsBase//<T>
    {
        //Internal fields
        protected string _strBasePath = string.Empty;
        protected bool _blnUseLazyLoading = false;
        protected string _strFileName = string.Empty;
        protected bool _blnHasBeenBuilt = false;
        protected ConcurrentDictionary<string, object> _dicSettings = new ConcurrentDictionary<string, object>();//Thread safe dictionary

		#region Public properties

		//Get the complete file path
		protected string FilePath
		{
			get
			{
				return Path.Combine(_strBasePath, _strFileName);
			}
		}

        #endregion

        #region Public methods

        /// <summary>
        /// Sets the settings file base path excluding the file name
        /// </summary>
        /// <param name="strBasePath">String base path.</param>
        //public virtual T SetBasePath(string strBasePath)
        //{
        //    _strBasePath = strBasePath;
        //    return (T)this;
        //}

        //Loads all the settings into memory
        /// <summary>
        /// Loads the entire settings file into memory
        /// </summary>
        /// <returns>The build.</returns>
        /// <param name="strFileName">String file name.</param>
        //public abstract T Build(string strFileName);



        /// <summary>
        /// Writes the current in-memory settings to the setttings file.
        /// </summary>
        public abstract void Save();

        #endregion

        #region Getter and setter methods

        //Gets the settings from the internal dictionary
        protected TSetting GetValue<TSetting>(TSetting objDefaultValue, [CallerMemberName] string strKey = "")
        {
            
            return (TSetting)this._dicSettings.GetOrAdd(strKey, objDefaultValue);
        }

        protected void SetValue<TSetting>(TSetting objValue, [CallerMemberName] string strKey = "")
        {
            this._dicSettings.AddOrUpdate(strKey, objValue, (key,oldValue)=> { 
                return objValue; });
            OnPropertyChanged(strKey);
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName()] string strPropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        #endregion
    }
}
