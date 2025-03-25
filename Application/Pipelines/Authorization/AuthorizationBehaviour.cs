using CrossCuttingConcerns.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Security.Extensions;

namespace Application.Pipelines.Authorization;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest: IRequest<TResponse>, ISecuredRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
        if (roleClaims is null) throw new AuthorizationException("Claims not found.");

        var isNotMatchedARoleClaimWithRequestRoles = roleClaims
            .FirstOrDefault(roleClaim => request.Roles.Any(role => role == roleClaim)).Any();
        if (isNotMatchedARoleClaimWithRequestRoles) throw new AuthorizationException("You are not authorized");

        return await next();
        
    }
}