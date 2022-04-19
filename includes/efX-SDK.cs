// Copyright (c) 2020 North Star Imaging, Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
// following conditions are met:
//   * Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//     disclaimer.
//   * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
//     following disclaimer in the documentation and/or other materials provided with the distribution.
//   * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote
//     products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
// USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Runtime.InteropServices;

namespace NSI
{
    class efX
    {

        public enum NSI_ERROR
        {
            SUCCESS = 0,
            ERROR_INVALID_FILE = 100,
            ERROR_UNSUPPORTED_HDR_VERSION = 200,
            ERROR_UNSPECIFIED = 9999
        }

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static IntPtr nsi_efx_volume_create();

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static NSI_ERROR nsi_efx_volume_wopen(IntPtr handle, string fname);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_slice_width(IntPtr handle, ref UInt32 width);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_slice_height(IntPtr handle, ref UInt32 height);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_num_slices(IntPtr handle, ref UInt32 num);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_vmin(IntPtr handle, ref double x, ref double y, ref double z);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_vmax(IntPtr handle, ref double x, ref double y, ref double z);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_voxel_size(IntPtr handle, ref double x, ref double y, ref double z);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_data_min(IntPtr handle, ref double min);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_data_max(IntPtr handle, ref double max);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static bool nsi_efx_volume_read_slice(IntPtr handle, [In, Out] float[] slice, UInt32 slice_idx);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static bool nsi_efx_save_gray_tif32_w(string fname, [In, Out] float[] slice, UInt32 num_rows, UInt32 num_cols);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static bool nsi_efx_volume_close(IntPtr handle);

        [DllImport("efX-SDK.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private extern static void nsi_efx_volume_delete(IntPtr handle);

        public struct Vec3d
        {
            public double x;
            public double y;
            public double z;
        }

        public class Volume : IDisposable
        {
            private IntPtr handle;
            internal Volume(IntPtr handle)
            {
                this.handle = handle;
            }

            public void Dispose()
            {
                nsi_efx_volume_delete(handle);
                handle = IntPtr.Zero;
            }

            public void open(string fname)
            {
                NSI_ERROR err = nsi_efx_volume_wopen(handle, fname);
                if (err != NSI_ERROR.SUCCESS)
                    throw new System.IO.IOException("Failed to open file", (int)err);
            }

            public void close()
            {
                if (!nsi_efx_volume_close(handle))
                    throw new System.IO.IOException("Failed to close file");
            }

            public uint slice_width()
            {
                uint width = 0;
                nsi_efx_volume_slice_width(handle, ref width);
                return width;
            }

            public uint slice_height()
            {
                uint height = 0;
                nsi_efx_volume_slice_height(handle, ref height);
                return height;
            }

            public uint num_slices()
            {
                uint num = 0;
                nsi_efx_volume_num_slices(handle, ref num);
                return num;
            }

            public Vec3d vmin()
            {
                Vec3d min = new Vec3d();
                nsi_efx_volume_vmin(handle, ref min.x, ref min.y, ref min.z);
                return min;
            }

            public Vec3d vmax()
            {
                Vec3d max = new Vec3d();
                nsi_efx_volume_vmax(handle, ref max.x, ref max.y, ref max.z);
                return max;
            }

            public Vec3d voxel_size()
            {
                Vec3d size = new Vec3d();
                nsi_efx_volume_voxel_size(handle, ref size.x, ref size.y, ref size.z);
                return size;
            }

            public double data_min()
            {
                double min = 0;
                nsi_efx_volume_data_min(handle, ref min);
                return min;
            }

            public double data_max()
            {
                double max = 0;
                nsi_efx_volume_data_max(handle, ref max);
                return max;
            }

            public float[] read_slice(uint sliceidx)
            {
                float[] slice = new float[slice_width() * slice_height()];
                if (!nsi_efx_volume_read_slice(handle, slice, sliceidx))
                    throw new System.IO.IOException("Failed to read slice");
                return slice;
            }
        }

        public static Volume Init()
        {
            return new Volume(nsi_efx_volume_create());
        }

        public static void save_tif32(string slice_fname, float[] slice, uint slice_height, uint slice_width)
        {
            if (!nsi_efx_save_gray_tif32_w(slice_fname, slice, slice_height, slice_width))
                throw new System.IO.IOException("Failed to save tif");
        }
    }
}
