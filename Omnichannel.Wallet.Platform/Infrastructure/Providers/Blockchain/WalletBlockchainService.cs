using System;
using System.Threading.Tasks;
using Core.Framework.Blockchain;
using Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain.Models;

namespace Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain
{
    public class WalletBlockchainService : IWalletBlockchainService
    {
        readonly IWalletBlockchainRepository _repository;

        public WalletBlockchainService(IWalletBlockchainRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<WalletBlockchain> GetBlockchain()
        {
            try
            {
                return await _repository.GetBlockchain();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task AddBlock(Block block)
        {
            try
            {
                var blockchain = await GetBlockchain();

                if (blockchain == null)
                {
                    blockchain = new WalletBlockchain();
                    blockchain.AddBlock(block);
                    await _repository.Create(blockchain);
                }
                else
                {
                    blockchain.AddBlock(block);
                    await _repository.Update(blockchain);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
