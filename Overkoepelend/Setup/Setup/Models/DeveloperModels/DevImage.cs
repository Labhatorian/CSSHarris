namespace Setup.Models.DeveloperModels
{
    public class DevImage
    {
        public string FilePath { get; set; }
        public string Name { get; set; }

        public DevImage(string filePath, string name)
        {
            FilePath = filePath;
            Name = name;
        }
    }
}
