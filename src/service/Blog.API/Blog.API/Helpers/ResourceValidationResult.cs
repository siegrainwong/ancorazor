using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.API.Helpers
{
    public class ResourceValidationResult : Dictionary<string, IEnumerable<ResourceValidationError>>
    {
        public ResourceValidationResult() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public ResourceValidationResult(ModelStateDictionary modelState)
            : this()
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            foreach (var (key, value) in modelState)
            {
                var errors = value.Errors;
                if (errors == null || errors.Count <= 0) continue;

                var errorsToAdd = new List<ResourceValidationError>();
                foreach (var error in errors)
                {
                    var keyAndMessage = error.ErrorMessage.Split('|');

                    if (keyAndMessage.Length > 1)
                    {
                        errorsToAdd.Add(new ResourceValidationError(keyAndMessage[1], keyAndMessage[0]));
                    }
                    else
                    {
                        errorsToAdd.Add(new ResourceValidationError(keyAndMessage[0]));
                    }
                }
                Add(key, errorsToAdd);
            }
        }
    }
}
