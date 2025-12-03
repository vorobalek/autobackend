namespace AutoBackend.Sdk;

internal static class Constants
{
    #region Services

    internal const string ClusterDiscoveryServiceUrl = "/__discovery";

    #endregion

    #region Headers

    internal const string XForwardedForHeaderName = "X-Forwarded-For";

    #endregion

    #region Core

    internal const string HostDefaultUrl = "http://+:{0}";

    #endregion

    #region Generic

    internal const string GenericDatabaseSchemaName = "generic";
    private const string GenericInternalsPrefix = "__Generic__";
    private const string IdName = "Id";
    internal const string GenericIdPropertyName = $"{GenericInternalsPrefix}{IdName}";
    private const string GenericAssemblyName = "AutoBackend.Sdk.Runtime";

    internal const string GenericRequestsAssemblyName = $"{GenericAssemblyName}.{GenericRequestsModuleName}";
    internal const string GenericRequestsModuleName = "GenericRequests";
    internal const string GenericResponsesAssemblyName = $"{GenericAssemblyName}.{GenericResponsesModuleName}";
    internal const string GenericResponsesModuleName = "GenericResponses";
    internal const string GenericRequestTypeName = "{0}Request";
    internal const string GenericResponseTypeName = "{0}Response";

    internal const string GenericFiltersAssemblyName = $"{GenericAssemblyName}.{GenericFiltersModuleName}";
    internal const string GenericFiltersModuleName = "GenericFilters";
    internal const string GenericFilterTypeName = "{0}Filter";
    internal const string GenericFilterPropertyTypeName = "{0}{1}Filter";

    internal const string GenericGqlQueriesAssemblyName = $"{GenericAssemblyName}.{GenericGqlQueriesModuleName}";
    internal const string GenericGqlQueriesModuleName = "GenericGqlQueries";
    internal const string GenericGqlQueryTypeName = "GenericGqlQuery";
    internal const string GenericGqlQueryPropertyTypeName = "{0}Query";

    internal const string GenericGqlMutationsAssemblyName = $"{GenericAssemblyName}.{GenericGqlMutationsModuleName}";
    internal const string GenericGqlMutationsModuleName = "GenericGqlMutations";
    internal const string GenericGqlMutationTypeName = "GenericGqlMutation";
    internal const string GenericGqlMutationPropertyTypeName = "{0}Mutation";

    internal const string RequestStartedOnContextItemName = "RequestStartedOn";
    internal const string DatabaseConfigurationSectionName = "Database";
    internal const string GenericInMemoryDatabaseName = "GenericInMemoryDatabase";

    internal const string PropertyBackingFieldName = "_{0}";
    internal const string PropertyGetterName = "get_{0}";
    internal const string PropertySetterName = "set_{0}";

    internal const string GenericTypeBeautifulName = "{0}<{1}>";

    #endregion

    #region Exceptions

    internal const string TheOperationHasBeenCanceled = "The operation has been canceled.";
    internal const string InconsistentDataHasBeenFound = "Inconsistent data has been found.";
    internal const string NoDataHasBeenFound = "No data has been found.";

    internal const string NoDatabaseProviderHasBeenChosenAsAPrimaryOne =
        "No database provider has been chosen as a primary one.";

    internal const string AnUnexpectedInternalServerErrorHasHappened =
        "An unexpected internal server error has happened.";

    internal const string AnUnexpectedInternalServerErrorHasHappenedOutOfTheControllerContext =
        "An unexpected internal server error has happened out of the controller context.";

    internal const string AnEntityWithTheSameKeyAlreadyExists = "An entity with the same key already exists ([{0}]).";
    internal const string AnEntityWithTheGivenKeyDoesNotExist = "An entity with the given key does not exist ([{0}]).";

    internal const string UnableToFindAPropertyWithNameInObject =
        "Unable to find a property with name {0} in object {1}.";

    internal const string UnableToFindAFieldWithNameInObject =
        "Unable to find a field with name {0} in object {1}.";

    internal const string UnableToFindASuitableConstructorForTheType =
        "Unable to find a suitable constructor for the type {0}.";

    internal const string UnableToFindAMethodWithParametersForTheType =
        "Unable to find a method {0} with {1} parameters for the type {2}.";

    internal const string UnableToFindAMethodWithParametersAndGenericArgumentsForTheType =
        "Unable to find a method {0} with {1} parameters and {2} generic arguments for the type {3}.";

    internal const string TheEntityTypeKeySetDoesNotMatchTheGivenKeys =
        "The entity type key set doesn't match the given keys.";

    internal const string TheEntityKeyValuesDoesNotMatchTheGivenKeyValues =
        "The entity key values doesn't match the given key values. ([{0}] != [{1}])";

    internal const string TheFilterPropertiesHaveToBeInheritedFrom =
        "The filter properties have to be inherited from {0}.";

    internal const string MethodCanBeInvokedOnlyIfFilterWasFilled =
        "Method {0} can be invoked only if {1} filter was filled.";

    internal const string AGenericControllerCanBeGeneratedOnlyForTypesMarkedWith =
        "A generic controller can be generated only for types marked with {0} ({1}).";

    internal const string AGenericGraphQlQueryCanBeGeneratedOnlyForTypesMarkedWith =
        "A generic GraphQL query can be generated only for types marked with {0} ({1}).";

    internal const string AGenericGraphQlMutationCanBeGeneratedOnlyForTypesMarkedWith =
        "A generic GraphQL mutation can be generated only for types marked with {0} ({1}).";

    internal const string UnableToBuildAGenericControllerForTypeThePropertyHasNotBeenFound =
        "Unable to build a generic controller for type {0}. The property {1} has not been found.";

    internal const string UnableToBuildAGenericGraphQlQueryForTypeThePropertyHasNotBeenFound =
        "Unable to build a generic GraphQL query for type {0}. The property {1} has not been found.";

    internal const string UnableToBuildAGenericGraphQlMutationForTypeThePropertyHasNotBeenFound =
        "Unable to build a generic GraphQL mutation for type {0}. The property {1} has not been found.";

    #endregion

    #region Api

    internal const string ApiVersion = "v1";
    internal const string ApiGroupName = $"Generic API {ApiVersion}";
    internal const string ApiRouteTemplate = $"api/{ApiVersion}/{{0}}";

    #endregion

    #region Environment

    internal const string HostEnvironmentVariable = "HOST";
    internal const string PortEnvironmentVariable = "PORT";

    #endregion

    #region Logs

    internal const string ClusterDiscoveryRequestLogName = "discovery request";

    internal const string ClusterDiscoveryRequestLogMessage =
        "Discover at (node: ({NodeId}, {NodeCreatedUtc}, {NodeLastRequestToUtc}, {NodeLastRequestFromUtc}, {NodeRequestTimeMs})) with params (node: ({RemoteNodeId}, {RemoteNodeCreated}, {RemoteNodeLastRequestTo}, {RemoteNodeLastRequestFrom}, {RemoteNodeRequestTimeMs}))";

    #endregion
}