using Security.Entities;

namespace Security.Jwt;

public interface ITokenHelper<TId>
{
    AccessToken CreateToken(User<TId> user, IList<OperationClaim<TId>> operationClaims);

    RefreshToken<TId> CreateRefreshToken(User<TId> user, string ipAddress);
}