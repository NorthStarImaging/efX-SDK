// Copyright (c) 2022 North Star Imaging, Inc.
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
using NSI;

namespace csharpExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string vol_fname = "example.nsihdr";
            uint slice_idx = 39;
            string slice_fname = "slice.tif";

            using (efX.Volume vol = efX.Init())
            {
                Console.WriteLine("SDK Version: " + vol.sdk_version());
                vol.open(vol_fname);
                Console.WriteLine("Open volume: Success");

                Console.WriteLine(vol.slice_width());
                Console.WriteLine(vol.slice_height());
                Console.WriteLine(vol.num_slices());
                Console.WriteLine(vol.data_max());
                Console.WriteLine(vol.data_min());

                float[] slice = vol.read_slice(slice_idx);

                efX.save_tif32(slice_fname, slice, vol.slice_height(), vol.slice_width());
            }

            Console.WriteLine("Done");
        }
    }
}
