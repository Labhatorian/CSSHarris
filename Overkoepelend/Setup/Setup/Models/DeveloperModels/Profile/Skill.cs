using System.ComponentModel.DataAnnotations;

namespace Setup.Models.DeveloperModels.Profile
{
    public class Skill
    {
        public string SkillName { get; set; }

        [Range(0, 5)]
        public int Stars { get; set; }

        public Skill(string skillName, int stars)
        {
            SkillName = skillName;
            Stars = stars;
        }
    }
}
