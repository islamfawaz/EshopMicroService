using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.BuildingBlocks
{

    public interface ICommand:ICommand<Unit>
    {
        
    }
    public interface ICommand<out TResponse> :IRequest<TResponse>
    {
    }
}
