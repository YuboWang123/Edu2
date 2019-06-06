
using Fleck;
using Fleck.Events;
using Fleck.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edu.Service
{
    class Program
    {
       
        static void Main(string[] args)
        {
            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("ws://0.0.0.0:8881");

            FileProcessor fp=new FileProcessor("");
            Task.Run(()=>fp.FileMonitor());

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };

                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };

                ///user request 
                socket.OnMessage = message =>
                {
                    //socket.ShowingId = message;

                    ////start feeding.
                    //fp.NewFileCome += socket.OnFileHasCome;

                    Console.WriteLine("get message from socket " + socket.ConnectionInfo.ClientIpAddress+":"+socket.ConnectionInfo.ClientPort+" and msg is:"+ message);

                };

            });

            

            

            var input = Console.ReadLine();
            while (input != "exit")
            {
                foreach (var socket in allSockets.ToList())
                {
                    socket.Send(input);
                }
                input = Console.ReadLine();
            }
        }
    }
}
