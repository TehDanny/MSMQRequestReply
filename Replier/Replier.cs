using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace Replier
{
    class Replier
    {
        private MessageQueue requestQueue;
        private MessageQueue invalidQueue;

        public Replier(string requestQueueName, string invalidQueueName)
        {
            if (!MessageQueue.Exists(invalidQueueName))
                invalidQueue = MessageQueue.Create(invalidQueueName);
            else
                invalidQueue = new MessageQueue(invalidQueueName);

            if (!MessageQueue.Exists(requestQueueName))
                requestQueue = MessageQueue.Create(requestQueueName);
            else
                requestQueue = new MessageQueue(requestQueueName);

            requestQueue.MessageReadPropertyFilter.SetAll();
            requestQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            requestQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnReceiveCompleted);
            requestQueue.BeginReceive();
        }

        public void OnReceiveCompleted(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue requestQueue = (MessageQueue)source;
            Message requestMessage = requestQueue.EndReceive(asyncResult.AsyncResult);

            try
            {
                Console.WriteLine("Received request");
                Console.WriteLine("\tTime: {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
                Console.WriteLine("\tCorrel. ID: {0}", "<n/a>");
                Console.WriteLine("\tReply to: {0}", requestMessage.ResponseQueue.Path);
                Console.WriteLine("\tContents: {0}", requestMessage.Body.ToString());

                string contents = requestMessage.Body.ToString();
                MessageQueue replyQueue = requestMessage.ResponseQueue;
                Message replyMessage = new Message();
                replyMessage.Body = contents;
                replyMessage.CorrelationId = requestMessage.Id;
                replyQueue.Send(replyMessage);

                Console.WriteLine("Sent reply");
                Console.WriteLine("\tTime: {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                Console.WriteLine("\tMessage ID: {0}", replyMessage.Id);
                Console.WriteLine("\tCorrel. ID: {0}", replyMessage.CorrelationId);
                Console.WriteLine("\tReply to: {0}", "<n/a>");
                Console.WriteLine("\tContents: {0}", replyMessage.Body.ToString());
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid message detected");
                Console.WriteLine("\tType: {0}", requestMessage.BodyType);
                Console.WriteLine("\tTime: {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
                Console.WriteLine("\tCorrel. ID: {0}", "<n/a>");
                Console.WriteLine("\tReply to: {0}", "<n/a>");
                requestMessage.CorrelationId = requestMessage.Id;
                invalidQueue.Send(requestMessage);
                Console.WriteLine("Sent to invalid message queue");
                Console.WriteLine("\tType: {0}", requestMessage.BodyType);
                Console.WriteLine("\tTime: {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
                Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
                Console.WriteLine("\tCorrel. ID: {0}", requestMessage.CorrelationId);
                Console.WriteLine("\tReply to: {0}", requestMessage.ResponseQueue.Path);
            }

            requestQueue.BeginReceive();
        }
    }
}
