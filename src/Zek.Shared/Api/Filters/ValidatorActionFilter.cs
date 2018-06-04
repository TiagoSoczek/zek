using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Zek.Shared.Api.Dtos;
using Zek.Shared.Resources;

namespace Zek.Shared.Api.Filters
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var issues = new Dictionary<string, string[]>();

            foreach (var modelState in context.ModelState)
            {
                if (modelState.Value.Errors.Count > 0)
                {
                    issues[modelState.Key] = modelState.Value.Errors.Select(GetValidationMessage).ToArray();
                }
            }

            var resultDto = new ErrorResultDto
            {
                Error = Messages.ValidationError,
                Issues = issues
            };

            context.Result = new BadRequestObjectResult(resultDto);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed
        }

        private string GetValidationMessage(ModelError error)
        {
            if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
            {
                return error.ErrorMessage;
            }

            if (error.Exception != null)
            {
                return error.Exception.Message;
            }

            return Messages.UnknownValidationError;
        }
    }
}