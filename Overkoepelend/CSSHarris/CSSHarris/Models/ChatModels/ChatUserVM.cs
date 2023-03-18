using System.ComponentModel.DataAnnotations.Schema;

namespace CSSHarris.Models.ChatModels
{
    [NotMapped]
    public class ChatUserVM
    {
        public string? ChatUserID { get; set; }
        public string UserName { get; set; }
    }
}
