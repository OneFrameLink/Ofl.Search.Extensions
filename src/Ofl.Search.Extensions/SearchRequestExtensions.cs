using System;

namespace Ofl.Search
{
    public static class SearchRequestExtensions
    {
        public static void Validate(this SearchRequest request)
        {
            // Call the overload.
            request.Validate(nameof(request));
        }

        public static void Validate(this SearchRequest request, string parameter)
        {
            // Validate parameters.
            // NOTE: Order is important here, as first parameter can't be validated with a proper message
            // without second parameter being set properly.
            if (string.IsNullOrWhiteSpace(parameter)) throw new ArgumentNullException(nameof(parameter));
            if (request == null) throw new ArgumentNullException(parameter);

            // Don't validate query, nothing means return nothing.
            if (request.Skip < 0) throw new ArgumentException($"The { nameof(request.Skip) } property on the { parameter } parameter must be a non-negative number.");
            if (request.Take < 0) throw new ArgumentException($"The { nameof(request.Take) } property on the { parameter } parameter must be a non-negative number.");

            // Minimum score.
            if (request.MinimumScore < 0) throw new ArgumentException($"The { nameof(request.MinimumScore)} property on the { parameter } parameter must be a non-negative number.");
        }
    }
}
