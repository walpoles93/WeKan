﻿using MediatR;

namespace WeKan.Application.Commands.AddCardToBoard
{
    public class AddCardToBoardCommand : IRequest
    {
        public string Title { get; set; }
        public int BoardId { get; set; }
    }
}
