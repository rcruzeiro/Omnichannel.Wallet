using System;
using Core.Framework.Entities;

namespace Omnichannel.Wallet.Platform.Domain
{
    public abstract class ValueObject : IValueObject
    {
        public int ID { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
