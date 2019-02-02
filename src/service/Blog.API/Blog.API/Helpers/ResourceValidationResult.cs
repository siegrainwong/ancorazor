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

            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
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
}
