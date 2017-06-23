using System;
namespace GwhrSettings.Core
{
    public interface IGwhrSettings<T>
    {
		/// <summary>
		/// Sets the settings file base path excluding the file name
		/// </summary>
		/// <param name="strBasePath">String base path.</param>
		T SetBasePath(string strBasePath);
		
        /// <summary>
		/// Loads the entire settings file into memory
		/// </summary>
		/// <returns>The build.</returns>
		/// <param name="strEndPoint">String file name.</param>
		T Build(string strEndPoint);

		/// <summary>
		/// Writes the current in-memory settings to the setttings file.
		/// </summary>
		void Save();
    }
}
