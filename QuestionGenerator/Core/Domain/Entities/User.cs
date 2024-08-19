using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace QuestionGenerator.Core.Domain.Entities
{
    public class User : Auditables
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; }
        public string? GoogleId { get; set; }
        public string Email { get; set; } = default!;
        public string? PasswordHash { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int RoleId { get; set; }

        #region Behaviours
        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16]; // 16 bytes for a salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        #endregion

        #region Relationships
        public Role Role { get; set; }
        public List<Document> Documents { get; set; } = [];
        public List<Token> Tokens { get; set; } = [];

        #endregion
    }
}
