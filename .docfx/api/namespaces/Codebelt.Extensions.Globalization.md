---
uid: Codebelt.Extensions.Globalization
summary: *content
---
# Codebelt.Extensions.Globalization

The `Codebelt.Extensions.Globalization` namespace helps you enrich `CultureInfo` instances with Windows NLS formatting behavior when you need culture-aware date and number patterns that behave more like the Windows runtime than the ICU defaults.

Use it when you want to adapt a culture for consumer-facing formatting in applications that rely on Windows-style patterns, especially for `DateTimeFormat` and `NumberFormat` values.

If you are trying to switch a culture to NLS-style formatting, start with `CultureInfoExtensions`.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Globalization namespace](https://docs.microsoft.com/en-us/dotnet/api/system.globalization) 🔗

## Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|`CultureInfo`|⬇️|`UseNationalLanguageSupport()`|
|`IEnumerable<CultureInfo>`|⬇️|`UseNationalLanguageSupport()`|

### Example

```csharp
using System;
using System.Globalization;
using Codebelt.Extensions.Globalization;

namespace Demo;

public static class Sample
{
    public static void Run()
    {
        var danishCultureIcu = new CultureInfo("da-dk", false);
        var danishCultureNls = new CultureInfo("da-dk", false).UseNationalLanguageSupport();


        Console.WriteLine(danishCultureIcu.DateTimeFormat.ShortDatePattern); // danishCultureIcu outputs dd.MM.yyyy from danishCultureIcu.DateTimeFormat.ShortDatePattern
        Console.WriteLine(danishCultureNls.DateTimeFormat.ShortDatePattern); // danishCultureNls outputs dd-MM-yyyy from danishCultureNls.DateTimeFormat.ShortDatePattern
    }
}
```
