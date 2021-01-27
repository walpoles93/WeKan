using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeKan.Application.Common.Exceptions;
using WeKan.Application.Common.Interfaces;

namespace WeKan.Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IAuthorizer<TRequest>> _authorisers;

        public AuthorizationBehaviour(IEnumerable<IAuthorizer<TRequest>> authorisers)
        {
            _authorisers = authorisers ?? throw new ArgumentNullException(nameof(authorisers));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_authorisers.Any())
            {
                foreach (var authoriser in _authorisers)
                {
                    var authorised = await authoriser.Authorise(request, cancellationToken);

                    if (!authorised) throw new UnauthorisedApplicationException("Current user is not authorised to perform this action");
                }
            }

            return await next();
        }
    }
}
