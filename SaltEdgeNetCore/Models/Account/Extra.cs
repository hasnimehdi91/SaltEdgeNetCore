using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SaltEdgeNetCore.Models.Transaction;

namespace SaltEdgeNetCore.Models.Account
{
    public class Extra
    {
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("assets")]
        public IEnumerable<string> Assets { get; set; }
        
        [JsonProperty("available_amount")]
        public double? AvailableAmount { get; set; }

        [JsonProperty("blocked_amount")]
        public double? BlockedAmount { get; set; }
        
        [JsonProperty("card_type")]
        public string CardType { get; set; }
        
        [JsonProperty("cards")]
        public IEnumerable<string> Cards { get; set; }
        
        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("closing_balance")]
        public double? ClosingBalance { get; set; }
        
        [JsonProperty("credit_limit")]
        public double? CreditLimit { get; set; }

        [JsonProperty("current_date")]
        public DateTime? CurrentDate { get; set; }

        [JsonProperty("current_time")]
        public DateTime CurrentTime { get; set; }
        
        [JsonProperty("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonProperty("iban")]
        public string Iban { get; set; }
        
        [JsonProperty("interest_rate")]
        public double? InterestRate { get; set; }
        
        [JsonProperty("next_payment_amount")]
        public double? NextPaymentAmount { get; set; }
        
        [JsonProperty("next_payment_date")]
        public string NextPaymentDate { get; set; }
        
        [JsonProperty("open_date")]
        public string OpenDate { get; set; }

        [JsonProperty("opening_balance")]
        public double? OpeningBalance { get; set; }

        [JsonProperty("partial")]
        public bool? Partial { get; set; }

        [JsonProperty("sort_code")]
        public string SortCode { get; set; }

        [JsonProperty("statement_cut_date")]
        public DateTime? StatementCutDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("swift")]
        public string Swift { get; set; }

        [JsonProperty("total_payment_amount")]
        public double? TotalPaymentAmount { get; set; }
        
        [JsonProperty("transactions_count")]
        public Count TransactionsCount { get; set; }

        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }

        [JsonProperty("cashback_amount")]
        public double? CashBackAmount { get; set; }

        [JsonProperty("unit_price")]
        public double? UnitPrice { get; set; }

        [JsonProperty("units")]
        public double? Units { get; set; }

        [JsonProperty("indicative_unit_price")]
        public double? IndicativeUnitPrice { get; set; }

        [JsonProperty("interest_income")]
        public double? InterestIncome { get; set; }

        [JsonProperty("interest_amount")]
        public double? InterestAmount { get; set; }

        [JsonProperty("fund_holdings")]
        public dynamic FundHoldings { get; set; }

        [JsonProperty("premium_frequency")]
        public string PremiumFrequency { get; set; }

        [JsonProperty("policy_status")]
        public string PolicyStatus { get; set; }

        [JsonProperty("life_assured_name")]
        public string LifeAssuredName { get; set; }

        [JsonProperty("premium_amount")]
        public double? PremiumAmount { get; set; }

        [JsonProperty("financial_consultant")]
        public string FinancialConsultant { get; set; }

        [JsonProperty("total_reversionary_bonus")]
        public double? TotalReversionaryBonus { get; set; }

        [JsonProperty("gross_surrender")]
        public double? GrossSurrender { get; set; }

        [JsonProperty("guaranteed_gross_surrender")]
        public double? GuaranteedGrossSurrender { get; set; }

        [JsonProperty("reversionary_bonus_cash_value")]
        public double? ReversionaryBonusCashValue { get; set; }

        [JsonProperty("owned_policy_amount")]
        public double? OwnedPolicyAmount { get; set; }

        [JsonProperty("policy_loan_limit")]
        public double? PolicyLoanLimit { get; set; }

        [JsonProperty("policy_converted_to_paid_up")]
        public double? PolicyConvertedToPaidUp { get; set; }

        [JsonProperty("paid_up_conversion_reversionary_bonus")]
        public double? PaidUpConversionReversionaryBonus { get; set; }

        [JsonProperty("policy_components")]
        public dynamic PolicyComponents { get; set; }
        
        [JsonProperty("last_posted_transaction_id")]
        public string LastPostedTransactionId { get; set; }
        
    }
}