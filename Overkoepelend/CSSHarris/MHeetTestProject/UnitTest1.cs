using CSSHarris.Models;
using Mailjet.Client.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;

namespace MHeetTestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateAccountCorrectly()
        {
            Assert.Pass();
        }

        [Test]
        public void CreateRoleAndAddToUser()
        {

            Assert.Pass();
        }

        [Test]
        public void ManageUserAndBan()
        {
            Assert.Pass();
        }

        [Test]
        public void AddUserToAdminCheckForAdminPolicy()
        {


            Assert.Pass();
        }

        [Test]
        public void AddUserToNoRoleCheckForAdminPolicy()
        {


            Assert.Pass();
        }
    }
}