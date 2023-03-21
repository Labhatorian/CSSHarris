namespace CSSHarris.Models.Management
{
    public class UserManageViewModel
    {
        public string UserId { get; internal set; }
        public string? Username { get; internal set; }
        public string? Email { get; internal set; }
        public bool? Banned { get; internal set; }
        public bool? Verified { get; internal set; }
        public List<string>? Roles { get; internal set; }
    }
}