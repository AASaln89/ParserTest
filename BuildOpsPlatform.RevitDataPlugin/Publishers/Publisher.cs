using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildOpsPlatform.RevitDataPlugin.Publishers
{
    public abstract class Publisher : IPublisher
    {
        protected string Topic;
        protected string Host;
        protected int? Port;

        protected Publisher(string topic, string host, int? port) 
        { 
            Topic = topic;
            Host = host;
            Port = port;
        }

        public abstract void Publish<T>(T data);
    }
}
