# Incremental Loading Collection

The **IncrementalLoadingCollection\<T\>** is a simple collection designed to facilitate incremental data virtualisation in Windows Universal apps through the implementation of the [ISupportIncrementalLoading](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.ui.xaml.data.isupportincrementalloading.aspx) interface. It is a generic class and is designed to be used in place of an [ObservableCollection\<T\>](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/ms668604(v=vs.140).aspx).

To use the **IncrementalLoadingCollection\<T\>** you must provide the class with a data source that is capable of loading data sequentially by implementing the **IVirtualisedDataSource\<T\>**.

See [Getting Started](https://github.com/wc2/incremental-loading-collection/wiki/Getting-Started) for details on how to use the **IncrementalLoadingCollection\<T\>** in your own apps.

## Further Reading

You can find out more about UI and data virtualisation by reading [Using virtualization (sic) with a list of grid (XAML)](https://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh780657.aspx) on MSDN.