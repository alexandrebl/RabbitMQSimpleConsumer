using System;
using System.Collections.Generic;

namespace RabbitMQSimpleConsumer.Library {

    /// <summary>
    /// Responsável por consumo de fila
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConsumer<T> {

        /// <summary>
        /// Evento de recebimento da mensagem
        /// </summary>
        event Action<T, ulong> ReceiveMessage;

        /// <summary>
        /// Evento lança uma exception no recebimento da mensagem da fila
        /// </summary>
        event Action<Exception, ulong> OnReceiveMessageException;

        /// <summary>
        /// Inicia o consumidor e fica aguardando mensagens
        /// </summary>
        void WatchInit(bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null);

        /// <sumary>
        /// Remove a mensagem da fila
        /// </summary>
        /// <param name="deliveryTag"></param>
        void Ack(ulong deliveryTag);

        /// <summary>
        /// Retorna a mensagem da fila
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="requeued"></param>
        void Nack(ulong deliveryTag, bool requeued = true);
    }
}
