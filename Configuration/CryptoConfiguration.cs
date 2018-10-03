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
        public string FullPath { get; }

        public void Save(string passphrase = null) {
            string jsonString = JsonConvert.SerializeObject(Sections);
            byte[] encrypted;
            RijndaelEncryptionBase encryption = CreateRijndaelAlgorithm(passphrase);
            encrypted = encryption.EncryptStringToBytes(jsonString);

            File.WriteAllBytes(FullPath, encrypted);
        }

        private static RijndaelEncryptionBase CreateRijndaelAlgorithm(string passphrase) {
            return passphrase == null ? (RijndaelEncryptionBase)new RijndaelEncryptionWithComputerInfo() : new RijndaelEncryptionWithPassphrase(passphrase);
        }

        public void Load(string passphrase = null) {
            if (!File.Exists(FullPath)) {
                File.Create(FullPath);
            }
            string encrypted = string.Empty;

            RijndaelEncryptionBase encryption = CreateRijndaelAlgorithm(passphrase);
            encrypted = encryption.DecryptStringFromBytes(File.ReadAllBytes(FullPath));
            
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
