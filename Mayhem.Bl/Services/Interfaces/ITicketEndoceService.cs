using Mayhem.Dal.Dto.Requests;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface ITicketEndoceService
    {
        AuthorizationDecodedRequest DecodeTicket(string ticket);
    }
}
