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

#ifndef CXX_NSI_EFX_SDK_H
#define CXX_NSI_EFX_SDK_H

#if _MSC_VER > 1000
#pragma once
#endif


#include <stdbool.h>
#include <stdint.h>


#ifdef NSI_EFX_SDK_EXPORTS
#define EFX_API __declspec(dllexport)
#else
#define EFX_API __declspec(dllimport)
#endif

typedef enum {
	// No errors
	NSI_SUCCESS = 0,
	// File does not exist, is not readable, or is an invalid format
	NSI_ERROR_INVALID_FILE = 100,
	// .nsihdr file is a version newer than what the SDK supports (SDK requires updating)
	NSI_ERROR_UNSUPPORTED_HDR_VERSION = 200,
	// Other
	NSI_ERROR_UNSPECIFIED = 9999
} NSI_ERROR;

#ifdef __cplusplus
namespace NSI {
namespace efX {


class EFX_API Vec3_d
{
public:
	double x;
	double y;
	double z;
	Vec3_d();
	Vec3_d(double x, double y, double z);
};

class EFX_API Volume
{
public:
	// resolution (voxels)
	virtual uint32_t slice_width() const = 0;
	virtual uint32_t slice_height() const = 0;
	virtual uint32_t num_slices() const = 0;

	// min point (mm)
	virtual Vec3_d vmin() const = 0;

	// max point (mm)
	virtual Vec3_d vmax() const = 0;

	// voxel size (mm)
	virtual Vec3_d voxel_size() const = 0;

	// minimum voxel value
	virtual double data_min() const = 0;

	// maximum voxel value
	virtual double data_max() const = 0;

	static Volume* Create();

	virtual ~Volume() = 0;

	virtual void destroy() = 0;

	// open an nsihdr
	virtual NSI_ERROR open(const char* fname) = 0;

	// open an nsihdr
	virtual NSI_ERROR open(const wchar_t* fname) = 0;

	// read a y-slice (rows X cols == res.i X res.k)
	virtual bool read_slice(float* slice, uint32_t slice_idx) = 0;

	// close an nsihdr
	virtual bool close() = 0;

	// read the loaded SDK version string into the buffer and return the length
	virtual uint32_t sdk_version(char* buffer, uint32_t buffer_length) = 0;
};


}} // end namespace NSI::efX


extern "C"
{
	typedef NSI::efX::Volume NSIVolume;
#else
    typedef struct NSIVolume NSIVolume;
#endif
	EFX_API
	// create a volume handle
	NSIVolume* nsi_efx_volume_create();

	EFX_API
	// delete a volume handle (closes nsihdr, if opened)
	void nsi_efx_volume_delete(NSIVolume* handle);

	EFX_API
	// open an nsihdr
	NSI_ERROR nsi_efx_volume_open(NSIVolume* handle, const char* fname);

	EFX_API
	// open an nsihdr
	NSI_ERROR nsi_efx_volume_wopen(NSIVolume* handle, const wchar_t* fname);

	// resolution (voxels)
	EFX_API void nsi_efx_volume_slice_width(NSIVolume* handle, uint32_t* w);
	EFX_API void nsi_efx_volume_slice_height(NSIVolume* handle, uint32_t* h);
	EFX_API void nsi_efx_volume_num_slices(NSIVolume* handle, uint32_t* n);

	EFX_API
	// min point (mm)
	void nsi_efx_volume_vmin(NSIVolume* handle, double* x, double* y, double* z);

	EFX_API
	// max point (mm)
	void nsi_efx_volume_vmax(NSIVolume* handle, double* x, double* y, double* z);

	EFX_API
	// voxel size (mm)
	void nsi_efx_volume_voxel_size(NSIVolume* handle, double* x, double* y, double* z);

	EFX_API
	// minimum voxel value
	void nsi_efx_volume_data_min(NSIVolume* handle, double* min);

	EFX_API
	// maximum voxel value
	void nsi_efx_volume_data_max(NSIVolume* handle, double* max);

	EFX_API
	// read a y-slice (rows X cols == res.i X res.k)
	bool nsi_efx_volume_read_slice(NSIVolume* handle, float* slice, uint32_t slice_idx);

	EFX_API
	// write a slice as a 32-bit gray tif 
	bool nsi_efx_save_gray_tif32(const char* fname, float* slice, uint32_t num_rows, uint32_t num_cols);

	EFX_API
	// write a slice as a 32-bit gray tif 
	bool nsi_efx_save_gray_tif32_w(const wchar_t* fname, float* slice, uint32_t num_rows, uint32_t num_cols);

	EFX_API
	// close an nsihdr
	bool nsi_efx_volume_close(NSIVolume* handle);

	EFX_API
	// get the loaded SDK version
	uint32_t nsi_efx_volume_sdk_version(NSIVolume* handle, char* buffer, uint32_t buffer_length);
#ifdef __cplusplus
}
#endif

#endif
