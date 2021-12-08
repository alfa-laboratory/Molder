using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Confluent.Kafka;
using Serilog;

namespace Molder.Kafka.Models
{
    [ExcludeFromCodeCoverage]
    public class KafkaQuery
    {
        private KafkaQuery() {}

        private static Lazy<Dictionary<string, Kafka>> _kafkas = new(() => new Dictionary<string, Kafka>());
        public static Dictionary<string, Kafka> KafkaList
        {
            get => _kafkas.Value;
            set
            {
                _kafkas = new Lazy<Dictionary<string, Kafka>>(() => value);
            }
        }
    }
    
    public class Kafka
    {
        private ConsumerConfig _config;
        private string _topic;
        private string _name;

        public List<string> Messages { get; set; } 
        
        public Kafka(string name, string topic, ConsumerConfig config)
        {
            _name = name;
            _topic = topic;
            _config = config;
            
            Messages = new List<string>();
        }

        public void CreateConsumer()
        {
            var cancellationToken = new CancellationTokenSource();
            cancellationToken.CancelAfter((int)_config.SessionTimeoutMs);

            try
            {
                using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
                {
                    consumer.Subscribe(_topic);
                    try
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                var cr = consumer.Consume(cancellationToken.Token);
                                Messages.Add(cr.Message.Value);
                                Log.Logger.Debug(
                                    $"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");
                            }
                            catch (ConsumeException e)
                            {
                                Log.Logger.Error($"Error occured: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Unsubscribe();
                        consumer.Close();
                        Log.Logger.Information($"Cancellation Requested - consumer Unsubscribe - consumer Close ");
                        
                        if (KafkaQuery.KafkaList.ContainsKey(_name))
                        {
                            KafkaQuery.KafkaList.Remove(_name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Subscribe error {ex}");
            }
        }
    }
}