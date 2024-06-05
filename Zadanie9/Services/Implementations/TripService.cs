using Zadanie9.Data;
using Zadanie9.DTO;
using Zadanie9.Models;
using Zadanie9.Services.Interfaces;
using System;
using System.Linq;

namespace Zadanie9.Services.Implementations
{
    public class TripService : ITripService
    {
        private readonly MasterContext _masterContext;

        public TripService(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }

        public PaginatedTripDTO GetTrips(int pageNumber, int pageSize)
        {
            var totalTrips = _masterContext.Trips.Count();
            var allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

            var trips = _masterContext.Trips
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<TripDTO> tripDTOs = new List<TripDTO>();

            foreach (var trip in trips)
            {
                List<ClientDTO> clients = new List<ClientDTO>();
                foreach (var clientTrip in _masterContext.ClientTrips.Where(ct => ct.IdTrip == trip.IdTrip).ToList())
                {
                    var c = _masterContext.Clients.SingleOrDefault(cl => cl.IdClient == clientTrip.IdClient);
                    clients.Add(new ClientDTO()
                    {
                        FirstName = c.FirstName,
                        LastName = c.LastName
                    });
                }

                List<CountryDTO> countries = new List<CountryDTO>();
                foreach (var countryTrip in _masterContext.CountryTrips.Where(co => co.IdTrip == trip.IdTrip).ToList())
                {
                    var c = _masterContext.Countries.SingleOrDefault(co => co.IdCountry == countryTrip.IdCountry);
                    countries.Add(new CountryDTO()
                    {
                        Name = c.Name
                    });
                }
                tripDTOs.Add(new TripDTO()
                {
                    Name = trip.Name,
                    Description = trip.Description,
                    DateFrom = trip.DateFrom,
                    DateTo = trip.DateTo,
                    MaxPeople = trip.MaxPeople,
                    Countries = countries,
                    Clients = clients,
                });
            }

            return new PaginatedTripDTO()
            {
                PageSize = pageSize,
                AllPages = allPages,
                Trips = tripDTOs
            };
        }

        public bool ClientHasTrips(int idClient)
        {
            return _masterContext.ClientTrips.Any(ct => ct.IdClient == idClient);
        }

        public bool IsClientAssignedToTrip(string pesel, int idTrip)
        {
            var client = _masterContext.Clients.SingleOrDefault(c => c.Pesel == pesel);
            if (client == null)
            {
                return false;
            }

            return _masterContext.ClientTrips.Any(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
        }

        public Trip GetTripById(int idTrip)
        {
            return _masterContext.Trips.SingleOrDefault(t => t.IdTrip == idTrip);
        }

        public bool AssignClientToTrip(int idTrip, ClientAssignmentDTO clientDto, DateTime registeredAt)
        {
            var client = _masterContext.Clients.SingleOrDefault(c => c.Pesel == clientDto.Pesel);
            if (client == null)
            {
                client = new Client
                {
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName,
                    Email = clientDto.Email,
                    Telephone = clientDto.Telephone,
                    Pesel = clientDto.Pesel
                };
                _masterContext.Clients.Add(client);
                _masterContext.SaveChanges();
            }

            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                PaymentDate = clientDto.PaymentDate,
                RegisteredAt = registeredAt
            };

            _masterContext.ClientTrips.Add(clientTrip);
            _masterContext.SaveChanges();

            return true;
        }
    }
}
