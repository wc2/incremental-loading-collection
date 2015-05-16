using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalWindowsPlatformToolkit.IncrementalLoadingCollection;

namespace IncrementalLoadingCollection.SampleApp
{
    public class GeneratingDataSource : IVirtualisedDataSource<int>
    {
        private readonly int _count;

        public GeneratingDataSource(int count = 1000000)
        {
            _count = count;
        }

        public Task<int> GetCountAsync()
        {
            return Task.FromResult(_count);
        }

        public Task<IEnumerable<int>> GetItemsAsync(uint startIndex, uint count)
        {
            return Task.FromResult(Enumerable.Range((int)startIndex, (int)count));
        }
    }
}
