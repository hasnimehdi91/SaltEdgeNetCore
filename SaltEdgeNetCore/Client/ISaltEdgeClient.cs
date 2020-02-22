using System.Collections.Generic;
using SaltEdgeNetCore.Models.Country;

namespace SaltEdgeNetCore.Client
{
    public interface ISaltEdgeClient
    {
        IEnumerable<Country> ListCountriesAsync();
    }
}