using System.Collections.Generic;

using System.Collections.Generic;

namespace Zadanie9.DTO
{
    public class PaginatedTripDTO
    {
        public int PageSize { get; set; }
        public int AllPages { get; set; }
        public List<TripDTO> Trips { get; set; }
    }
}
