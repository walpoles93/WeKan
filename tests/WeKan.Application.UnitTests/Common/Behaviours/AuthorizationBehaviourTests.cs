using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Commands.CreateBoard;
using WeKan.Application.Common.Behaviours;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;
using Xunit;

namespace WeKan.Application.UnitTests.Common.Behaviours
{
    public class AuthorizationBehaviourTests
    {
        private readonly Mock<RequestHandlerDelegate<BoardCreatedDto>> _next;
        private readonly Mock<IAuthorizer<CreateBoardCommand>> _authoriser;

        public AuthorizationBehaviourTests()
        {
            _next = new Mock<RequestHandlerDelegate<BoardCreatedDto>>();
            _authoriser = new Mock<IAuthorizer<CreateBoardCommand>>();
        }

        [Fact]
        public void Ctor_IAuthorisersNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AuthorizationBehaviour<CreateBoardCommand, BoardCreatedDto>(null));
        }

        [Fact]
        public async Task Handle_AuthorisersEmpty_CallsNext()
        {
            var behaviour = new AuthorizationBehaviour<CreateBoardCommand, BoardCreatedDto>(new List<IAuthorizer<CreateBoardCommand>>());

            await behaviour.Handle(new CreateBoardCommand(), new CancellationToken(), _next.Object);

            _next.Verify(next => next(), Times.Once);
        }

        [Fact]
        public async Task Handle_AuthoriserReturnsFalse_ThrowsUnauthorisedApplicationException()
        {
            _authoriser.Setup(a => a.Authorise(It.IsAny<CreateBoardCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(false));
            var authorisers = new List<IAuthorizer<CreateBoardCommand>> { _authoriser.Object };
            var behaviour = new AuthorizationBehaviour<CreateBoardCommand, BoardCreatedDto>(authorisers);

            Task action() => behaviour.Handle(new CreateBoardCommand(), new CancellationToken(), _next.Object);

            await Assert.ThrowsAsync<UnauthorisedApplicationException>(action);
        }

        [Fact]
        public async Task Handler_AuthoriserReturnsTrue_CallsNext()
        {
            _authoriser.Setup(a => a.Authorise(It.IsAny<CreateBoardCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var authorisers = new List<IAuthorizer<CreateBoardCommand>> { _authoriser.Object };
            var behaviour = new AuthorizationBehaviour<CreateBoardCommand, BoardCreatedDto>(authorisers);

            await behaviour.Handle(new CreateBoardCommand(), new CancellationToken(), _next.Object);

            _next.Verify(next => next(), Times.Once);
        }
    }
}
