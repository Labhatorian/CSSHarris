namespace Setup.Models.DeveloperModels.Profile
{
    public class Experience
    {
        public string WorkName { get; set; }
        public string WorkPlace { get; set; }
        public DateTime StartWork { get; set; }
        public DateTime EndWork { get; set; }

        public Experience(string workName, string workPlace, DateTime startWork, DateTime endWork)
        {
            WorkName = workName;
            WorkPlace = workPlace;
            StartWork = startWork;
            EndWork = endWork;
        }
    }
}
