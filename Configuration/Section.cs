using System.Collections.Generic;

namespace Configuration {
    public class Section {

        public string Name { get; set; }

        public List<ConfigurationItem> Items { get; } = new List<ConfigurationItem>();
    }
}