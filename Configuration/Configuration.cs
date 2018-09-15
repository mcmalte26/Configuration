using System.Collections.Generic;
using System.Linq;

namespace Configuration {
    public class Configuration:IConfiguration {

        private List<Section> _sections = new List<Section>();

        public List<Section> Sections => _sections;

        public string[] GetEntryNames(string sectionName) {
            Section section = GetSection(sectionName);
            return section.Items.Select(item => item.Key).ToArray();
        }

        private Section GetSection(string sectionName) {
            if (HasSection(sectionName)) {
            return _sections.Single(section => section.Name == sectionName);
            }
            return null;
        }

        public string[] GetSectionNames() {
            return _sections.Select(section => section.Name).ToArray();
        }

        public object GetValue(string sectionName, string entryName) {
            Section section = GetSection(sectionName);
            ConfigurationItem item = section.Items.Find(i => i.Key == entryName);
            if (item != null)
                return item.Value;
            return null;
        }

        public bool HasEntry(string sectionName, string entryName) {
            Section section = GetSection(sectionName);
            return section.Items.Exists(item => item.Key == entryName);
        }

        public bool HasSection(string sectionName) {
            return _sections.Exists(section => section.Name == sectionName);
        }

        public void RemoveEntry(string sectionName, string entryName) {
            Section section = GetSection(sectionName);
            ConfigurationItem item =  section.Items.Find(i => i.Key == entryName);
            if (item != null) {
                section.Items.Remove(item);
            }
        }

        public void RemoveSection(string sectionName) {
            if (HasSection(sectionName)) {
                Section section = GetSection(sectionName);
                _sections.Remove(section);
            }
        }

        public void SetValue(string sectionName, string entryName, object valueName) {
            if (!HasSection(sectionName)) {
                _sections.Add(new Section { Name = sectionName, Items = { new ConfigurationItem { Key = entryName, Value = valueName } } });
            }else if(!HasEntry(sectionName, entryName)){
                GetSection(sectionName).Items.Add(new ConfigurationItem { Key = entryName, Value = valueName });
            } else {
                Section section = GetSection(sectionName);
                section.Items.Find(i => i.Key == entryName).Value = valueName;
            }
        }
    }
}