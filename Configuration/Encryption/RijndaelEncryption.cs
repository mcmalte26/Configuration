using Configuration.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Configuration {

    public abstract class RijndaelEncryptionBase {
        
        protected abstract byte[] Key { get; }
        protected abstract byte[] IV { get; }
        protected virtual int KeySize { get; }

        protected RijndaelEncryptionBase() {
            KeySize = 256;
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
                rijAlg.BlockSize = KeySize;
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
                rijAlg.BlockSize = KeySize;
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

    public class RijndaelEncryptionWithComputerInfo: RijndaelEncryptionBase {

        protected override byte[] Key { get; }
        protected override byte[] IV { get; }


        public RijndaelEncryptionWithComputerInfo() {
            Key = ComputerInfo.GetIndividualKey();
            IV = ComputerInfo.GetIndividualIV();
        }
      
    }

    public class RijndaelEncryptionWithPassphrase : RijndaelEncryptionBase {
        private const string IV_String = "Frünz jügt üm kümplütt vürwührlüsten Tüxü qür düch Büyürn.";

        public RijndaelEncryptionWithPassphrase(string passphrase) {
            Key = new SHA256CryptoServiceProvider().ComputeHash(new UTF32Encoding().GetBytes(passphrase));
        }

        protected override byte[] Key { get; }

        protected override byte[] IV => new SHA256CryptoServiceProvider().ComputeHash(new UTF32Encoding().GetBytes(IV_String));
    }
}
