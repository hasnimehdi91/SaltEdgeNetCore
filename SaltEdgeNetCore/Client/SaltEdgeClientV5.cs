using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using SaltEdgeNetCore.Client.Endpoints;
using SaltEdgeNetCore.Models.Country;
using SaltEdgeNetCore.Models.Error;
using SaltEdgeNetCore.Models.Responses;
using SaltEdgeNetCore.SaltEdgeExceptions;

namespace SaltEdgeNetCore.Client
{
    public class SaltEdgeClientV5 : ISaltEdgeClient
    {
        private IDictionary<string, string> _headers;

        public SaltEdgeClientV5()
        {
            _headers = new Dictionary<string, string>();
        }

        public ISaltEdgeClient SetHeaders(IDictionary<string, string> headers)
        {
            _headers = headers;
            return this;
        }

        public ISaltEdgeClient SetBody(object body)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Country> ListCountriesAsync()
        {
            var apiResponse = GetClient()
                .Get<SimpleResponse<IEnumerable<Country>>>(new RestRequest(SaltEdgeEndpointsV5.CountryList.Value));
            if (apiResponse.IsSuccessful)
            {
                return apiResponse.Data.Data;
            }

            HandleError(apiResponse.Content);
            return null;
        }

        private static void HandleError(string content)
        {
            try
            {
                var error = JsonConvert.DeserializeObject<SaltEdgeError>(content);
            }
            catch (Exception)
            {
                Console.WriteLine();
                throw new SaltEdgeMappingException("Unable to map error");
            }
        }

        private IRestClient GetClient()
        {
            _headers.Add("Content-Type", "application/json");

            var client = new RestClient(" https://www.saltedge.com/api/v5/");
            foreach (var (key, value) in _headers)
            {
                client.AddDefaultHeader(key, value);
            }

            client.UseNewtonsoftJson();
            return client;
        }
    }
}