using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Dynamo.Models;

namespace Dynamo
{
    /// <summary>
    /// PersistentSettings is a class for GUI to persist certain settings.
    /// Upon running of the GUI, those settings that are persistent will be loaded
    /// from a XML file from DYNAMO_SETTINGS_FILE.
    /// When GUI is closed, the settings into the XML file.
    /// </summary>
    public class PersistentSettings
    {
        const string DYNAMO_SETTINGS_DIRECTORY = @"Autodesk\Dynamo\";
        const string DYNAMO_SETTINGS_FILE = "DynamoSettings.xml";

        // Variables of the settings that will be persistent
        public bool ShowConsole { get; set; }
        public bool ShowConnector { get; set; }
        public ConnectorType ConnectorType { get; set; }

        public PersistentSettings()
        {
            // Default Settings
            this.ShowConsole = false;
            this.ShowConnector = true;
            this.ConnectorType = ConnectorType.BEZIER;
        }

        /// <summary>
        /// Save PersistentSetting in XML File Path if possible,
        /// else return false
        /// </summary>
        /// <param name="filePath">Path of the XML File</param>
        /// <returns>Whether file is saved or error occurred.</returns>
        public bool Save(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PersistentSettings));
                FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                serializer.Serialize(fs, this);
                fs.Close(); // Release file lock
                return true;
            }
            catch (Exception) { }
            
            return false;
        }
        /// <summary>
        /// Save PersistentSetting in a default directory when no path is specified
        /// </summary>
        /// <returns>Whether file is saved or error occurred.</returns>
        public bool Save()
        {
            // Get Default Path
            string path = GetSettingsFilePath();
            return Save(path);
        }

        /// <summary>
        /// Return PersistentSetting from XML path if possible,
        /// else return PersistentSetting with default values
        /// </summary>
        /// <param name="filePath">Path of the XML File</param>
        /// <returns>
        /// Stored PersistentSettings from xml file or
        /// Default PersistentSettings if xml file is not found.
        /// </returns>
        public static PersistentSettings Load(string filePath)
        {
            PersistentSettings settings = new PersistentSettings();
            
            if (string.IsNullOrEmpty(filePath) || (!File.Exists(filePath)))
                return settings;
            
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PersistentSettings));
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                settings = serializer.Deserialize(fs) as PersistentSettings;
                fs.Close(); // Release file lock
            }
            catch (Exception) { }
            
            return settings;
        }
        /// <summary>
        /// Return PersistentSetting from Default XML path
        /// </summary>
        /// <returns>
        /// Stored PersistentSettings from default xml file or
        /// Default PersistentSettings if default xml file is not found.
        /// </returns>
        public static PersistentSettings Load()
        {
            // Get Default Path
            string path = GetSettingsFilePath();
            return Load(path);
        }

        /// <summary>
        /// Return PersistentSetting Default XML File Path if possible
        /// </summary>
        public static string GetSettingsFilePath()
        {
            try
            {
                string appDataFolder = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.ApplicationData);

                appDataFolder = Path.Combine(appDataFolder, DYNAMO_SETTINGS_DIRECTORY);
                
                if (Directory.Exists(appDataFolder) == false)
                    Directory.CreateDirectory(appDataFolder);
                
                return (Path.Combine(appDataFolder, DYNAMO_SETTINGS_FILE));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}