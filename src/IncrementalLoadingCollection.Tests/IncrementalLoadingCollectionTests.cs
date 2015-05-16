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
using System.Linq;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
using Xunit;
using Assert = Xunit.Assert;

namespace UniversalWindowsPlatformToolkit.IncrementalLoadingCollection.Tests
{
    public class IncrementalLoadingCollectionTests
    {
        private static object[] GetSampleData(int count = 10)
        {
            return Enumerable.Range(1, count).Select(i => new object()).ToArray();
        }

        private static IVirtualisedDataSource<T> CreateMockDataSource<T>(IEnumerable<T> data)
        {
            var mockDataSource = Mock.Create<IVirtualisedDataSource<T>>();

            mockDataSource
                .Arrange(source => source.GetItemsAsync(Arg.IsAny<uint>(), Arg.IsAny<uint>()))
                .Returns((uint startIndex, uint count) =>
                {
                    var result = data
                        .Skip((int)startIndex)
                        .Take((int)count);
                    return Task.FromResult(result);
                });
            mockDataSource
                .Arrange(source => source.GetCountAsync())
                .Returns(Task.FromResult(data.Count()));

            return mockDataSource;
        }

        [Fact]
        public void ctor_VirtualisedDataSourceRequired()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new IncrementalLoadingCollection<object>(null));
        }

        [Fact]
        public async Task LoadMoreItemsAsync_OnlySupportsOneLoadOperationAtOnce()
        {
            var loadingTaskSource = new TaskCompletionSource<IEnumerable<object>>();

            var mockDataSource = CreateMockDataSource(GetSampleData());
            mockDataSource
                .Arrange(dataSource => dataSource.GetItemsAsync(Arg.IsAny<uint>(), Arg.IsAny<uint>()))
                .Returns(loadingTaskSource.Task);

            var collection = new IncrementalLoadingCollection<object>(mockDataSource);

            collection.LoadMoreItemsAsync(1);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await collection.LoadMoreItemsAsync(1));
        }

        [Fact]
        public void HasMoreItems_IsTrueByDefault()
        {
            var collection = new IncrementalLoadingCollection<object>(Mock.Create<IVirtualisedDataSource<object>>());

            Assert.Equal(true, collection.HasMoreItems);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task LoadMoreItemsAsync_LoadsExpectedItemsFromDataSource(uint count)
        {
            var data       = GetSampleData();
            var dataSource = CreateMockDataSource(data);
            var collection = new IncrementalLoadingCollection<object>(dataSource);

            await collection.LoadMoreItemsAsync(count);

            var expectedData = data.Take((int)count);

            Assert.Equal(expectedData, collection.ToArray());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public async Task LoadMoreItemsAsync_ReturnsNumberOfItemsLoaded(uint count, uint expectedCount)
        {
            var collection = new IncrementalLoadingCollection<object>(CreateMockDataSource(GetSampleData()));
            var result     = await collection.LoadMoreItemsAsync(count);

            Assert.Equal(expectedCount, result.Count);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public async Task LoadMoreItemsAsync_LoadsMoreExpectedItemsFromDataSource(uint initialCount, uint count)
        {
            var data       = GetSampleData();
            var dataSource = CreateMockDataSource(data);
            var collection = new IncrementalLoadingCollection<object>(dataSource);

            await collection.LoadMoreItemsAsync(initialCount);
            await collection.LoadMoreItemsAsync(count);

            var expectedData = data.Take((int)(initialCount + count));

            Assert.Equal(expectedData, collection.ToArray());
        }

        [Theory]
        [InlineData(1,  false, new uint[] { 1 })]
        [InlineData(2,  true,  new uint[] { 1 })]
        [InlineData(2,  false, new uint[] { 1, 1 })]
        [InlineData(10, true,  new uint[] { 2, 2, 2, 2, 1 })]
        [InlineData(10, false, new uint[] { 2, 2, 2, 2, 2 })]
        public async Task HasMoreItems_ReturnsTrueWhileDataStillInSource(int dataSourceCount, bool expectedHasMoreItems, uint[] countsToLoad)
        {
            var data = GetSampleData(dataSourceCount);
            var dataSource = CreateMockDataSource(data);
            var collection = new IncrementalLoadingCollection<object>(dataSource);

            foreach (var countToLoad in countsToLoad)
            {
                await collection.LoadMoreItemsAsync(countToLoad);
            }

            Assert.Equal(expectedHasMoreItems, collection.HasMoreItems);
        }
    }
}
