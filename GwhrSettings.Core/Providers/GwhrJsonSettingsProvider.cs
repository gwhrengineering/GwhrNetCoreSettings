using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace GwhrSettings.Core
{
    public class GwhrJsonSettingsProvider<T> : GwhrSettingsBase<T> where T : GwhrJsonSettingsProvider<T>
    {
        //Internal fields
        private ReaderWriterLockSlim _objFileLock = new ReaderWriterLockSlim();
        private string _strBasePath = string.Empty;

        #region Properties

        //Get the complete file path
        protected string FilePath => Path.Combine(_strBasePath, _strFileName);

        #endregion

        #region Overridden methods

        public override T Build(string strFileName)
        {
            _strFileName = strFileName;

            //Load the file into memory
            if (_blnHasBeenBuilt == false)
            {
                ReadToDictionary();
            }
            _blnHasBeenBuilt = true;
            return (T)this;
        }

        public override void Save()
        {
            base.Save();//Resets all settings state to unchanged

            EnsureFileExists();
            try
            {
                _objFileLock.EnterWriteLock();

                //Clear existing file text
                File.WriteAllText(FilePath, "");

                //Write the dictionary to the file
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(_dicSettings));
            }
            finally
            {
                _objFileLock.ExitWriteLock();
            }
        }

        #endregion

        #region Private methods

        //Ensure the supplied base path and filename point to a valid file
        private void EnsureFileExists()
        {
            if (File.Exists(FilePath))
            {
                return;//throw new Exception($"Invalid file path: {FilePath}");
            }
            //Assumes file does not exist
            try
            {
                _objFileLock.EnterWriteLock();

                //Clear existing file text
                File.WriteAllText(FilePath, "{}");

                //Write the dictionary to the file
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(_dicSettings));
            }
            finally
            {
                _objFileLock.ExitWriteLock();
            }

        }

        //Parses the settings json file to memory
        private void ReadToDictionary()
        {
            EnsureFileExists();
            try
            {
                _objFileLock.EnterReadLock();
                _dicSettings = JsonConvert.DeserializeObject<ConcurrentDictionary<string, GwhrSetting>>(File.ReadAllText(FilePath));
            }
            finally
            {
                _objFileLock.ExitReadLock();
            }
        }

        public T SetBasePath(string strBasePath)
        {
            _strBasePath = strBasePath;
            return (T)this;
        }

        #endregion
    }
}
