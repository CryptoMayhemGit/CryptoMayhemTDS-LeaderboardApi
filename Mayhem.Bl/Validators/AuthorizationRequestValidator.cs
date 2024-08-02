using FluentValidation;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Bl.Validators.Base;
using Mayhem.Dal.Dto.Requests;

namespace Mayhem.Bl.Validators
{
    public class AuthorizationRequestValidator : BaseValidator<AuthorizationDecodedRequest>
    {
        public AuthorizationRequestValidator(
            IBlockchainService blockchainService
            )
        {
            Validation(blockchainService);
        }

        private void Validation(IBlockchainService blockchainService)
        {
            VerifyBasicData();
            VerifySignedDataFormat();
            VerifyWalletWithSignedMessage(blockchainService);
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.signedData.Wallet).NotEmpty().WithErrorCode("BAD_REQUEST");
            RuleFor(x => x.signedData.Wallet).NotEmpty().MaximumLength(200).WithErrorCode("BAD_REQUEST");
            RuleFor(x => x.signedFreshData.Signature).NotEmpty().MaximumLength(2000).WithErrorCode("BAD_REQUEST");
            RuleFor(x => x.signedFreshData.Data).NotEmpty().MaximumLength(2000).WithErrorCode("BAD_REQUEST");
        }

        private void VerifySignedDataFormat()
        {
            RuleFor(x => new { x.signedData }).MustAsync(async (request, cancellation) =>
            {
                if (Int64.TryParse(request.signedData.Nonce, out long nonce))
                {
                    try
                    {
                        DateTime dateTime = UnixTimeToDateTime(nonce);
                        if (dateTime.Year <= DateTime.Now.Year && dateTime.Year > 2000)
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }

                return false;

            }).WithErrorCode("BAD_REQUEST");
        }

        private void VerifyWalletWithSignedMessage(IBlockchainService blockchainService)
        {
            RuleFor(x => new { x.signedFreshData, x.signedData, x.isCyberConnect }).MustAsync(async (request, cancellation) =>
            {
                if (request.isCyberConnect)
                {
                    bool result = await blockchainService.VerifyWalletWithSignedMessageAsync(request.signedData.Wallet, request.signedData.Message, request.signedFreshData.Signature);
                    return result;
                }
                else
                {
                    bool result = await blockchainService.VerifyWalletWithSignedMessageAsync(request.signedData.Wallet, request.signedFreshData.Data, request.signedFreshData.Signature);
                    return result;
                }
            }).WithErrorCode("BAD_REQUEST");
        }
    }
}
