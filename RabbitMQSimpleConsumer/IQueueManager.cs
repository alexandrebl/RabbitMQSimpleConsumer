using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConsumer.Library;

namespace RabbitMQSimpleConsumer
{
    public interface IQueueManager<T>
    {
        /// <summary>
        /// Consumidor de mensagens
        /// </summary>
        Consumer<T> Consumer { get; set; }

        /// <summary>
        /// Atribui uma configuração para conexão com RabbitMQ
        /// </summary>
        /// <param name="connectionSetting">Configurações de conexão</param>
        /// <returns>Instância de gerenciamento de fila com uma conexão com RabbitMQ</returns>
        QueueManager<T> WithConnectionSetting(ConnectionSetting connectionSetting, string clientProvidedName = null);

        /// <summary>
        /// Atribui um consumidor ao gerenciador
        /// </summary>
        /// <param name="prefetchCount">Controla o número de mensagem recebidas</param>
        /// <param name="autoAck">Indica se a mensagem será removida ou não da fila</param>
        /// <returns>Instância de gerenciamento de fila com um consumidor</returns>
        QueueManager<T> WithConsumer(ushort prefetchCount = 1, bool autoAck = false);
    }
}
