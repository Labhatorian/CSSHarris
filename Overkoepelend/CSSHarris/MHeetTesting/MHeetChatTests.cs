using CSSHarris.Data;
using CSSHarris.Hubs;
using CSSHarris.Models.ChatModels;
using MHeetTesting.Integration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace MHeetTesting
{
    public class MHeetChatTests
    {
        public ChatHub Hub { get; set; }
        public Mock<IConnectionHub> MockConnectionHub { get; set; }

        //[SetUp]
        public MHeetChatTests()
        {
            //Database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryTest")
                .Options;
            var context = new ApplicationDbContext(options);

            //HTTPContext
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(r => r.Headers).Returns(new HeaderDictionary { { "device-id", "123" } });
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

            //User
            var ctx = new ControllerContext() { HttpContext = new DefaultHttpContext() };
            ctx.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
            }));

            //Hub
            Hub = new ChatHub(context, null);
            var mockClients = new Mock<IHubCallerClients<IConnectionHub>>();
            MockConnectionHub = new Mock<IConnectionHub>();
            mockClients.Setup(m => m.Caller).Returns(MockConnectionHub.Object);
            //Groups
            var mockGroups = new Mock<IGroupManager>();
            mockGroups.Setup(g => g.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            //Clients
            mockClients.Setup(c => c.Group(It.IsAny<string>()))
           .Returns(MockConnectionHub.Object);
            //Apply
            Hub.Groups = mockGroups.Object;
            Hub.Clients = mockClients.Object;
            Hub.Context = new FakeHubHTTPContext(ctx.HttpContext, "1");

            //Test room
            Room room = new()
            {
                Owner = "TestUser",
                Title = "TestRoom",
                ID = "1"
            };
            context.Rooms.Add(room);
        }

        [Fact]
        public void SendMessageSuccess()
        {
            bool sendCalled = false;

            Hub.Join("TestUser");
            Hub.JoinRoom("1");

            MockConnectionHub.Setup(m => m.ReceiveMessage(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => sendCalled = true);
            Hub.SendMessage("1", "Hello World");

            Assert.True(sendCalled);
        }
    }
}