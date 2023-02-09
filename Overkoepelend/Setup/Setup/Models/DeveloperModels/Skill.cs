namespace Setup.Models.DeveloperModels
{
    public class Skill
    {
        public string SkillName { get; set; }
        public int Stars { get; set; } //TODO 0 to 5

        public Skill(string skillName, int stars)
        {
            SkillName = skillName;
            Stars = stars;
        }
    }
}
