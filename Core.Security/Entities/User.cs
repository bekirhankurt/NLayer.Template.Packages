using Persistence.Repositories.Concrete;
using Security.Enums;

namespace Security.Entities;

public class User<TId> : Entity<TId>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public bool Status { get; set; }
    public AuthenticatorType AuthenticatorType { get; set; }

    public virtual ICollection<UserOperationClaim<TId>> UserOperationClaims { get; set; }
    public virtual ICollection<RefreshToken<TId>> RefreshTokens { get; set; }

    public User()
    {
        UserOperationClaims = new HashSet<UserOperationClaim<TId>>();
        RefreshTokens = new HashSet<RefreshToken<TId>>();
    }

    public User(TId id, string firstName, string lastName, string email, byte[] passwordSalt, byte[] passwordHash,
        bool status, AuthenticatorType authenticatorType) : this()
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        Status = status;
        AuthenticatorType = authenticatorType;
    }
}