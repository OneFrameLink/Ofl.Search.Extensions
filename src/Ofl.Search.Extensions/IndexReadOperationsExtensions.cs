using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Collections.Generic;
using Ofl.Linq;

namespace Ofl.Search
{
    public static class IndexReadOperationsExtensions
    {
        public static Task<IReadOnlyCollection<T>> GetAsync<T>(
            this IIndexReadOperations<T> readOperations,
            CancellationToken cancellationToken, params object[] ids) where T : class =>
            readOperations.GetAsync(ids, cancellationToken);

        public static async Task<IReadOnlyCollection<T>> GetAsync<T>(
            this IIndexReadOperations<T> readOperations, IEnumerable<object> ids,
            CancellationToken cancellationToken) where T : class
        {
            // Validate parameters.
            if (readOperations == null) throw new ArgumentNullException(nameof(readOperations));
            if (ids == null) throw new ArgumentNullException(nameof(ids));

            // Materialize.
            IReadOnlyCollection<object> materializedIds = ids.ToReadOnlyCollection();

            // If no items, return.
            if (materializedIds.Count == 0) return ReadOnlyCollectionExtensions.Empty<T>();

            // Create the request.
            var request = new GetRequest {
                Ids = materializedIds,
                Take = materializedIds.Count
            };

            // Return the response.
            return (await readOperations.GetAsync(request, cancellationToken)
                .ConfigureAwait(false)).Hits.Select(h => h.Item).ToReadOnlyCollection();
        }

        public static async Task<T> GetAsync<T>(
            this IIndexReadOperations<T> readOperations, object id, CancellationToken cancellationToken)
            where T : class =>
                (await readOperations.GetAsync(cancellationToken, id).ConfigureAwait(false))
                    .SingleOrDefault();
    }
}
