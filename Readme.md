# NSI efX SDK 1.0
 
Software Development Kit for reading slices from .nsivol and .nsihdr file formats.

Includes bindings for C, C++, C#, and Python.

## Usage

Documentation is in C++ and examples are available for C++, C#, and Python.

1. Import SDK
```c++
#include "efX_SDK.h"
using namespace NSI::efX;
```

2. Create a volume accessor
```c++
Volume* vol = Volume::Create();
```

3. Open a .nsivol or .nsihdr file
```c++
vol->open(L"example.nsihdr");
```

4. Access properties of the volume and allocate a buffer for the slices
```c++
float* slice = new float[vol->slice_width() * vol->slice_height()];
```

5. Read a specific slice
```c++
// Index ranges from 0 to vol->num_slices()-1
vol->read_slice(slice, 100); // Read 100th slice
```

6. Cleanup when done using slice and volume
```c++
delete vol;
delete[] slice;
```

## Copyright 2020 North Star Imaging
Licensed under BSD-3-Clause. See [LICENSE](LICENSE) for more information.
