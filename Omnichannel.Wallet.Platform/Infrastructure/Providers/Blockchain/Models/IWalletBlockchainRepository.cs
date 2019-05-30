using System.Threading.Tasks;

namespace Omnichannel.Wallet.Platform.Infrastructure.Providers.Blockchain.Models
{
    public interface IWalletBlockchainRepository
    {
        Task<WalletBlockchain> GetBlockchain();
        Task Create(WalletBlockchain blockchain);
        Task<bool> Update(WalletBlockchain blockchain);
    }
}
