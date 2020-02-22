using System.Collections.Generic;
using SaltEdgeNetCore.Models.Country;

namespace SaltEdgeNetCore.Client
{
    public interface ISaltEdgeClient
    {
        ISaltEdgeClient SetHeaders(IDictionary<string, string> headers);

        ISaltEdgeClient SetBody(object body);
        
        IEnumerable<Country> ListCountriesAsync();
    }
}