; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers.ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
SBBIT0001 | SbBitConverter.SourceGenerator | Error | 特性参数无法解析
SBBIT0002 | SbBitConverter.SourceGenerator | Error | 数组长度或元素尺寸非法
SBBIT0003 | SbBitConverter.SourceGenerator | Error | 结构体未声明 partial
SBBIT0004 | SbBitConverter.SourceGenerator | Error | 嵌套或泛型结构体不受支持
SBBIT0005 | SbBitConverter.SourceGenerator | Info | 标注了 [SbBitConverterStruct] 但无 [FieldOffset] 字段，不生成代码
SBBIT0006 | SbBitConverter.SourceGenerator | Info | 字段类型尺寸无法确定，该字段被跳过
SBBIT0007 | SbBitConverter.SourceGenerator | Error | 同一结构体同时标注两个特性
