using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;

namespace GameSignalR
{
    public class GameHub : Hub
    {
        public class Command
        {
            public String Name { get; set; }
            public String Params { get; set; }
        }

        public void Hello()
        {
            Clients.All.hello();
        }

        public void GetCommand(Command command)
        {
            Queue(command.Name).AddMessage(new CloudQueueMessage(command.Params));            
        }

        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnection"]);
        private CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

        private CloudQueue Queue(string name)
        {
            var queue = queueClient.GetQueueReference(name.ToLower());
            queue.CreateIfNotExists();
            return queue;
        }


    }
}