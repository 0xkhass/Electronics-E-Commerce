using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsRevoked { get; private set; }

        public RefreshToken() { } // For EF Core

        public RefreshToken(string token, Guid userId, DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            Token = token;
            UserId = userId;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            IsRevoked = false;
        }

        public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive() => !IsRevoked && !IsExpired();

        public void Revoke() => IsRevoked = true;
    }
}
