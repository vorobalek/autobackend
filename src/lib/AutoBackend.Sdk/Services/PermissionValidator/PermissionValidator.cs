using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using AutoBackend.Sdk.Configuration;
using AutoBackend.Sdk.Enums;
using AutoBackend.Sdk.Exceptions.Data;
using AutoBackend.Sdk.Services.EntityMetadataProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AutoBackend.Sdk.Services.PermissionValidator;

internal sealed class PermissionValidator(
    IEntityMetadataProvider entityMetadataProvider,
    IHttpContextAccessor httpContextAccessor,
    IOptions<JwtConfiguration> options): IPermissionValidator
{
    public void Validate<TEntity>(PermissionType type, HashSet<string>? affectedProperties) where TEntity : class, new()
    {
        var requiredPermissions = GetRequiredPermissions<TEntity>(type, affectedProperties ?? []);
        if (requiredPermissions.Length == 0) return;
        var givenPermissions = GetValidGivenPermissions();
        CheckPermissions(givenPermissions, requiredPermissions);
    }

    private string[] GetRequiredPermissions<TEntity>(
        PermissionType permissionType,
        HashSet<string> affectedProperties) where TEntity : class, new()
    {
        var permissions = new List<string>();
        var metadata = entityMetadataProvider.GetMetadata<TEntity>();
        if (metadata.Permissions.HasFlag(permissionType)) 
            permissions.Add(
                string.Format(
                    Constants.GenericEntityPermissionName,
                    metadata.Name,
                    Enum.GetName(permissionType) ?? string.Empty));
        permissions.AddRange(
            metadata.Properties
                .Where(property => affectedProperties.Contains(property.Name))
                .Where(property => property.Permissions.HasFlag(permissionType))
                .Select(property => string.Format(
                    Constants.GenericPropertyPermissionName,
                    metadata.Name,
                    property.Name,
                    Enum.GetName(permissionType) ?? string.Empty)));
        return permissions.ToArray();
    }

    private string[] GetValidGivenPermissions()
    {
        var authorization = httpContextAccessor.HttpContext?.Request.Headers[Constants.AuthorizationHeaderName].FirstOrDefault();
        if (authorization is not null &&
            authorization.StartsWith(Constants.AuthorizationBearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var token = authorization[Constants.AuthorizationBearerPrefix.Length..].Trim();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(options.Value.PublicKey);
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(rsa)
                };
                if (!string.IsNullOrWhiteSpace(options.Value.ValidIssuer))
                {
                    parameters.ValidateIssuer = true;
                    parameters.ValidIssuer = options.Value.ValidIssuer;
                }
                if (!string.IsNullOrWhiteSpace(options.Value.ValidAudience))
                {
                    parameters.ValidateAudience = true;
                    parameters.ValidAudience = options.Value.ValidAudience;
                }

                var handler = new JwtSecurityTokenHandler();
                try
                {
                    var claimsPrincipal = handler.ValidateToken(token, parameters, out _);
                    return claimsPrincipal.Claims
                        .Where(claim => claim.Type == Constants.GenericPermissionsClaimType)
                        .Select(claim => claim.Value)
                        .ToArray();
                }
                catch (Exception exception)
                {
                    throw new UnauthorizedAccessDataException(
                        Constants.Unauthorized,
                        exception);
                }
            }
        }
        return [];
    }

    private static void CheckPermissions(string[] givenPermissions, string[] requiredPermissions)
    {
        var missingPermissions = requiredPermissions.Except(givenPermissions).ToArray();
        if (missingPermissions.Length > 0)
            throw new UnauthorizedAccessDataException(
                string.Format(
                    Constants.UnauthorizedMissingPermissions,
                    string.Join(", ", missingPermissions)));
    }
}