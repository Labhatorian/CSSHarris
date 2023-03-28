using BloggerDomain.Repositories;
using BloggerDomain.Services;
using BloggerTests.Integration;
using Moq;

namespace BloggerTests.Unit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void UpdateBlogSuccess()
        {
            var mockRepo = new Mock<IBlogRepository>();
            mockRepo.Setup(r => new MockBloggingRepository());

            var blogService = new BlogService(mockRepo.Object);

            var authorID = 1;
            var authorBlog = blogService.GetBlog(authorID);

            var success = blogService.UpdateBlog(authorID,authorBlog ); 

            Assert.True( success );
        }

        [Fact]
        public void UpdateBlogFail()
        {
            var mockRepo = new Mock<IBlogRepository>();
            mockRepo.Setup(r => new MockBloggingRepository());

            var blogService = new BlogService(mockRepo.Object);

            var authorID = 1;
            var otherAuthorID = 2;
            var authorBlog = blogService.GetBlog(authorID);

            var success = blogService.UpdateBlog(authorID, authorBlog);

            Assert.Throws<InvalidOperationException>(() => blogService.UpdateBlog(otherAuthorID, authorBlog));
        }
    }
}