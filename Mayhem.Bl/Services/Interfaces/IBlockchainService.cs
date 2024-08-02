using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IBlockchainService
    {
        Task<bool> VerifyWalletWithSignedMessageAsync(string wallet, string messageToSign, string signedMessage);
    }
}
