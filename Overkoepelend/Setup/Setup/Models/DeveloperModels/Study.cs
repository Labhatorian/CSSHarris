namespace Setup.Models.DeveloperModels
{
    public class Study
    {
        public string StudyName { get; set; }
        public string StudyPlace { get; set; }
        public DateTime StartStudy { get; set; }
        public DateTime EndStudy { get; set; }

        public Study(string studyName, string studyPlace, DateTime startStudy, DateTime endStudy)
        {
            StudyName = studyName;
            StudyPlace = studyPlace;
            StartStudy = startStudy;
            EndStudy = endStudy;
        }
    }
}
