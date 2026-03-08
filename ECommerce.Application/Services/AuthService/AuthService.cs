using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, 
            IRefreshTokenRepository refreshTokenRepository, 
            IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // REGISTER
        public async Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO registerUserDTO) 
        {
            // Check email not already taken
            var existingUser = await _userRepository.GetUserByEmailAsync(registerUserDTO.Email);
            if (existingUser != null)
                throw new ConflictException("Email is already registered.");

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);

            // Create user
            var user = new User(
                registerUserDTO.UserName,
                registerUserDTO.FirstName + registerUserDTO.LastName,
                registerUserDTO.Email,
                passwordHash
            );

            await _userRepository.AddUserAsync(user);

            // Generate tokens
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken(user.UserId);

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return MapToAuthResponseDTO(user, accessToken, refreshToken.Token);
        }

        // LOGIN
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO) 
        {
            // Find user
            var user = await _userRepository.GetUserByEmailAsync(loginDTO.Email);
            if ( user is null)
                throw new NotFoundException("Invalid email or password.");

            // Check active
            if (!user.IsActive)
                throw new UnauthorizedException("User account is inactive.");

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash)) 
                throw new ValidationException("Invalid email or password.");


            // Generate tokens
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken(user.UserId);

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return MapToAuthResponseDTO(user, accessToken, refreshToken.Token);
        }


        // LOGOUT
        public async Task LogoutAsync(RefreshTokenDTO refreshTokenDTO) 
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshTokenDTO.RefreshToken);

            // Silently return if token doesn't exist or already revoked
            // Never tell the client WHY - security best practice
            if (storedToken is null || !storedToken.IsActive())
                return;

            storedToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(storedToken);
            await _unitOfWork.SaveChangesAsync();
        }

        // REFRESH TOKEN
        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO) 
        {
            // Find the token in DB
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshTokenDTO.RefreshToken);
            if (storedToken is null || !storedToken.IsActive())
                throw new ValidationException("Invalid or expired refresh token.");

            // Get user
            var user = await _userRepository.GetUserByIdAsync(storedToken.UserId);
            if (user is null || !user.IsActive)
                throw new NotFoundException("User not found or deactivated.");

            // Revoke old token
            storedToken.Revoke();
            await _refreshTokenRepository.UpdateAsync(storedToken);

            // Issue new tokens
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken(user.UserId);

            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return MapToAuthResponseDTO(user, newAccessToken, newRefreshToken.Token);
        }

        // CHANGE PASSWORD
        public async Task ChangePasswordAsync(ChangePasswordDTO changePasswordDTO) 
        {
            if (changePasswordDTO.UserId == Guid.Empty)
                throw new ValidationException("User ID is required.");

            var user = await _userRepository.GetUserByIdAsync(changePasswordDTO.UserId);
            if (user is null)
                throw new NotFoundException("User not found.");

            // verify current password
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDTO.CurrentPassword, user.PasswordHash))
                throw new ValidationException("Current password is incorrect.");

            // hash new password and update
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDTO.NewPassword);
            user.ChangePassword(newPasswordHash);

            await _userRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        // PRIVATE HELPERS
        private string GenerateAccessToken(User user) 
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
                );

            var claims = new[]
            { 
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(jwtSettings["AccessTokenExpiryMinutes"]!)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public RefreshToken GenerateRefreshToken(Guid userId) 
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiresAt = DateTime.UtcNow.AddDays(
                double.Parse(_configuration["JwtSettings:RefreshTokenExpiryDays"]!)
            );

            return new RefreshToken(token, userId, expiresAt);
        }

        private static AuthResponseDTO MapToAuthResponseDTO(User user, string accessToken, string refreshToken) 
        {
            return new AuthResponseDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString(),
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

    }
}
