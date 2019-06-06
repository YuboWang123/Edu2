using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(PushDataServer)))
            {
                Console.WriteLine("---host ready---");
                bool started = false;
                host.Open();
                if (started)
                {
                    while (true)
                    {
                        
                    }
                }

                host.Close();
                host.Abort();

            }
        }
    }
}
