using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSLayer
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, CommandResult>
        where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand,TResponse> : IRequestHandler<TCommand, CommandResult<TResponse>>
        where TCommand : ICommand<TResponse>
    {
    }
}
