using BuildingBlocks.BuildingBlocks;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ValidationException = FluentValidation.ValidationException;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        //Search for class implement ==> IValidator<CreateProductCommand>
       public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            
            var failures=validationResults
                .Where(r=>r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();       

            if (failures.Any())
                throw new ValidationException(failures);

          return  await next();
        }
    }
}
 
