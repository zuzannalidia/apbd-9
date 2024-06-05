using Zadanie9.Models;

namespace Zadanie9.Services.Interfaces
{
    public interface IClientService
    {
        bool DeleteClient(int idClient);
        Client GetClientByPesel(string pesel); 
    }
}