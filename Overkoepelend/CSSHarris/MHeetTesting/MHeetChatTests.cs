using CSSHarris.Data;
using CSSHarris.Hubs;
using CSSHarris.Models;
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
    [TestCaseOrderer(
    ordererTypeName: "XUnit.Project.Orderers.AlphabeticalOrderer",
    ordererAssemblyName: "XUnit.Project")]
    public class MHeetChatTests
    {
        public ChatHub Hub { get; set; }
        public Mock<IConnectionHub> MockConnectionHub { get; set; }
        public ApplicationDbContext TestingDatabase { get; set; }
        public Room TestRoom { get; set; }

        //[SetUp]
        public MHeetChatTests()
        {
            //Database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            TestingDatabase = new ApplicationDbContext(options);

            //HTTPContext
            var mockHttpContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(r => r.Headers).Returns(new HeaderDictionary { { "device-id", "123" } });
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

            //User in context
            var ctx = new ControllerContext() { HttpContext = new DefaultHttpContext() };
            ctx.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
            }));

            //Hub
            Hub = new ChatHub(TestingDatabase, null);
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

            TestRoom = new()
            {
                Owner = "TestUser",
                Title = "TestRoom",
                ID = "9999"
            };
            TestingDatabase.Rooms.Add(TestRoom);
        }

        [Fact]
        public void SendMessageSuccess()
        {
            bool sendCalled = false;

            //Join room 
            Hub.Join("TestUser");
            Hub.JoinRoom("9999");

            MockConnectionHub.Setup(m => m.ReceiveMessage(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => sendCalled = true);
            Hub.SendMessage("9999", "Hello World");

            Assert.True(sendCalled);
        }

        [Fact]
        public void SendMessageFail()
        {
            bool sendCalled = false;

            MockConnectionHub.Setup(m => m.ReceiveMessage(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => sendCalled = true);
            Hub.SendMessage("9998", "Hello World"); //9998 does not exist

            Assert.False(sendCalled);
        }

        [Fact]
        public void DeleteMessageFail()
        {
            bool deletePassed = false;

            MockConnectionHub.Setup(m => m.ShowMessages(It.IsAny<List<Message>>()))
                .Callback(() => deletePassed = true);

            TestRoom.Chatlog.Messages.Add(new Message()
            {
                ID = 9102,
                Username = "TestUser",
                Content = "Test"
            });

            Hub.DeleteMessage("9999", "9102");

            Assert.False(deletePassed);
        }

        [Fact]
        public void LeaveRoomSuccess()
        {
            bool leavePassed = false;

            MockConnectionHub.Setup(m => m.UpdateUserList(It.IsAny<List<ChatUser>>()))
                .Callback(() => leavePassed = true);

            Hub.LeaveRoom();

            Assert.True(leavePassed);
        }

    }
}