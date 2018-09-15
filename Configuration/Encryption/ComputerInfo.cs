using System;
using System.Collections.Generic;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Configuration.Encryption
{
    public class ComputerInfo {
        public static byte[] GetIndividualKey() {
            string computerIds = CpuId() + BiosId() + BaseId() + MacId();
            byte[] hash = new SHA256CryptoServiceProvider().ComputeHash(new UTF32Encoding().GetBytes(computerIds));
            return hash;
        }

        public static byte[] GetIndividualIV() {
            return GetIndividualKey();
        }

        private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue) {
            string str = "";
            foreach (ManagementObject instance in new ManagementClass(wmiClass).GetInstances()) {
                if (instance[wmiMustBeTrue].ToString() == "True") {
                    if (str == "") {
                        try {
                            str = instance[wmiProperty].ToString();
                            break;
                        } catch {
                        }
                    }
                }
            }
            return str;
        }

        private static string identifier(string wmiClass, string wmiProperty) {
            string str = "";
            foreach (ManagementObject instance in new ManagementClass(wmiClass).GetInstances()) {
                if (str == "") {
                    try {
                        str = instance[wmiProperty].ToString();
                        break;
                    } catch {
                    }
                }
            }
            return str;
        }

        private static string CpuId() {
            string str1 = ComputerInfo.identifier("Win32_Processor", "UniqueId");
            if (str1 == "") {
                str1 = ComputerInfo.identifier("Win32_Processor", "ProcessorId");
                if (str1 == "") {
                    string str2 = ComputerInfo.identifier("Win32_Processor", "Name");
                    if (str2 == "")
                        str2 = ComputerInfo.identifier("Win32_Processor", "Manufacturer");
                    str1 = str2 + ComputerInfo.identifier("Win32_Processor", "MaxClockSpeed");
                }
            }
            return str1;
        }

        private static string BiosId() {
            return ComputerInfo.identifier("Win32_BIOS", "Manufacturer") + ComputerInfo.identifier("Win32_BIOS", "SMBIOSBIOSVersion") + ComputerInfo.identifier("Win32_BIOS", "IdentificationCode") + ComputerInfo.identifier("Win32_BIOS", "SerialNumber") + ComputerInfo.identifier("Win32_BIOS", "ReleaseDate") + ComputerInfo.identifier("Win32_BIOS", "Version");
        }

        private static string DiskId() {
            return ComputerInfo.identifier("Win32_DiskDrive", "Model") + ComputerInfo.identifier("Win32_DiskDrive", "Manufacturer") + ComputerInfo.identifier("Win32_DiskDrive", "Signature") + ComputerInfo.identifier("Win32_DiskDrive", "TotalHeads");
        }

        private static string BaseId() {
            return ComputerInfo.identifier("Win32_BaseBoard", "Model") + ComputerInfo.identifier("Win32_BaseBoard", "Manufacturer") + ComputerInfo.identifier("Win32_BaseBoard", "Name") + ComputerInfo.identifier("Win32_BaseBoard", "SerialNumber");
        }

        private static string VideoId() {
            return ComputerInfo.identifier("Win32_VideoController", "DriverVersion") + ComputerInfo.identifier("Win32_VideoController", "Name");
        }

        private static string MacId() {
            return ComputerInfo.identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
        }
    }
}
