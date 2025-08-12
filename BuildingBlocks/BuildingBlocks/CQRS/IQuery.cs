using MediatR;

namespace BuildingBlocks.BuildingBlocks
{
    public interface IQuery<out TResponse> :IRequest<TResponse>
        where TResponse : notnull
    {
    }
}
