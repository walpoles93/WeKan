using System;
using WeKan.API.Controllers;
using Xunit;

namespace WeKan.API.UnitTests.Controllers
{
    public class BoardsControllerTests
    {
        [Fact]
        public void Ctor_IMediatorNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new BoardsController(null));
        }
    }
}
