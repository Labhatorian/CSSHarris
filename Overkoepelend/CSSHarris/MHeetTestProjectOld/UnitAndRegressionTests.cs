using CSSHarris.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace MHeetTestProject
{
    public class Tests
    {
        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<RoleManager<IdentityRole>> GetMockRoleManager()
        {
            var roleMock = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(
                roleMock.Object, null, null, null, null);
        }

        private Mock<UserManager<ApplicationUser>> GetUserManager()
        {
            var mock = new Mock<IUserStore<ApplicationUser>>();
            var userStore = mock.Object;

            return new UserManager<ApplicationUser>(
                userStore,
                null,
                new PasswordHasher<ApplicationUser>(),
                null,
                null,
                null,
                null,
                null,
                null);
        }

        private Mock<RoleManager<IdentityRole>> GetRoleManager()
        {
            var roleMock = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(
                roleMock.Object, null, null, null, null);
        }

        [Test]
        public async Task CreateAccountCorrectly()
        {
            var mockUserManager = GetMockUserManager();
            var user = new ApplicationUser { UserName = "testuser" };

            mockUserManager.Setup(x => x.CreateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var result = await mockUserManager.Object.CreateAsync(user);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task AddRoleToUser()
        {
            var mockUserManager = GetMockUserManager();
            var user = new ApplicationUser { UserName = "testuser" };

            mockUserManager.Setup(x => x.CreateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var userresult = await mockUserManager.Object.CreateAsync(user);

            mockUserManager.Setup(x => x.AddToRoleAsync(user, "TestRole")).ReturnsAsync(IdentityResult.Success);
            var finalresult = await mockUserManager.Object.AddToRoleAsync(user, "TestRole");

            mockUserManager.Setup(x => x.IsInRoleAsync(user, "TestRole")).ReturnsAsync(true);
            var isInRole = await mockUserManager.Object.IsInRoleAsync(user, "TestRole");

            Assert.IsTrue(userresult.Succeeded);
            Assert.IsTrue(finalresult.Succeeded);
            Assert.IsTrue(isInRole);
        }

        [Test]
        public async Task DontCreateRoleAndAddToUser()
        {
            var mockUserManager = GetMockUserManager();
            var user = new ApplicationUser { UserName = "testuser" };

            mockUserManager.Setup(x => x.CreateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var userresult = await mockUserManager.Object.CreateAsync(user);

            mockUserManager.Setup(x => x.IsInRoleAsync(user, "TestRole")).ReturnsAsync(false);
            var isInRole = await mockUserManager.Object.IsInRoleAsync(user, "TestRole");

            Assert.IsTrue(userresult.Succeeded);
            Assert.IsTrue(isInRole);
        }

        [Test]//undone
        public async Task ManageUserAndBan()
        {
            var mockUserManager = GetMockUserManager();
            var user = new ApplicationUser { UserName = "testuser" };

            mockUserManager.Setup(x => x.CreateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var userresult = await mockUserManager.Object.CreateAsync(user);

            Assert.Pass();
        }

        [Test] //Regressie
        public async Task AddUserToAdminCheckForAdminClaim()
        {
            var mockUserManager = GetMockUserManager();
            var mockRoleManager = GetMockRoleManager();
            var user = new ApplicationUser { UserName = "testuser" };
            var role = new IdentityRole { Name = "AdminTest" };
            var claim = new Claim("staff", "Admin");

            mockUserManager.Setup(x => x.CreateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var userresult = await mockUserManager.Object.CreateAsync(user);

            mockUserManager.Setup(x => x.AddToRoleAsync(user, "AdminTest")).ReturnsAsync(IdentityResult.Success);
            var finalresult = await mockUserManager.Object.AddToRoleAsync(user, "AdminTest");

            mockUserManager.Setup(x => x.IsInRoleAsync(user, "AdminTest")).ReturnsAsync(true);
            var isInRole = await mockUserManager.Object.IsInRoleAsync(user, "AdminTest");

            mockRoleManager.Setup(x => x.AddClaimAsync(role, claim));

            var claims = await mockUserManager.Object.GetClaimsAsync(user);

            Assert.IsTrue(userresult.Succeeded);
            Assert.IsTrue(finalresult.Succeeded);
            Assert.IsTrue(isInRole);
            Assert.Contains(claim, (System.Collections.ICollection?)claims);
        }

        [Test] //Regressie
        public async Task AddUserToNoRoleCheckForAdminClaim()
        {
            var mockUserManager = GetMockUserManager();
            var mockRoleManager = GetMockRoleManager();
            var user = new ApplicationUser { UserName = "testuser" };
            var role = new IdentityRole { Name = "AdminTest" };
            var claim = new Claim("staff", "Admin");

            mockUserManager.Setup(x => x.CreateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var userresult = await mockUserManager.Object.CreateAsync(user);

            mockUserManager.Setup(x => x.IsInRoleAsync(user, "AdminTest")).ReturnsAsync(true);
            var isInRole = await mockUserManager.Object.IsInRoleAsync(user, "AdminTest");

            mockRoleManager.Setup(x => x.AddClaimAsync(role, claim));

            mockUserManager.Setup(x => x.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>());
            var claims = await mockUserManager.Object.GetClaimsAsync(user);

            Assert.IsTrue(userresult.Succeeded);
            Assert.IsTrue(isInRole);
            Assert.IsFalse(claims.Contains(claim));
        }

        [Test]//undone
        public async Task LoginAccountCorrect()
        {
            var mockUserManager = GetMockUserManager();
            var user = new ApplicationUser { UserName = "testuser" };

            mockUserManager.Setup(x => x.CreateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var result = await mockUserManager.Object.CreateAsync(user);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]//undone
        public async Task LoginAccountFail()
        {
            var mockUserManager = GetMockUserManager();
            var user = new ApplicationUser { UserName = "testuser" };

            mockUserManager.Setup(x => x.CreateAsync(user)).ReturnsAsync(IdentityResult.Success);
            var result = await mockUserManager.Object.CreateAsync(user);

            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task JoinChatHub()
        {

        }
    }
}