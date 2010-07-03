using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Content;


namespace TestProject
{
    class FileManager
    {
        private static StorageContainer container;
        private static StorageDevice device;
        private static IAsyncResult result;
        private static bool LoadSettings = true;
        
        private static void DoSaveSettings()
        {
            try
            {
                // Create the data to save.
                FileStream stream = OpenStorageSettings();
                // Convert the object to XML data and put it in the stream.
                XmlSerializer serializer = new XmlSerializer(typeof(InputMappings));
                serializer.Serialize(stream, Input.InputMappings);

                CloseStorage(stream);
            }
            catch { }
        }

        private static void DoLoadSettings()
        {
            try
            {
                FileStream stream = OpenStorageSettings();
                // Convert the object to XML data and put it in the stream.
                if (stream.Length > 0)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(InputMappings));
                    Input.InputMappings = (InputMappings)serializer.Deserialize(stream);
                }
                CloseStorage(stream);
            }
            catch{}
        }

        private static FileStream OpenStorageSettings()
        {

            // Open a storage container.
            container = device.OpenContainer("StarTrooper2DXNA");

            // Get the path of the save game.
            string filename = Path.Combine(container.Path, "StarTrooperControls.sav");

            // Open the file, creating it if necessary.
            FileStream stream = File.Open(filename, FileMode.OpenOrCreate);

            return stream;
        }

        private static void CloseStorage(FileStream stream)
        {
            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();

        }

        private static void SelectStorage()
        {
            // Set the request flag
            if (!Guide.IsVisible)
            {
                device = null;
                result = Guide.BeginShowStorageDeviceSelector(GetDevice, null);
                
            }
        }

        private static void GetDevice(IAsyncResult result)
        {
            device = Guide.EndShowStorageDeviceSelector(result);
            if (device != null && device.IsConnected)
            {
                if (LoadSettings) DoLoadSettings(); else DoSaveSettings();
            }
        }

        public static void LoadKeyMappings()
        {
            LoadSettings = true;
            if (device == null) SelectStorage(); else DoLoadSettings(); 
        }
        public static void SaveKeyMappings()
        {
            LoadSettings = false;
            Input.SettingsSaved = true;
            if (device == null) SelectStorage(); else DoSaveSettings();

        }
    }
}
