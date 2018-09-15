using Configuration.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Configuration {
    public class RijndaelEncryption {
        public byte[] Key { get; }
        public byte[] IV { get; }
        public int KeySize { get; }
        public RijndaelEncryption(int keysize, byte[] key, byte[] iv) {
            KeySize = keysize;
            Key = key;
            IV = iv;
        }

        public RijndaelEncryption() {
            KeySize = 128;
            Key = ComputerInfo.GetIndividualKey();
            IV = ComputerInfo.GetIndividualIV();
        }

        public byte[] EncryptStringToBytes(string decryptedText) {
            if (decryptedText == null || decryptedText.Length <= 0)
                throw new ArgumentNullException("Decrypted text is null");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key is null.");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV is null.");
            byte[] encryptedText;
            using (Rijndael rijAlg = RijndaelManaged.Create()) {
                rijAlg.KeySize = KeySize;
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream()) {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
                            swEncrypt.Write(decryptedText);
                        }
                        encryptedText = msEncrypt.ToArray();
                    }
                }
            }
            return encryptedText;
        }

        public string DecryptStringFromBytes(byte[] encryptedText) {
            if (encryptedText == null || encryptedText.Length <= 0)
                throw new ArgumentNullException("Encrypted Text is null.");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key is null.");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV is null.");
            string decryptedText = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged()) {
                rijAlg.KeySize = KeySize;
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(encryptedText)) {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return decryptedText;
        }
    }
}
