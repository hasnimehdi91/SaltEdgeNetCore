using System.Collections.Generic;
using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Transaction;

namespace SaltEdgeNetCore.Models.Account
{
    public class Extra
    {
        [JsonProperty("iban")]
        public string Iban { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("available_amount")]
        public double AvailableAmount { get; set; }

        [JsonProperty("transactions_count")]
        public Count TransactionsCount { get; set; }

        [JsonProperty("last_posted_transaction_id")]
        public string LastPostedTransactionId { get; set; }

        [JsonProperty("cards")]
        public IList<string> Cards { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonProperty("credit_limit")]
        public double? CreditLimit { get; set; }

        [JsonProperty("open_date")]
        public string OpenDate { get; set; }

        [JsonProperty("interest_rate")]
        public double? InterestRate { get; set; }

        [JsonProperty("next_payment_date")]
        public string NextPaymentDate { get; set; }

        [JsonProperty("next_payment_amount")]
        public double? NextPaymentAmount { get; set; }

        [JsonProperty("total_payment_amount")]
        public double? TotalPaymentAmount { get; set; }
    }
}