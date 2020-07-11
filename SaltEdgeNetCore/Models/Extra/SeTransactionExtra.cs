using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SaltEdgeNetCore.Models.Extra
{
    public class SeTransactionExtra
    {
        [JsonProperty("account_balance_snapshot")]
        public decimal? AccountBalanceSnapshot { get; set; }
        
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }
        
        [JsonProperty("additional")]
        public string Additional { get; set; }
        
        [JsonProperty("asset_amount")]
        public decimal? AssetAmount { get; set; }

        [JsonProperty("asset_code")]
        public string AssetCode { get; set; }

        [JsonProperty("categorization_confidence")]
        public decimal? CategorizationConfidence { get; set; }

        [JsonProperty("check_number")]
        public string CheckNumber { get; set; }

        [JsonProperty("closing_balance")]
        public decimal? ClosingBalance { get; set; }

        [JsonProperty("constant_code")]
        public string ConstantCode { get; set; }

        [JsonProperty("convert")]
        public bool? Convert { get; set; }

        [JsonProperty("customer_category_code")]
        public string CustomerCategoryCode { get; set; }

        [JsonProperty("customer_category_name")]
        public string CustomerCategoryName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("information")]
        public string Information { get; set; }

        [JsonProperty("mcc")]
        public string Mcc { get; set; }

        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }

        [JsonProperty("opening_balance")]
        public decimal? OpeningBalance { get; set; }

        [JsonProperty("installment_debt_amount")]
        public decimal? InstallmentDebtAmount { get; set; }

        [JsonProperty("original_amount")]
        public decimal? OriginalAmount { get; set; }

        [JsonProperty("original_category")]
        public string OriginalCategory { get; set; }

        [JsonProperty("original_currency_code")]
        public string OriginalCurrencyCode { get; set; }

        [JsonProperty("original_subcategory")]
        public string OriginalSubcategory { get; set; }

        [JsonProperty("payee")]
        public string Payee { get; set; }

        [JsonProperty("payee_information")]
        public string PayeeInformation { get; set; }

        [JsonProperty("payer")]
        public string Payer { get; set; }

        [JsonProperty("payer_information")]
        public string PayerInformation { get; set; }

        [JsonProperty("possible_duplicate")]
        public bool? PossibleDuplicate { get; set; }

        [JsonProperty("posting_date")]
        public DateTime? PostingDate { get; set; }

        [JsonProperty("posting_time")]
        public TimeSpan? PostingTime { get; set; }

        [JsonProperty("record_number")]
        public string RecordNumber { get; set; }

        [JsonProperty("specific_code")]
        public string SpecificCode { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<string> Tags { get; set; }

        [JsonProperty("time")]
        public TimeSpan? Time { get; set; }

        [JsonProperty("transfer_account_name")]
        public string TransferAccountName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("unit_price")]
        public decimal? UnitPrice { get; set; }

        [JsonProperty("units")]
        public decimal? Units { get; set; }

        [JsonProperty("variable_code")]
        public string VariableCode { get; set; }
    }
}