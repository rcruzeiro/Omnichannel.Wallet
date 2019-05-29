using System;

namespace Omnichannel.Wallet.Platform.Domain.Accounts
{
    public class Transaction : ValueObject
    {
        private int accountId;
        public Account Account { get; private set; }

        public string Location { get; private set; }

        private int operationType;
        public virtual OperationType OperationType => (OperationType)operationType;

        private int eventType;
        public virtual EventType EventType => (EventType)eventType;

        public decimal Value { get; private set; }

        protected Transaction()
        { }

        internal Transaction(Account account, string location, OperationType operationType, EventType eventType, decimal value)
        {
            accountId = account.ID;
            Account = account;
            Location = location;
            Value = value;
            this.operationType = (int)operationType;
            this.eventType = (int)eventType;
            CreatedAt = DateTimeOffset.Now;
        }

        internal Transaction(Account account, OperationType operationType, EventType eventType, decimal value)
            : this(account, null, operationType, eventType, value)
        { }
    }

    public enum OperationType
    {
        Credit = 1,
        Debit
    }

    public enum EventType
    {
        Create = 1,
        Charge,
        Consume
    }
}
