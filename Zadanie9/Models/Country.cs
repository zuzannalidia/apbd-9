using System;
using System.Collections.Generic;

namespace Zadanie9.Models;

public partial class Country
{
    public int IdCountry { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CountryTrip> IdCountryTrips { get; set; } = new List<CountryTrip>();
}
