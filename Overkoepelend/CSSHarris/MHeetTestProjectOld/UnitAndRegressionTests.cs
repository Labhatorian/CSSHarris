using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MHeetTestProject
{
    public class Tests : WebApplicationFactory<IStartup>
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = CreateClient();
        }

        [Test]
        public async Task LoginGetsSuccess()
        {
            var response = await _client.GetAsync("/Identity/Account/Login");
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task RegisterGetsSuccess()
        {
            var response = await _client.GetAsync("/Identity/Account/Register");
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task AccountGetsSuccess()
        {
            var response = await _client.GetAsync("/Identity/Account/Manage");
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task LogoutGetsSuccess()
        {
            var response = await _client.GetAsync("/Identity/Account/Logout");
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task AdminHasAdminRoleAndPolicy()
        {
        }

        [Test]
        public async Task UserHasNoAdminRoleAndPolicy()
        {

        }

        [Test]
        public async Task LoginCorrect()
        {

        }

        [Test]
        public async Task LoginWrongPassword()
        {

        }
    }
}