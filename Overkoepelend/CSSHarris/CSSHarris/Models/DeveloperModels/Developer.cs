using CSSHarris.Models.DeveloperModels.Profile;

namespace CSSHarris.Models.DeveloperModels
{
    public class Developer
    {
        public string Name { get; set; }
        public string PersonImage { get; set; }
        public string ProfileDescription { get; set; }
        public List<string> Characteristics { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
        public List<Study> Studies { get; set; } = new();
        public List<Experience> WorkExperience { get; set; } = new();
        public List<DevImage> Images { get; set; } = new();

        /// <summary>
        /// Generates developer's profile
        /// </summary>
        public Developer()
        {
            Name = "Harris";
            PersonImage = "person-pasfoto.png";
            ProfileDescription = "Student op Windesheim";
            Characteristics.Add("Leerzam");
            Characteristics.Add("Serieus");
            Characteristics.Add("Goed");
            Skills.Add(new Skill("C#", 4));
            Skills.Add(new Skill("Java", 4));
            Skills.Add(new Skill("HTML", 3));
            Skills.Add(new Skill("Python", 3));
            Skills.Add(new Skill("JavaScript", 2));
            Skills.Add(new Skill("CSS", 2));
            Studies.Add(new Study("HBO-ICT", "Windesheim Zwolle", new DateTime(2021, 5, 1), new DateTime(2025, 5, 1)));
            Studies.Add(new Study("Havo", "Zuyderzee Lyceum", new DateTime(2019, 9, 1), new DateTime(2021, 5, 1)));
            Studies.Add(new Study("Snel-TL", "Bonifatius Mavo", new DateTime(2016, 9, 1), new DateTime(2019, 5, 1)));
            Images.Add(new DevImage("classic-cars.jpg", "Mijn auto"));
        }
    }
}