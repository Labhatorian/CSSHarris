using Setup.Models.DeveloperModels;
using Setup.Models.DeveloperModels.Profile;
using System.Text;

namespace Setup.Models
{
    public class DeveloperViewModel
    {
        public Developer DeveloperPerson { get; set; } = new Developer();

        /// <summary>
        /// Generates HTML for each charactistic
        /// </summary>
        /// <returns></returns>
        public string? ViewCharacteristics()
        {
            if (DeveloperPerson.Characteristics is null) return null;

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"personalia-content__category\">");
            sb.Append("<h3 class=\"personalia-content__category-title\">Eigenschappen</h3>");
            sb.Append("<hr>");
            sb.Append("<ul class=\"personalia-content__list\">");

            foreach (string characteristic in DeveloperPerson.Characteristics)
            {
                sb.Append("<li>" + characteristic + "</li>");
            }

            sb.Append("</ul>");
            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Generates HTML for every skill
        /// </summary>
        /// <returns></returns>
        public string? ViewSkills()
        {
            if (DeveloperPerson.Skills is null) return null;

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"personalia-content__category\">");
            sb.Append("<h3 class=\"personalia-content__category-title\">Skills</h3>");
            sb.Append("<hr>");
            sb.Append("<ul class=\"personalia-content__list\">");

            foreach (Skill skill in DeveloperPerson.Skills)
            {
                sb.Append("<li>" + skill.SkillName);
                sb.Append("<div class=\"personalia-content-list__stars\">");

                for (int i = 0; i <= 5; i++)
                {
                    string append = skill.Stars >= i ? "star__checked" : "";
                    sb.Append("<span class=\"fa fa-star " + append + "\"></span>");
                }
                sb.Append("</div>");
            }

            sb.Append("</ul>");
            sb.Append("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// Generates HTML for every study
        /// </summary>
        /// <returns></returns>
        public string? ViewStudies()
        {
            if (DeveloperPerson.Studies is null) return null;
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"profile-content__category\">" +
                "<h3 class=\"profile-content__category-title\">Opleidingen</h3>" +
                "<hr>" +
                "</div>");

            foreach (Study study in DeveloperPerson.Studies)
            {
                sb.Append("<div class=\"profile-content__element\">" +
                    "<div class=\"profile-content__element-header\">" +
                    "<h4 class=\"profile-content__title\">" + study.StudyName + "</h4>" +
                    "<p class=\"profile-content__period\">" + study.StartStudy.ToShortDateString() + " - " + study.EndStudy.ToShortDateString() + "</p>" +
                    "</div>" +
                    "<p class=\"profile-content__institute\">" + study.StudyPlace + "</p>" +
                    "</div>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates HTML for every experience
        /// </summary>
        /// <returns></returns>
        public string? ViewExperience()
        {
            if (DeveloperPerson.WorkExperience is null) return null;
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"profile-content__category\">" +
                "<h3 class=\"profile-content__category-title\">Werkervaring</h3>" +
                "<hr>" +
                "</div>");

            foreach (Experience experience in DeveloperPerson.WorkExperience)
            {
                sb.Append("<div class=\"profile-content__element\">" +
                    "<div class=\"profile-content__element-header\">" +
                    "<h4 class=\"profile-content__title\">" + experience.WorkName + "</h4>" +
                    "<p class=\"profile-content__period\">" + experience.StartWork.ToShortDateString() + " - " + experience.EndWork.ToShortDateString() + "</p>" +
                    "</div>" +
                    "<p class=\"profile-content__institute\">" + experience.WorkPlace + "</p>" +
                    "</div>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates the gallery and slideshow for every image
        /// </summary>
        /// <returns></returns>
        public string? ViewImages()
        {
            if (DeveloperPerson.WorkExperience is null) return null;
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"profile-content__category\">" +
                "<h3 class=\"profile-content__category-title\">Mijn afbeeldingen</h3>" +
                "<hr>" +
                "</div>");

            //Slideshow
            sb.Append("<div class=\"profile-content__slideshow\">");

            for (int i = 1; i <= DeveloperPerson.Images.Count(); i++)
            {
                sb.Append("<div class=\"profile-content-slideshow__slides\">" +
                    "<div class=\"profile-content-slideshow-slides__numbertext\"> " + i + " / " + DeveloperPerson.Images.Count() + " </div>" +
                    "<img src=\"/images/" + DeveloperPerson.Name + "/" + DeveloperPerson.Images[i - 1].FilePath + "\">" +
                    "<div class=\"profile-content-slideshow-slides__text\">" + DeveloperPerson.Images[i - 1].Name + "</div>" +
                    "</div>");
            }

            sb.Append("<a class=\"profile-content-slideshow__prev\" onclick=\"plusSlides(-1)\">&#10094;</a>" +
                "<a class=\"profile-content-slideshow__next\" onclick=\"plusSlides(1)\">&#10095;</a>" +
                "</div>" +
                "<br>");

            //Gallery
            sb.Append("<div class=\"profile-content-gallery\">");

            foreach (DevImage image in DeveloperPerson.Images)
            {
                sb.Append("<div class=\"profile-content-gallery__image\">" +
                    "<a target=\"_blank\" href=\"images/" + DeveloperPerson.Name + "/" + image.FilePath + "\">" +
                    "<img src=\"/images/" + DeveloperPerson.Name + "/" + image.FilePath + "\" alt=\"" + image.Name + "\" width=\"600\" height=\"400\">" +
                    "</a>" +
                    "<div class=\"profile-content-gallery-image_desc\">" + image.Name + "</div>" +
                    "</div>");
            }

            sb.Append("</div>");

            return sb.ToString();
        }
    }
}