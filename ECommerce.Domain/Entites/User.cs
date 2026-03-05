using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entites
{
    public class User
    {
        public Guid UserId { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty; // Unique
        public string PasswordHash { get; private set; } = string.Empty;

        public bool IsActive { get; private set; } = true;
        public Guid RoleId { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public User() { } // For EF Core

        public User(
            string userName,
            string fullName,
            string email,
            string passwordHash,
            Guid roleId
        ) 
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new DomainExceptions("UserName cannot be empty.");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new DomainExceptions("FullName cannot be empty.");
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainExceptions("Email cannot be empty.");

            if (!email.Contains("@"))
                throw new DomainExceptions("Email must be a valid email address.");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainExceptions("PasswordHash cannot be empty.");

            UserId = Guid.NewGuid();
            UserName = userName;
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            RoleId = roleId;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }


        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void UpdateProfile(string fullName, string userName) 
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new DomainExceptions("FullName cannot be empty.");
            if (string.IsNullOrWhiteSpace(userName))
                throw new DomainExceptions("UserName cannot be empty.");

            FullName = fullName;
            UserName = userName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash) 
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new DomainExceptions("New password hash cannot be empty.");
            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignRole(Guid newRoleId) 
        {
            RoleId = newRoleId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
