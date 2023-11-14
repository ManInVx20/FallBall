using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SystemPath = System.IO.Path;

namespace VinhLB
{
    public static class FileDataHandler
    {
        private static readonly string encryptionCodeWord = "retro";

        public static void Save(GameData data, string fileName, bool useEncryption)
        {
            string basePath = SystemPath.GetFullPath(Application.persistentDataPath);
            string fullPath = SystemPath.Combine(basePath, fileName);

            try
            {
                Directory.CreateDirectory(SystemPath.GetDirectoryName(fullPath));

                string dataToSave = JsonConvert.SerializeObject(data, Formatting.Indented);

                if (useEncryption)
                {
                    dataToSave = EncryptDecrypt(dataToSave);
                }

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToSave);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to save data to file: {fullPath}\n{e}");
            }
        }

        public static GameData Load(string fileName, bool useEncryption)
        {
            GameData loadedData = null;
            string basePath = SystemPath.GetFullPath(Application.persistentDataPath);
            string fullPath = SystemPath.Combine(basePath, fileName);

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = string.Empty;
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }

                    loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error occured when trying to load data to file: {fullPath}\n{e}");
                }
            }

            return loadedData;
        }

        public static void Delete(string fileName)
        {
            string basePath = SystemPath.GetFullPath(Application.persistentDataPath);
            string fullPath = SystemPath.Combine(basePath, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private static string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }

            return modifiedData;
        }
    }
}
