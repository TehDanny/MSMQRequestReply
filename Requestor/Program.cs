﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Threading;

namespace Requestor
{
    class Program
    {
        static void Main(string[] args)
        {
            Program myProgram = new Program();
            myProgram.Run();
        }

        private void Run()
        {
            string requestQueueName = @".\private$\requestQueue";
            string replyQueueName = @".\private$\replyQueue";

            Requestor requestor = new Requestor(requestQueueName, replyQueueName);

            string userInput = "";

            while (userInput.ToLower() != "quit")
            {
                Console.Write("Request message: ");
                userInput = Console.ReadLine();
                if (userInput.ToLower() == "quit")
                    break;

                requestor.Send(userInput);

                requestor.ReceiveSync();
            }
        }
    }
}
