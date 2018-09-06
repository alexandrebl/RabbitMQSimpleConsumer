using System;

namespace RabbitMQSimpleConsumer.Library {

    [Serializable]
    public class ReceiveMessageException : Exception {
        public string QueueMessage { get; set; }
        public object QueueType { get; set; }

        public ReceiveMessageException()
            : base() { }
    }
}