using System.Threading.Tasks;
using Core.Framework.Blockchain;
using Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain.Models;

namespace Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain
{
    public interface IWalletBlockchainService
    {
        Task<WalletBlockchain> GetBlockchain();
        Task AddBlock(Block block);
    }
}
