using Configuration.Encryption;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Configuration {
    public class CryptoConfiguration : Configuration {
        public CryptoConfiguration(string fullpath) {
            FullPath = fullpath;
        }

        public CryptoConfiguration(string path, string fileName) {
            FullPath = Path.Combine(path, fileName);
        }

        public string FullPath { get; }

        public void Save() {
            string jsonString = JsonConvert.SerializeObject(Sections);
            byte[] encrypted;
            using (RijndaelManaged rijndael = new RijndaelManaged()) {
                rijndael.KeySize = 256;
                rijndael.BlockSize = 256;
                rijndael.Key = ComputerInfo.GetIndividualKey();
                rijndael.IV = ComputerInfo.GetIndividualIV();
                ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);
                using (MemoryStream memoryStream = new MemoryStream()) {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream)) {
                            streamWriter.Write(jsonString);
                        }
                        encrypted = memoryStream.ToArray();
                    }
                }
            }
            File.WriteAllBytes(FullPath, encrypted);
        }

        public void Load() {
            if (!File.Exists(FullPath)) {
                File.Create(FullPath);
            }
            string encrypted = string.Empty;
            using (RijndaelManaged rijndael = new RijndaelManaged()) {
                rijndael.KeySize = 256;
                rijndael.BlockSize = 256;
                rijndael.Key = ComputerInfo.GetIndividualKey();
                rijndael.IV = ComputerInfo.GetIndividualIV();
                ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);
                using (Stream memoryStream = File.OpenRead(FullPath)) {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {
                        using (StreamReader streamReader = new StreamReader(cryptoStream)) {
                            encrypted = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            JsonSerializerSettings settings = new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All
            };
            List<Section> sections = JsonConvert.DeserializeObject<List<Section>>(encrypted, settings);
            if (sections != null) {
                Sections.Clear();
                Sections.AddRange(sections);
            } else {
                Sections.Clear();
            }
        }

    }
}
