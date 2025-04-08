using Persistence.Repositories.Concrete;

namespace Security.Entities;

public class OtpAuthenticator <TId> : Entity<TId>
{
    public int UserId { get; set; }
    public byte[] SecretKey { get; set; }
    public bool IsVerified { get; set; }

    public virtual User<TId> User { get; set; }

    public OtpAuthenticator()
    {
    }

    public OtpAuthenticator(TId id, int userId, byte[] secretKey, bool isVerified) : this()
    {
        Id = id;
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = isVerified;
    }
}