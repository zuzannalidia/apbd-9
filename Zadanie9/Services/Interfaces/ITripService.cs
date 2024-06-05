using Zadanie9.DTO;
using Zadanie9.Models;

namespace Zadanie9.Services.Interfaces
{
    public interface ITripService
    {
        PaginatedTripDTO GetTrips(int pageNumber, int pageSize);
        bool ClientHasTrips(int idClient);
        bool IsClientAssignedToTrip(string pesel, int idTrip);
        Trip GetTripById(int idTrip);
        bool AssignClientToTrip(int idTrip, ClientAssignmentDTO clientDto, DateTime registeredAt);
    }
}