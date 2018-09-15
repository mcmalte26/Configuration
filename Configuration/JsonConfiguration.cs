﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Configuration {
    public class JsonConfiguration : Configuration {
        public JsonConfiguration(string fullpath) {
            FullPath = fullpath;
        }

        public JsonConfiguration(string path, string fileName) {
            FullPath = Path.Combine(path, fileName);
        }

        public string FullPath { get; }
        
        public void Save() {
            string json = JsonConvert.SerializeObject(Sections,Formatting.Indented);
            File.WriteAllText(FullPath, json, Encoding.UTF8);
        }

        public void Load() {
            if (!File.Exists(FullPath)) {
                File.Create(FullPath);
            }
                string jsonString = File.ReadAllText(FullPath, Encoding.UTF8);
                List<Section> sections = JsonConvert.DeserializeObject<List<Section>>(jsonString);
            if (sections != null) {
                Sections.Clear();
                Sections.AddRange(sections);
            }else {
                Sections.Clear();
                //throw new FileLoadException("Loading failed because file is empty or does not exists.");
            }
        }

    }
}
