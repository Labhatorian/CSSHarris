using System.ComponentModel.DataAnnotations.Schema;

namespace CSSHarris.Models.ChatModels
{
    [NotMapped]
    public class ChatUserVM
    {
        public string UserName { get; set; }
    }
}
