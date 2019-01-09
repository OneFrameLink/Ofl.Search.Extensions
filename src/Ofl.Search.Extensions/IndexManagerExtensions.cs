using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Search
{
    public static class IndexManagerExtensions
    {
        public static async Task UpsertAsync<T>(this IIndexManager indexManager, string indexName, IEnumerable<T> source,
            CancellationToken cancellationToken)
            where T : class
        {
            // Validate parameters.
            if (indexManager == null) throw new ArgumentNullException(nameof(indexManager));
            if (string.IsNullOrWhiteSpace(indexName)) throw new ArgumentNullException(nameof(indexName));
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Get the index, cast to the type of T.
            var index = (IIndex<T>) await indexManager.GetIndexAsync(indexName, cancellationToken).ConfigureAwait(false);

            // Get the write operations.
            IIndexWriteOperations<T> wo = await index.GetWriteOperationsAsync(cancellationToken).ConfigureAwait(false);

            // Update.
            await wo.UpsertAsync(source, cancellationToken).ConfigureAwait(false);
        }
        public static async Task<SearchResponse<T>> SearchAsync<T>(this IIndexManager indexManager, string indexName, SearchRequest request,
            CancellationToken cancellationToken)
            where T : class
        {
            // Validate parameters.
            if (indexManager == null) throw new ArgumentNullException(nameof(indexManager));
            if (string.IsNullOrWhiteSpace(indexName)) throw new ArgumentNullException(nameof(indexName));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Get the index.
            var index = (IIndex<T>) await indexManager.GetIndexAsync(indexName, cancellationToken).ConfigureAwait(false);

            // Get the read operations.
            IIndexReadOperations<T> ro = await index.GetReadOperationsAsync(cancellationToken).ConfigureAwait(false);

            // Search.
            return await ro.SearchAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
