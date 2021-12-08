using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Confluent.Kafka;

namespace Molder.Kafka.Models
{
    [ExcludeFromCodeCoverage]
    public class KafkaSettings
    {
        private KafkaSettings() {}

        private static Lazy<Dictionary<string, Settings>> _settings = new(() => new Dictionary<string, Settings>());
        public static Dictionary<string, Settings> Settings
        {
            get => _settings.Value;
            set
            {
                _settings = new Lazy<Dictionary<string, Settings>>(() => value);
            }
        }
    }

    public class Settings
    {
        public ConsumerConfig Config { get; set; }
        public string Topic { get; set; }
        public string Name { get; set; }
    }
}