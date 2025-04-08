using Persistence.Repositories.Concrete;

namespace Security.Entities;

public class EmailAuthenticator <TId> : Entity<TId>
{
    public int UserId { get; set; }
    public string? ActivationKey { get; set; }
    public bool IsVerified { get; set; }

    public virtual User<TId> User { get; set; }

    public EmailAuthenticator()
    {
    }

    public EmailAuthenticator(TId id, int userId, string? activationKey, bool isVerified) : this()
    {
        Id = id;
        UserId = userId;
        ActivationKey = activationKey;
        IsVerified = isVerified;
    }
}