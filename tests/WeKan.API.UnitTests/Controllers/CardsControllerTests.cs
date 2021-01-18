using System;
using WeKan.API.Controllers;
using Xunit;

namespace WeKan.API.UnitTests.Controllers
{
    public class CardsControllerTests
    {
        [Fact]
        public void Ctor_IMediatorNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CardsController(null));
        }
    }
}
