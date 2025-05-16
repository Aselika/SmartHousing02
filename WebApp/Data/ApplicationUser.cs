using Microsoft.AspNetCore.Identity;
using WebApp.Models;

namespace WebApp.Data
{
    /// <summary>
    /// Пользователь приложения, расширяющий базовый IdentityUser.
    /// Здесь можно добавить любые дополнительные поля профиля.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        // Навигационное свойство к профилю, если вы добавляли UserProfile
        public UserProfile Profile { get; set; }

        // Дополнительные поля, если понадобятся:
        // public string FirstName { get; set; }
        // public string LastName  { get; set; }
    }
}
