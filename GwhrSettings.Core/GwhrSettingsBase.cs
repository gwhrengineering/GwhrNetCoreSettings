using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace GwhrSettings.Core
{
    public abstract class GwhrSettingsBase<T> where T : GwhrSettingsBase<T>
    {
        //Internal fields
        protected bool _blnUseLazyLoading = false;
        protected string _strFileName = string.Empty;
        protected bool _blnHasBeenBuilt = false;
        protected ConcurrentDictionary<string, GwhrSetting> _dicSettings = new ConcurrentDictionary<string, GwhrSetting>();//Thread safe dictionary

        #region Public properties



        #endregion

        #region Public methods

        /// <summary>
        /// Loads the entire settings file into memory
        /// </summary>
        /// <returns>The build.</returns>
        /// <param name="strFileName">String file name.</param>
        public abstract T Build(string strFileName);

        /// <summary>
        /// Resets the state of the settings to State.Unchanged.  
        /// Override to write the current in-memory settings to the permanent settings location.
        /// </summary>
        public virtual void Save() {
            foreach (GwhrSetting objSetting in this._dicSettings.Values)
            {
                objSetting.State = State.Unchanged;
            }
        }

        #endregion

        #region Getter and setter methods

        //Gets the settings from the internal dictionary
        protected TSetting GetValue<TSetting>(TSetting objDefaultValue, [CallerMemberName] string strKey = "")
        {
            GwhrSetting objSetting = this._dicSettings.GetOrAdd(strKey, (key) =>
             {
                 return new GwhrSetting()
                 {
                     Key = strKey,
                     Value = objDefaultValue,
                     State = State.Added
                 };
             });
            return (TSetting)objSetting.Value;
            //return (TSetting)this._dicSettings.GetOrAdd(strKey, objDefaultValue);
        }

        protected void SetValue<TSetting>(TSetting objValue, [CallerMemberName] string strKey = "")
        {
            this._dicSettings.AddOrUpdate(strKey, (key) =>
            {
                //This is the addValueFactory
                return new GwhrSetting()
                {
                    Key = strKey,
                    Value = objValue,
                    State = State.Added
                };
            }, (key, oldValue) =>
            {
                //This is the update value factory
                oldValue.State = State.Modified;
                oldValue.Value = objValue;
                return oldValue;
            });

            //this._dicSettings.AddOrUpdate(strKey, objValue, (key, oldValue) =>
            //{
            //    return objValue;
            //});
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
