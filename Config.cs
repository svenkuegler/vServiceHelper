using System;
using System.Collections.Generic;
using System.Xml;

namespace vServiceHelper
{
    class Config
    {
        private List<KeyValuePair<string, List<string>>> serviceGroups = new List<KeyValuePair<string, List<string>>>();

        public Config load(string configFile)
        {
            XmlDocument reader = new XmlDocument();
            reader.Load(configFile);
            
            foreach (XmlNode serviceGroup in reader.SelectNodes("/config/*"))
            {
                var services = new List<string>();
                foreach (XmlNode service in serviceGroup["services"])
                {
                    services.Add(service.InnerText);
                }

                this.serviceGroups.Add(new KeyValuePair<string, List<string>>(serviceGroup["name"].InnerText, services));
            }

            return this;
        }

        public List<KeyValuePair<string, List<string>>> getServiceGroups()
        {
            return this.serviceGroups;
        }
    }
}
