using System.Runtime.InteropServices;
using Sb.Extensions.System;
using SbBitConverter.Attributes;

namespace Bench.Models;

// Bench 专用模型（与 SbBitConverter.Tests\Models 保持一致的关键子集）

[SbBitConverterArray(typeof(byte), 64)]
public partial struct Byte64 { }

[SbBitConverterArray(typeof(int), 4)]
public partial struct Int4 { }

[SbBitConverterArray(typeof(int), 4, BigAndSmallEndianEncodingMode.DCBA)]
public partial struct Int4DCBA { }

[SbBitConverterArray(typeof(int), 4, BigAndSmallEndianEncodingMode.ABCD)]
public partial struct Int4ABCD { }
