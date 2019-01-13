using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Search
{
    public static class IndexExtensions
    {
        public static async Task RecreateIndexAsync(this IIndex index, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (index == null) throw new ArgumentNullException(nameof(index));

            // Destroy the index.
            await index.DestroyAsync(cancellationToken).ConfigureAwait(false);

            // Create the index.
            await index.CreateAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
