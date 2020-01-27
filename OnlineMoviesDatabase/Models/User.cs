using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BCrypt.Net;

namespace OnlineMovieDatabase.Models
{
    public enum UserRoleEnum
    {
        Admin,
        Moderator,
        User
    }
    public class User
    {
        public User()
        {
            
        }
        public static readonly string AdminRole = "Admin";
        public static readonly string ModeratorRole = "Moderator";
        public static readonly string SimpleUserRole = "User";
        public int Id { get; set; }
        public byte UserRole { get; set; }
        public string RoleName { get { return ((UserRoleEnum)UserRole).ToString(); } }
        public bool IsBanned { get; set; }
        public bool? IsConfirmed { get; set; }
        public bool OauthAuthorized { get; set; }
        public bool NeedToNotificate { get; set; }
        public DateTime? BannedFor { get; set; }
        public string UserName { get; set; }
        [EmailAddress] public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public string OauthServiceName { get; set; }
        public string OauthSocialId { get; set; }
        public string GetHexHashCodeString()
        {
            string string4hash = GetStringForVerify();

            return UserName + "|" + BCrypt.Net.BCrypt.HashString(string4hash).Replace("/", "~~");
        }
        public string GetStringForVerify()
        {
            return UserName + UserRole + PasswordHash;
        }
        public void ConfirmEmail()
        {
            this.IsConfirmed = true;
        }
        public UserMinimalInfo GetUserMinimalInfo()
        {
            return new UserMinimalInfo(this);
        }

        internal static User CreateOauthUser(string _serviceName, int _socilaId, string _username, string _avatarUrl = null)
        {
                User result = new User()
                {
                    UserName = _username,
                    UserRole = (byte)UserRoleEnum.User,
                    OauthAuthorized = true,
                    OauthServiceName = _serviceName,
                    OauthSocialId = _socilaId.ToString(),
                    IsConfirmed = true
                };
                return result;

        }
    }

    public class UserMinimalInfo
    {
        public UserMinimalInfo(User user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
        }
        public UserMinimalInfo(int _id, string _userName)
        {
            this.Id = _id;
            this.UserName = _userName;
        }
        public string UserName { get; set; }
        public int Id { get; set; }
    }
}