using ECommerce.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO registerDTO);
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO);
        Task LogoutAsync(RefreshTokenDTO refreshTokenDTO);
        Task ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);
    }
}
