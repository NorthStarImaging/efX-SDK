# Copyright (c) 2020 North Star Imaging, Inc.
# All rights reserved.
#
# Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
# following conditions are met:
#   * Redistributions of source code must retain the above copyright notice, this list of conditions and the following
#     disclaimer.
#   * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
#     following disclaimer in the documentation and/or other materials provided with the distribution.
#   * Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote
#     products derived from this software without specific prior written permission.
#
# THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
# INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
# DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
# SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
# SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
# WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
# USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

import os
import sys
import shutil

if __name__ == "__main__":
    # Copy dll into current folder (required by nsiefx)
    currentdir = os.path.dirname(os.path.realpath(__file__))
    rootdir = os.path.dirname(os.path.dirname(currentdir))
    dll = os.path.join(rootdir, "bin", "win64", "efX-SDK.dll")
    shutil.copy(dll, os.path.join(currentdir, "efX-SDK.dll"))
    
    # Copy example .nsihdr into current folder
    parentdir = os.path.dirname(currentdir)
    nsihdr = os.path.join(parentdir, "example.nsihdr")
    shutil.copy(nsihdr, os.path.join(currentdir, "example.nsihdr"))

    # Add includes directory to import path
    includesdir = os.path.join(rootdir, "includes")
    sys.path.append(includesdir)
    import nsiefx

    vol_fname = "example.nsihdr"
    slice_idx = 39
    slice_fname = "slice.tif"

    with nsiefx.open(vol_fname) as volume:
        print(volume.slice_width())
        print(volume.slice_height())
        print(volume.num_slices())
        print(volume.vmin())
        print(volume.vmax())
        print(volume.voxel_size())
        print(volume.data_max())
        print(volume.data_min())

        slice = volume.read_slice(slice_idx)

        nsiefx.save_tif32(slice_fname, slice, volume.slice_height(), volume.slice_width())

    print("Done")
