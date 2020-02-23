using System;
using System.Collections.Generic;
using SaltEdgeNetCore.Models.Country;
using SaltEdgeNetCore.Models.Customer;
using SaltEdgeNetCore.Models.Provider;
using SaltEdgeNetCore.Models.Responses;

namespace SaltEdgeNetCore.Client
{
    public interface ISaltEdgeClient
    {
        /// <summary>
        /// Set the HTTP request headers
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        ISaltEdgeClient SetHeaders(IDictionary<string, string> headers);

        /// <summary>
        /// The country is represented just as a string. We’re using ISO 3166-1 alpha-2 country codes.
        /// Thus, all the country codes will have exactly two uppercase letters. There are two special cases:
        /// “Other”, encoded as XO
        /// “Fake”, encoded as XF
        /// </summary>
        /// <returns>Returns a list of countries supported by Salt Edge API.</returns>
        IEnumerable<Country> ListCountries();

        /// <summary>
        /// Provider show allows you to inspect the single provider in order to give your users a proper
        /// interface to input their credentials.
        /// </summary>
        /// <param name="providerCode"></param>
        /// <returns>Provider instance</returns>
        Provider ProviderShow(string providerCode);

        /// <summary>
        /// Returns all the providers we operate with. If a provider becomes disabled, it is not included in the list.
        /// </summary>
        /// <param name="fromDate">
        /// filtering providers created or updated starting from this date, defaults to null
        /// <remarks>date format should be "yyyy-mm-dd"</remarks>
        /// </param>
        /// <param name="fromId">the id of the provider which the list starts with, defaults to null</param>
        /// <param name="countryCode">filtering providers by country, defaults to null</param>
        /// <param name="mode">filtering providers by mode, possible values are: oauth, web, api, file</param>
        /// <param name="includeFakeProviders">whether you wish to fetch the fake providers, defaults to false</param>
        /// <param name="includeProviderFields">whether you wish to include all provider fields in the provider objects, defaults to false</param>
        /// <param name="providerKeyOwner">filtering providers by key owner, possible values are: client, saltedge. When value is set as client,
        /// only providers with client-set keys will be returned.</param>
        /// <returns>Salt Edge Response instance</returns>
        Response<IEnumerable<Provider>, Paging> ProvidersList(DateTime? fromDate = null, string fromId = default,
            string countryCode = default,
            string mode = default,
            bool includeFakeProviders = false,
            bool includeProviderFields = false,
            string providerKeyOwner = default);

        /// <summary>
        /// Creates a customer
        /// </summary>
        /// <param name="customerId">a unique identifier of the new customer</param>
        /// <returns>Customer instance</returns>
        Customer CreateCustomer(string customerId);

        /// <summary>
        /// Get a customer
        /// </summary>
        /// <param name="customerId">the id of the customer</param>
        /// <returns>Customer instance</returns>
        Customer CustomerShow(string customerId);

        /// <summary>
        /// List all of your app’s customers.
        /// </summary>
        /// <returns>Salt Edge Response instance</returns>
        Response<IEnumerable<Customer>, Paging> CustomersList();

        /// <summary>
        /// Remove a customer
        /// </summary>
        /// <param name="customerId">the id of the customer</param>
        /// <returns>RemoveCustomer instance</returns>
        RemoveCustomer CustomerRemove(string customerId);

        /// <summary>
        /// Lock a customer and its data 
        /// </summary>
        /// <param name="customerId">the id of the customer</param>
        /// <returns>LockCustomer instance</returns>
        LockCustomer CustomerLock(string customerId);

        /// <summary>
        /// Unlock a customer and its data
        /// </summary>
        /// <param name="customerId">the id of the customer</param>
        /// <returns>UnlockCustomer instance</returns>
        UnlockCustomer CustomerUnlock(string customerId);
    }
}