---
uid: Codebelt.Extensions.Globalization.CultureInfoExtensions
example:
- *content
---
# CultureInfoExtensions

The following example shows how to enrich a culture with Windows NLS formatting information for consumer-facing date and number patterns.

```csharp
using System;
using System.Globalization;
using Codebelt.Extensions.Globalization;

namespace Demo;

public static class Sample
{
    public static void Run()
    {
        CultureInfo culture = CultureInfo.GetCultureInfo("da-DK");
        CultureInfo enriched = culture.UseNationalLanguageSupport();

        Console.WriteLine(enriched.DateTimeFormat.ShortDatePattern);
    }
}
```
