using System;

namespace Configuration {
    interface IConfiguration {
       
        string[] GetEntryNames(string sectionName);
        string[] GetSectionNames();
        object GetValue(string sectionName, string entryName);
        bool HasEntry(string sectionName, string entryName);
        bool HasSection(string sectionName);
        void RemoveEntry(string sectionName, string entryName);
        void RemoveSection(string sectionName);
        void SetValue(string sectionName, string entryName, object valueName);
    }
}