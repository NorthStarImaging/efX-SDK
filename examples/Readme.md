# SDK Examples

The examples included here all read `example.nsihdr` and output a slice file that matches `example_output.tif`:

![Example output](example_output.png)

Every example expects `efX-SDK.dll` and `example.nsihdr` to be in the example's current directory, and the examples attempt to handle this automatically.

The python example copies the above files into its example folder when it is run.

The C, C++, and C# examples use a prebuild command included in the `*.*proj` files to copy their dependencies into the correct location.
