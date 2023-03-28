using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MHeetTesting.Integration
{
    public class FakeHubHTTPContext : HubCallerContext
    {
        public HttpRequest Request;
        private ClaimsPrincipal? _User;
        private string _ConnectionID;
        public FakeHubHTTPContext(HttpContext httpContext, string connectionID) : base()
        {
            Request = httpContext.Request;
            _User = httpContext.User;
            _ConnectionID = connectionID;
        }
        public override ClaimsPrincipal? User => _User;
        public override string ConnectionId => _ConnectionID;

        public override string? UserIdentifier => throw new NotImplementedException();
        public override IDictionary<object, object?> Items => throw new NotImplementedException();
        public override IFeatureCollection Features => throw new NotImplementedException();
        public override CancellationToken ConnectionAborted => throw new NotImplementedException();
        public override void Abort() => throw new NotImplementedException();
    }
}