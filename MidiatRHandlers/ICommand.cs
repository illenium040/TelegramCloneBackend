using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSLayer
{
    public interface ICommand : IRequest<CommandResult>
    {
    }
    public interface ICommand<TResponse> : IRequest<CommandResult<TResponse>>
    {

    }
}
