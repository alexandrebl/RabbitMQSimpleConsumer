using RabbitMQ.Client;
using RabbitMQSimpleConsumer.Library;
using System;
using RabbitMQSimpleConnectionFactory.Entity;
using RabbitMQSimpleConnectionFactory.Library;

namespace RabbitMQSimpleConsumer
{

    /// <summary>
    /// Responsável por gerenciar publicação e o consumo no RabbitMQ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueManager<T> : IQueueManager<T>, IDisposable
    {

        /// <summary>
        /// Canal de comunicação com a fila
        /// </summary>
        private IModel _channel;
        private ChannelFactory _channelFactory;

        /// <summary>
        /// Consumidor de mensagens
        /// </summary>
        public Consumer<T> Consumer { get; set; }

        /// <summary>
        /// Descrição da fila
        /// </summary>
        private readonly string _queueName;


        /// <summary>
        /// Método construtor parametrizado
        /// </summary>
        /// <param name="queueName">Descrição da fila</param>
        public QueueManager(string queueName = null)
        {
            _queueName = queueName;
        }

        /// <summary>
        /// Atribui uma configuração para conexão com RabbitMQ
        /// </summary>
        /// <param name="connectionSetting">Configurações de conexão</param>
        /// <returns>Instância de gerenciamento de fila com uma conexão com RabbitMQ</returns>
        public QueueManager<T> WithConnectionSetting(ConnectionSetting connectionSetting, string clientProvidedName = null)
        {
            this._channelFactory = new ChannelFactory(connectionSetting, clientProvidedName);
            _channel = this._channelFactory.Create();
            return this;
        }

        public QueueManager<T> WithChannelFactory(ChannelFactory channelFactory)
        {
            this._channelFactory = channelFactory;
            _channel = this._channelFactory.Create();
            return this;
        }

        /// <summary>
        /// Atribui um consumidor ao gerenciador
        /// </summary>
        /// <param name="prefetchCount">Controla o número de mensagem recebidas</param>
        /// <param name="autoAck">Indica se a mensagem será removida ou não da fila</param>
        /// <returns>Instância de gerenciamento de fila com um consumidor</returns>
        public QueueManager<T> WithConsumer(ushort prefetchCount = 1, bool autoAck = false)
        {
            if (_queueName == null) throw new Exception($"Queue name is undefined");

            this.Consumer = new Consumer<T>(_channel, _queueName, prefetchCount, autoAck);
            return this;
        }

        public QueueManager<T> WithConsumer(IModel channel, ushort prefetchCount = 1, bool autoAck = false)
        {
            if (_queueName == null) throw new Exception($"Queue name is undefined");

            this.Consumer = new Consumer<T>(channel, _queueName, prefetchCount, autoAck);
            return this;
        }

        public QueueManager<T> WithConsumer(IConnection connection, ushort prefetchCount = 1, bool autoAck = false)
        {
            if (_queueName == null) throw new Exception($"Queue name is undefined");

            var channel = connection.CreateModel();

            this.Consumer = new Consumer<T>(channel, _queueName, prefetchCount, autoAck);
            return this;
        }

        /// <summary>
        /// 'IDisposable' implementation.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 'IDisposable' implementation.
        /// </summary>
        /// <param name="disposeManaged">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            // Return if already disposed.
            if (this._alreadyDisposed) return;

            // Release managed resources if needed.
            if (disposeManaged)
            {
                this.Consumer?.Dispose();
                this._channel?.Dispose();
            }

            this._alreadyDisposed = true;
        }

        /// <summary>
        /// Whether the object was already disposed.
        /// </summary>
        private bool _alreadyDisposed = false;
    }
}