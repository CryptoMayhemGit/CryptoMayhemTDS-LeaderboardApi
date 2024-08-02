using Mayhem.Bl.Services.Interfaces;
using Nethereum.Signer;

namespace Mayhem.Bl.Services.Implementations
{
    public class BlockchainService : IBlockchainService
    {
        public async Task<bool> VerifyWalletWithSignedMessageAsync(string wallet, string messageToSign, string signedMessage)
        {
            EthereumMessageSigner signer = new EthereumMessageSigner();
            string addressRec = signer.EncodeUTF8AndEcRecover(messageToSign, signedMessage);
            return (!wallet.Equals(addressRec, StringComparison.InvariantCultureIgnoreCase)) ? (await Task.FromResult(result: false)) : (await Task.FromResult(result: true));
        }
    }
}
