using Persistence.Repositories.Concrete;

namespace Security.Entities;

public class UserOperationClaim <TId> : Entity<TId>
{
    public int UserId { get; set; }
    public int OperationClaimId { get; set; }

    public virtual User<TId> User { get; set; }
    public virtual OperationClaim<TId> OperationClaim { get; set; }

    public UserOperationClaim()
    {
    }

    public UserOperationClaim(TId id, int userId, int operationClaimId) : base(id)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }
}