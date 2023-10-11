using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCollectionPrototype.Core;
using DataCollectionPrototype.Core.Model;

namespace DataCollectionPrototype.TargetWriting
{
    internal class LibraryApiWriter : ITargetWriter
    {
        public Task WriteAsync(BookModel[] data)
        {
            return Task.CompletedTask;
        }
    }
}
