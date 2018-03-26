using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace Requestor
{
    public class Requestor
    {
        private MessageQueue requestQueue;
        private MessageQueue replyQueue;

        public Requestor(string requestQueueName, string replyQueueName)
        {
            if (!MessageQueue.Exists(requestQueueName))
                requestQueue = MessageQueue.Create(requestQueueName);
            else
                requestQueue = new MessageQueue(requestQueueName);

            if (!MessageQueue.Exists(replyQueueName))
                replyQueue = MessageQueue.Create(replyQueueName);
            else
                replyQueue = new MessageQueue(replyQueueName);

            replyQueue.MessageReadPropertyFilter.SetAll();
            replyQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
        }

        public void Send(string messageString)
        {
            Message requestMessage = new Message();
            requestMessage.Body = messageString;
            requestMessage.ResponseQueue = replyQueue;
            requestQueue.Send(requestMessage);

            Console.WriteLine("Message sent");
            Console.WriteLine("\tTime: {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
            Console.WriteLine("\tMessage ID: {0}", requestMessage.Id);
            Console.WriteLine("\tCorrel. ID: {0}", requestMessage.CorrelationId);
            Console.WriteLine("\tReply to: {0}", requestMessage.ResponseQueue.Path);
            Console.WriteLine("\tContents: {0}", requestMessage.Body.ToString());
        }

        public void ReceiveSync()
        {
            Message replyMessage = replyQueue.Receive();

            Console.WriteLine("Received reply");
            Console.WriteLine("\tTime: {0}", DateTime.Now.ToString("HH:mm:ss.ffffff"));
            Console.WriteLine("\tMessage ID: {0}", replyMessage.Id);
            Console.WriteLine("\tCorrel. ID: {0}", replyMessage.CorrelationId);
            Console.WriteLine("\tReply to: {0}", "<n/a>");
            Console.WriteLine("\tContents: {0}", replyMessage.Body.ToString());
        }
    }
}
