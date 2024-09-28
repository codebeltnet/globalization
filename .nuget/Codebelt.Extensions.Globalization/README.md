## About

An open-source project (MIT license) that targets and complements the [System.Globalization](https://github.com/dotnet/runtime/tree/main/src/libraries/System.Private.CoreLib/src/System/Globalization) namespace. It aims to provide a way to favor National Language Support (NLS) over International Components for Unicode (ICU).

It is, by heart, free, flexible and built to extend and boost your agile codebelt.

## **Codebelt.Extensions.Globalization** for .NET

The `Codebelt.Extensions.Globalization` namespace contains extension methods that is an addition to the `System.Globalization` namespace.

More documentation available at our documentation site:

- [Codebelt.Extensions.Globalization](https://globalization.codebelt.net/api/Codebelt.Extensions.Globalization.html) 🔗

### CSharp Example

```csharp
var danishCultureIcu = new CultureInfo("da-dk", false);
var danishCultureNls = new CultureInfo("da-dk", false).UseNationalLanguageSupport();

// danishCultureIcu outputs dd.MM.yyyy from danishCultureIcu.DateTimeFormat.ShortDatePattern
// danishCultureNls outputs dd-MM-yyyy from danishCultureNls.DateTimeFormat.ShortDatePattern
```
