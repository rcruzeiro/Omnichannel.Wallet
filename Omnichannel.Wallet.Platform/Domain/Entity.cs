using System;
using Core.Framework.Entities;

namespace Omnichannel.Wallet.Platform.Domain
{
    public abstract class Entity : IEntity
    {
        public int ID { get; set; }

        public string Company { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
