using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Request.Models;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;

namespace Mayhem.Bl.Services.Implementations
{

    public class TicketEndoceService : ITicketEndoceService
    {
        private readonly ILogger<TicketEndoceService> logger;

        public TicketEndoceService(ILogger<TicketEndoceService> logger)
        {
            this.logger = logger;
        }

        public AuthorizationDecodedRequest DecodeTicket(string ticket)
        {
            AuthorizationDecodedRequest result = new();

            try
            {
                string decodedJsonTicket = Base64Decode(ticket);
                SignedFreshData signedFreshData = Newtonsoft.Json.JsonConvert.DeserializeObject<SignedFreshData>(decodedJsonTicket);
                SignedData signedData = Newtonsoft.Json.JsonConvert.DeserializeObject<SignedData>(signedFreshData.Data);

                result.signedData = signedData;
                result.signedFreshData = signedFreshData;
            }
            catch
            {
                AddErrorBadRequest();
            }

            return result;
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void AddErrorBadRequest()//TODO to separate class.
        {
            string errorMessage = $"Bad request.";
            logger.LogInformation(errorMessage);
            throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
        }
    }
}
