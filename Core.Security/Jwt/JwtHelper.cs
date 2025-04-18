﻿using Security.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security.Encryption;
using Security.Extensions;


namespace Security.Jwt;

public class JwtHelper<TId> : ITokenHelper<TId>
{
    public IConfiguration Configuration { get; }
    private readonly TokenOptions _tokenOptions;
    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        Configuration = configuration;
        _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
    }
    
    public AccessToken CreateToken(User<TId> user, IList<OperationClaim<TId>> operationClaims)
    {
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new AccessToken
        {
            Token = token,
            Expiration = _accessTokenExpiration
        };
    }

    private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User<TId> user, SigningCredentials signingCredentials, IList<OperationClaim<TId>> operationClaims)
    {
        return new JwtSecurityToken(tokenOptions.Issuer, tokenOptions.Audience, expires: _accessTokenExpiration,
            notBefore: DateTime.Now, claims: SetClaims(user, operationClaims), signingCredentials: signingCredentials);
    }

    private static IEnumerable<Claim> SetClaims(User<TId> user, IList<OperationClaim<TId>> operationClaims)
    {
        var claims = new List<Claim>();
        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddEmail(user.Email);
        claims.AddName($"{user.FirstName} {user.LastName}");
        claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

        return claims;
    }

    public RefreshToken<TId> CreateRefreshToken(User<TId> user, string ipAddress)
    {
        return new RefreshToken<TId>
        {
            UserId = user.Id,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }
}
