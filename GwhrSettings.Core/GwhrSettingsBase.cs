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
        public abstract T Load(string strFileName);

        /// <summary>
        /// Resets the state of the settings to State.Unchanged.  
        /// Override to write the current in-memory settings to the permanent settings location.
        /// </summary>
        public virtual void Save()
        {
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
        }

        protected void SetValue<TSetting>(TSetting objNewValue, [CallerMemberName] string strKey = "")
        {
            GwhrSetting objSetting = this._dicSettings.AddOrUpdate(strKey, (key) =>
            {
                //This is the add value Factory
                return new GwhrSetting()
                {
                    Key = strKey,
                    Value = objNewValue,
                    State = State.Added
                };
            }, (key, objCurrentSetting) =>
            {
                //This is the update value factory
                if (objNewValue.Equals(objCurrentSetting.Value))
                {
                    //The value has not changed.  Return the current setting
                    return objCurrentSetting;
                }
                //The value has changed.  Mark the setting as modified and assign the new value to it.
                objCurrentSetting.State = State.Modified;
                objCurrentSetting.Value = objNewValue;
                return objCurrentSetting;
            });

            if (objSetting.State == State.Added || objSetting.State == State.Modified)
            {
                OnPropertyChanged(strKey);
            }
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
