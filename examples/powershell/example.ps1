# Define the P/Invoke methods
# Note: The efX-SDK.dll must be in the same directory as this script, and the volume file path must be updated to a valid path
Add-Type @"
using System;
using System.Runtime.InteropServices;
using System.Text;

public class efX
{
    public enum NSI_ERROR
    {
        SUCCESS = 0,
        ERROR_INVALID_FILE = 100,
        ERROR_UNSUPPORTED_HDR_VERSION = 200,
        ERROR_UNSPECIFIED = 9999
    }

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern IntPtr nsi_efx_volume_create();

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern NSI_ERROR nsi_efx_volume_wopen(IntPtr handle, string fname);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_slice_width(IntPtr handle, ref uint width);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_slice_height(IntPtr handle, ref uint height);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_num_slices(IntPtr handle, ref uint num);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_vmin(IntPtr handle, ref double x, ref double y, ref double z);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_vmax(IntPtr handle, ref double x, ref double y, ref double z);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_voxel_size(IntPtr handle, ref double x, ref double y, ref double z);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_data_min(IntPtr handle, ref double min);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_data_max(IntPtr handle, ref double max);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern bool nsi_efx_volume_read_slice(IntPtr handle, float[] slice, uint slice_idx);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern bool nsi_efx_save_gray_tif32_w(string fname, float[] slice, uint num_rows, uint num_cols);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern bool nsi_efx_volume_close(IntPtr handle);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public static extern void nsi_efx_volume_delete(IntPtr handle);

    [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern int nsi_efx_volume_sdk_version(IntPtr handle, StringBuilder buffer, int buffer_length);
}
"@

# Initialize the efX volume
$handle = [efX]::nsi_efx_volume_create()

# Open a volume file
$result = [efX]::nsi_efx_volume_wopen($handle, "example.nsihdr")
if ($result -ne [efX+NSI_ERROR]::SUCCESS) {
    Write-Output "Failed to open file: $result"
    return
}

# Get slice dimensions
[uint32]$width = 0
[uint32]$height = 0
[efX]::nsi_efx_volume_slice_width($handle, [ref]$width)
[efX]::nsi_efx_volume_slice_height($handle, [ref]$height)

Write-Output "Slice Width: $width"
Write-Output "Slice Height: $height"

# Close the volume
if (![efX]::nsi_efx_volume_close($handle)) {
    Write-Error "Failed to close file"
}

# Delete the handle
[efX]::nsi_efx_volume_delete($handle)

Write-Output "Operation complete"
