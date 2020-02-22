using System.Collections.Generic;
using SaltEdgeNetCore.Models.Country;

namespace SaltEdgeNetCore.Client
{
    public class SaltEdgeClient: ISaltEdgeClient
    {
        public IEnumerable<Country> ListCountriesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}