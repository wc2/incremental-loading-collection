/*
Copyright 2015 William Cowell Consulting Limited.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace UniversalWindowsPlatformToolkit.IncrementalLoadingCollection
{
    public class IncrementalLoadingCollection<T> :
        ObservableCollection<T>,
        IIncrementalLoadingCollection<T>
    {
        private readonly IVirtualisedDataSource<T> _dataSource;
        private int? _dataSourceCount;
        private bool _isLoading;

        public IncrementalLoadingCollection(IVirtualisedDataSource<T> dataSource)
        {
            _dataSource = dataSource;
            if (dataSource == null)
            {
                throw new ArgumentNullException("dataSource", "Data Source is required.");
            }
        }

        private uint AddRange(IEnumerable<T> items)
        {
            uint count = 0;

            foreach (var item in items)
            {
                Add(item);
                ++count;
            }

            return count;
        }

        private async Task EnsureDataSourceHasBeenCount()
        {
            if (!_dataSourceCount.HasValue)
            {
                _dataSourceCount = await _dataSource.GetCountAsync();
            }
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsFromDataSourceAsync(uint count)
        {
            var result = new LoadMoreItemsResult();
            _isLoading = true;

            try
            {
                await EnsureDataSourceHasBeenCount();

                var startIndex = (uint)Count;
                var itemsToAdd = await _dataSource.GetItemsAsync(startIndex, count);
                var itemsAddedCount = AddRange(itemsToAdd);

                _hasMoreItems = (_dataSourceCount > Count);

                result.Count = itemsAddedCount;
            }
            finally
            {
                _isLoading = false;
            }

            return result;
        }

        private bool _hasMoreItems = true;
        public bool HasMoreItems { get { return _hasMoreItems; } }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_isLoading)
            {
                throw new InvalidOperationException();
            }
            return AsyncInfo.Run(token => LoadMoreItemsFromDataSourceAsync(count));
        }
    }
}
