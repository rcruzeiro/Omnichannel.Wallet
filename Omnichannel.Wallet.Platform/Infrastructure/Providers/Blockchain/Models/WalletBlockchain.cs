using MongoDB.Bson;

namespace Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain.Models
{
    public class WalletBlockchain : Core.Framework.Blockchain.Blockchain
    {
        public ObjectId _id { get; set; }
    }
}
