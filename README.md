# Incremental Loading Collection

The **IncrementalLoadingCollection\<T\>** is a simple collection designed to facilitate incremental data virtualisation in Windows Universal apps through the implementation of the [ISupportIncrementalLoading](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.ui.xaml.data.isupportincrementalloading.aspx) interface. You can use it in place of an **ObservableCollection\<T\>** as an [ItemsSource](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.ui.xaml.controls.itemscontrol.itemssource.aspx) for a ListView or GridView control in any Windows Universal app.

To use the **IncrementalLoadingCollection\<T\>** you must provide the class with a data source that is capable of loading data sequentially by implementing the **IVirtualisedDataSource\<T\>**.

See [Getting Started](https://github.com/wc2/incremental-loading-collection/wiki/Getting-Started) for details on how to use the **IncrementalLoadingCollection\<T\>** in your own apps.

## NuGet

The **Incremental Loading Collection** can be downloaded from GitHub, but it is recommended that you install it via [NuGet](https://www.nuget.org/packages/UniversalWindowsPlatformToolkit.IncrementalLoadingCollection/)

## License

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this software except in compliance with the License. You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the [License](https://github.com/wc2/incremental-loading-collection/blob/master/LICENSE) for the specific language governing permissions and limitations under the License.

## Further Reading

You can find out more about UI and data virtualisation by reading [Using virtualization (sic) with a list of grid (XAML)](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh780657.aspx) on MSDN.