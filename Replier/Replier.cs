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
        }

        public void Receive()
        {

        }
    }
}
