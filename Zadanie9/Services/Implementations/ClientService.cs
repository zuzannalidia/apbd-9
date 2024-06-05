using Zadanie9.Data;
using Zadanie9.Models;
using Zadanie9.Services.Interfaces;
using System.Linq;

namespace Zadanie9.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly MasterContext _masterContext;

        public ClientService(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }

        public bool DeleteClient(int idClient)
        {
            var client = _masterContext.Clients.SingleOrDefault(c => c.IdClient == idClient);

            if (client == null)
            {
                return false;
            }

            _masterContext.Clients.Remove(client);
            _masterContext.SaveChanges();
            return true;
        }

        public Client GetClientByPesel(string pesel)
        {
            return _masterContext.Clients.SingleOrDefault(c => c.Pesel == pesel);
        }
    }
}