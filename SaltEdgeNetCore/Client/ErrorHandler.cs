using System.Runtime.InteropServices;
using SaltEdgeNetCore.SaltEdgeExceptions;

namespace SaltEdgeNetCore.Client
{
    public static class ErrorHandler
    {
        public static void Handle(string errorClass, string message)
        {
            switch (errorClass)
            {
                case "AccountNotFound":
                    throw new AccountNotFoundException(message);

                case "ActionNotAllowed":
                    throw new ActionNotAllowedException(message);

                case "AllAccountsExcluded":
                    throw new AllAccountsExcludedException(message);

                case "ApiKeyNotFound":
                    throw new ApiKeyNotFoundException(message);

                case "AppIdNotProvided":
                    throw new AppIdNotProvidedException(message);

                case "AttemptNotFound":
                    throw new AttemptNotFoundException(message);

                case "BackgroundFetchLimitExceeded":
                    throw new BackgroundFetchLimitExceededException(message);

                case "BatchSizeLimitExceeded":
                    throw new BatchSizeLimitExceededException(message);

                case "CategorizationLimitReached":
                    throw new CategorizationLimitReachedException(message);

                case "ClientDisabled":
                    throw new ClientDisabledException(message);

                case "ClientNotFound":
                    throw new ClientNotFoundException(message);

                case "ClientPending":
                    throw new ClientPendingException(message);

                case "ClientRestricted":
                    throw new ClientRestrictedException(message);

                case "ConnectionFailed":
                    throw new ConnectionFailedException(message);

                case "ConnectionLost":
                    throw new ConnectionLostException(message);

                case "CountryNotFound":
                    throw new CountryNotFoundException(message);

                case "CustomerNotFound":
                    throw new CustomerNotFoundException(message);

                case "CustomerLocked":
                    throw new CustomerLockedException(message);

                case "CredentialsNotMatch":
                    throw new CredentialsNotMatchException(message);

                case "CustomFieldsSizeTooBig":
                    throw new CustomFieldsSizeTooBigException(message);

                case "CustomFieldsFormatInvalid":
                    throw new CustomFieldsFormatInvalidException(message);

                case "DateFormatInvalid":
                    throw new DateFormatInvalidException(message);

                case "DateOutOfRange":
                    throw new DateOutOfRangeException(message);

                case "DateTimeFormatInvalid":
                    throw new DateTimeFormatInvalidException(message);

                case "DateTimeOutOfRange":
                    throw new DateTimeOutOfRangeException(message);

                case "DuplicatedCustomer":
                    throw new DuplicatedCustomerException(message);

                case "EmailInvalid":
                    throw new EmailInvalidException(message);

                case "ExecutionTimeout":
                    throw new ExecutionTimeoutException(message);

                case "ExpiresAtInvalid":
                    throw new ExpiresAtInvalidException(message);

                case "FetchingTimeout":
                    throw new FetchingTimeoutException(message);

                case "FetchScopesNotAllowed":
                    throw new FetchScopesNotAllowedException(message);

                case "FetchScopesInvalid":
                    throw new FetchScopesInvalidException(message);

                case "FileError":
                    throw new FileErrorException(message);

                case "FileNotProvided":
                    throw new FileNotProvidedException(message);

                case "FileNotSaved":
                    throw new FileNotSavedException(message);

                case "HolderInfoNotSupported":
                    throw new HolderInfoNotSupportedException(message);

                case "IdentifierInvalid":
                    throw new IdentifierInvalidException(message);

                case "InteractiveAdapterTimeout":
                    throw new InteractiveAdapterTimeoutException(message);

                case "InteractiveTimeout":
                    throw new InteractiveTimeoutException(message);

                case "InternalServerError":
                    throw new InternalServerErrorException(message);

                case "InvalidCredentials":
                    throw new InvalidCredentialsException(message);

                case "InvalidEncoding":
                    throw new InvalidEncodingException(message);

                case "InvalidFromDate":
                    throw new InvalidFromDateException(message);

                case "InvalidInteractiveCredentials":
                    throw new InvalidInteractiveCredentialsException(message);

                case "InvalidToDate":
                    throw new InvalidToDateException(message);

                case "JsonParseError":
                    throw new JsonParseErrorException(message);

                case "ConnectionAlreadyProcessing":
                    throw new ConnectionAlreadyProcessingException(message);

                case "ConnectionAlreadyAuthorized":
                    throw new ConnectionAlreadyAuthorizedException(message);

                case "ConnectionCannotBeRefreshed":
                    throw new ConnectionCannotBeRefreshedException(message);

                case "ConnectionDisabled":
                    throw new ConnectionDisabledException(message);

                case "ConnectionDuplicated":
                    throw new ConnectionDuplicatedException(message);

                case "ConnectionFetchingStopped":
                    throw new ConnectionFetchingStoppedException(message);

                case "ConnectionLimitReached":
                    throw new ConnectionLimitReachedException(message);

                case "ConnectionNotFound":
                    throw new ConnectionNotFoundException(message);

                case "MissingExpiresAt":
                    throw new MissingExpiresAtException(message);

                case "MissingSignature":
                    throw new MissingSignatureException(message);

                case "ProviderAccessNotGranted":
                    throw new ProviderAccessNotGrantedException(message);

                case "ProviderDisabled":
                    throw new ProviderDisabledException(message);

                case "ProviderError":
                    throw new ProviderErrorException(message);

                case "ProviderInactive":
                    throw new ProviderInactiveException(message);

                case "ProviderNotFound":
                    throw new ProviderNotFoundException(message);

                case "ProviderKeyFound":
                    throw new ProviderKeyFoundException(message);

                case "ProviderNotInteractive":
                    throw new ProviderNotInteractiveException(message);

                case "ProviderUnavailable":
                    throw new ProviderUnavailableException(message);

                case "PublicKeyNotProvided":
                    throw new PublicKeyNotProvidedException(message);

                case "RateLimitExceeded":
                    throw new RateLimitExceededException(message);

                case "RequestExpired":
                    throw new RequestExpiredException(message);

                case "ReturnURLInvalid":
                    throw new ReturnURLInvalidException(message);

                case "ReturnURLTooLong":
                    throw new ReturnURLTooLongException(message);

                case "RouteNotFound":
                    throw new RouteNotFoundException(message);

                case "SecretNotProvided":
                    throw new SecretNotProvidedException(message);

                case "SignatureNotMatch":
                    throw new SignatureNotMatchException(message);

                case "TooManyRequests":
                    throw new TooManyRequestsException(message);

                case "TransactionNotFound":
                    throw new TransactionNotFoundException(message);

                case "ValueOutOfRange":
                    throw new ValueOutOfRangeException(message);

                case "WrongClientToken":
                    throw new WrongClientTokenException(message);

                case "WrongProviderMode":
                    throw new WrongProviderModeException(message);

                case "WrongRequestFormat":
                    throw new WrongRequestFormatException(message);

                case "DateOutOfAispConsentRange":
                    throw new DateOutOfAispConsentRangeException(message);

                case "AispConsentScopesInvalid":
                    throw new AispConsentScopesInvalidException(message);

                case "AispConsentScopesNotAllowed":
                    throw new AispConsentScopesNotAllowedException(message);

                case "InvalidAispConsentFromDate":
                    throw new InvalidAispConsentFromDateException(message);

                case "InvalidAispConsentPeriod":
                    throw new InvalidAispConsentPeriodException(message);

                case "AispConsentAlreadyRevoked":
                    throw new AispConsentAlreadyRevokedException(message);

                case "AispConsentRevoked":
                    throw new AispConsentRevokedException(message);

                case "AispConsentExpired":
                    throw new AispConsentExpiredException(message);

                case "AispConsentNotFound":
                    throw new AispConsentNotFoundException(message);

                default:
                    throw new ExternalException("Salt Edge Error");
            }
        }
    }
}