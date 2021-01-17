using MediatR;
using System.Collections.Generic;

namespace WeKan.Application.Commands.ReorderCards
{
    public class ReorderCardsCommand : IRequest
    {
        public IEnumerable<int> CardIds { get; set; }
    }
}
