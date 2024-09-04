using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.Identity;
using ChatGPTClone.Application.Common.Models.Jwt;
using ChatGPTClone.Infrastructure.Identity;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatGPTClone.Infrastructure.Services;

public class IdentityManager : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;

    public IdentityManager(UserManager<AppUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    // Kullanıcının kimliğini doğrular.
    public async Task<bool> AuthenticateAsync(IdentityAuthenticateRequest request, CancellationToken cancellationToken)
    {
        // Kullanıcıyı e-posta adresine göre bul.
        var user = await _userManager.FindByEmailAsync(request.Email);

        // Kullanıcı bulunamazsa false döndür.
        if (user is null) return false;

        // Kullanıcının parolasını kontrol et ve sonucu döndür.
        return await _userManager.CheckPasswordAsync(user, request.Password);
    }

    // E-posta adresinin veritabanında olup olmadığını kontrol eder.
    public Task<bool> CheckEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return _userManager
        .Users
        .AnyAsync(x => x.Email == email, cancellationToken);
    }

    // Kullanıcının giriş yapmasını sağlar.
    public async Task<IdentityLoginResponse> LoginAsync(IdentityLoginRequest request, CancellationToken cancellationToken)
    {
        // Kullanıcıyı e-posta adresine göre bul.
        var user = await _userManager.FindByEmailAsync(request.Email);

        // Kullanıcının rollerini al.
        var roles = await _userManager.GetRolesAsync(user);

        // JWT oluşturma isteği oluştur.
        var jwtRequest = new JwtGenerateTokenRequest(user.Id, user.Email, roles);

        // JWT oluştur.
        var jwtResponse = _jwtService.GenerateToken(jwtRequest);

        // Giriş yanıtını döndür.
        return new IdentityLoginResponse(jwtResponse.Token, jwtResponse.ExpiresAt);
    }

    // Yeni bir kullanıcı kaydeder.
    public async Task<IdentityRegisterResponse> RegisterAsync(IdentityRegisterRequest request, CancellationToken cancellationToken)
    {
        // Yeni bir kullanıcı kimliği oluştur.
        var userId = Ulid
        .NewUlid()
        .ToGuid();

        // Yeni bir kullanıcı nesnesi oluştur.
        var user = new AppUser
        {
            Id = userId,
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedByUserId = userId.ToString(),
            CreatedOn = DateTimeOffset.UtcNow,
            EmailConfirmed = false,
        };

        // Kullanıcıyı veritabanına kaydet.
        var result = await _userManager.CreateAsync(user, request.Password);

        // Kayıt işlemi başarısız olursa hata fırlat.
        if (!result.Succeeded) CreateAndThrowValidationException(result.Errors);

        // E-posta onaylama jetonu oluştur.
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // Kayıt yanıtını döndür.
        return new IdentityRegisterResponse(userId, emailToken);
    }

    // Doğrulama hatası oluşturur ve fırlatır.
    private void CreateAndThrowValidationException(IEnumerable<IdentityError> errors)
    {
        // Hata mesajlarını ve özelliklerini içeren yeni bir doğrulama hatası oluştur.
        var errorMessages = errors
        .Select(x => new ValidationFailure(x.Code, x.Description))
        .ToArray();

        // Doğrulama hatasını fırlat.
        throw new ValidationException(errorMessages);
    }
}