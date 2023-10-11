using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCollectionPrototype.Core;

namespace DataCollectionPrototype.TargetWriting
{
    internal class LibraryApiWriter : ITargetWriter
    {
        public Task WriteAsync(object[] data)
        {
            return Task.CompletedTask;
        }
    }
}
