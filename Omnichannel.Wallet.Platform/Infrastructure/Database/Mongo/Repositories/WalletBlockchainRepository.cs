using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain.Models;

namespace Omnichannel.Wallet.Platform.Infrastructure.Database.Mongo.Repositories
{
    public class WalletBlockchainRepository : BaseRepository<WalletBlockchain>, IWalletBlockchainRepository
    {
        public WalletBlockchainRepository(string connstring)
            : base(connstring, "wallet", "blockchains")
        { }

        public async Task<WalletBlockchain> GetBlockchain()
        {
            try
            {
                return await Collection.Find(_ => true)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Create(WalletBlockchain blockchain)
        {
            try
            {
                await Collection.InsertOneAsync(blockchain);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<bool> Update(WalletBlockchain blockchain)
        {
            try
            {
                ReplaceOneResult updateResult =
                    await Collection.ReplaceOneAsync(
                        wbc => wbc._id == blockchain._id, blockchain);
                return updateResult.IsAcknowledged &&
                    updateResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
