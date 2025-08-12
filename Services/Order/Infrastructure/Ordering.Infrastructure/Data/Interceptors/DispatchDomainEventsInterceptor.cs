//using MediatR;
//using Microsoft.EntityFrameworkCore.Diagnostics;
//using System.Threading.Tasks;

//namespace Ordering.Infrastructure.Data.Interceptors
//{
//    public class DispatchDomainEventsInterceptor(IMediator mediator) :SaveChangesInterceptor
//    {
//        public override  InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
//        {
//            DispatchDomainEvents(eventData.Context!).GetAwaiter().GetResult();
//            return base.SavingChanges(eventData, result);
//        }

//        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
//        {
//           await  DispatchDomainEvents(eventData.Context!);

//            return await base.SavingChangesAsync(eventData, result, cancellationToken);
//        }

//        public async Task DispatchDomainEvents(DbContext context)
//        {
//            if (context is null) return;

//            var aggregate = context.ChangeTracker
//                .Entries<IAggregate>()
//                .Where(e => e.Entity.DomainEvents.Any())
//                .Select(e => e.Entity);

//            var domainEvents=aggregate.
//                SelectMany(e=>e.DomainEvents)
//                .ToList();

//            aggregate.ToList().ForEach(e => e.ClearDomainEvents());

//            foreach (var domainEvent in domainEvents)
//            {
//                await mediator.Publish(domainEvent);
//            }

//        }

//    }
//}
