using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace GwhrSettings.Core
{
    public class GwhrSettings : GwhrSettingsBase
    {
        //Internal fields
        private ReaderWriterLockSlim _objFileLock = new ReaderWriterLockSlim();

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

        #region Public methods

        public override void Build(string strFileName)
        {
            _strFileName = strFileName;

            //Load the file into memory
            if (_blnHasBeenBuilt == false)
            {
                ReadToDictionary();
            }
            _blnHasBeenBuilt = true;
        }

        public override void Save()
        {
            WriteToFile();
        }

        public override void Load(string strFileName)
        {
            throw new NotImplementedException("This method is not currently supported by this implementation");
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
                _dicSettings = JsonConvert.DeserializeObject<ConcurrentDictionary<string, object>>(File.ReadAllText(FilePath));
            }
            finally
            {
                _objFileLock.ExitReadLock();
            }
        }

        //Writes the in-memory dictionary to disk
        private void WriteToFile()
        {
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


    }
}