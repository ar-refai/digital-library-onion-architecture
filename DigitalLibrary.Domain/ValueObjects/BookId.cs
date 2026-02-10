using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalLibrary.Domain.ValueObjects
{
    public readonly record struct BookId(Guid Value)
    {
        public static BookId New() => new BookId(Guid.NewGuid());
        public static BookId From(Guid value) => new(value);
    }
}
