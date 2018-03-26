using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replier
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
            string invalidQueueName = @".\private$\invalidQueue";

            Replier replier = new Replier(requestQueueName, invalidQueueName);

            Console.ReadLine();
        }
    }
}
