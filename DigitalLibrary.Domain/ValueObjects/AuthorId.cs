using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalLibrary.Domain.ValueObjects
{
    public readonly record struct AuthorId(Guid Value)
    {
        public static AuthorId New() => new AuthorId(Guid.NewGuid());
        public static AuthorId From(Guid value) => new(value);
    }
}
