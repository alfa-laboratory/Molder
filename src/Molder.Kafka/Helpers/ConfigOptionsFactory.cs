using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Molder.Kafka.Infrastructures;
using Molder.Kafka.Models;

namespace Molder.Kafka.Helpers
{
    public static class ConfigOptionsFactory
    {
        public static IOptions<List<Settings>> Create(IConfiguration configuration)
        {
            var blc = configuration.GetSection(Constants.CONFIG_BLOCK);
            var lst = (from child in blc.GetChildren() 
                let configBlock = child.GetSection(Constants.SETTINGS_BLOCK) 
                let topicBlock = child.GetSection(Constants.TOPIC_BLOCK) 
                let nameBlock = child.GetSection(Constants.NAME_BLOCK) 
                select new Settings {Name = nameBlock.Get<string>(), Topic = topicBlock.Get<string>(), Config = configBlock.Get<ConsumerConfig>()}).ToList();
            return Options.Create(lst);
        }
    }
}