using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildOpsPlatform.RevitDataPlugin.Publishers
{
    public interface IPublisher
    {
        void Publish<T>(T data);
    }
}
