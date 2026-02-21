using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Codebelt.Extensions.Globalization;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Extensions.IO;
using Cuemon.Globalization;
using Cuemon.Reflection;
using YamlDotNet.Serialization.NamingConventions;

namespace gse
{
    internal class Program
    {
        private static readonly string SurrogatesPath;
        private static readonly string SurrogatesPathRaw;

        static Program()
        {
            var assemblyPath = typeof(Program).Assembly.Location;
            SurrogatesPath = Path.GetFullPath(Path.Combine(assemblyPath, "..", "..", "..", "..", "Surrogates"));
            SurrogatesPathRaw = Path.Combine(SurrogatesPath, "raw");
            Directory.CreateDirectory(SurrogatesPath);
            Directory.CreateDirectory(SurrogatesPathRaw);
        }

        static void Main(string[] args)
        {
            var regions = World.Regions;
            foreach (var region in regions)
            {
                var cultureInfos = World.GetCultures(region);
                foreach (var cultureInfo in cultureInfos)
                {
                    var dtSurrogate = new DateTimeFormatInfoSurrogate(cultureInfo.DateTimeFormat);
                    var nfSurrogate = new NumberFormatInfoSurrogate(cultureInfo.NumberFormat);
                    var ciSurrogate = new CultureInfoSurrogate(dtSurrogate, nfSurrogate);

                    var ms = YamlFormatter.SerializeObject(ciSurrogate, o =>
                    {
                        o.Settings.NamingConvention = NullNamingConvention.Instance;
                        o.Settings.ReflectionRules = new MemberReflection();
                        o.Settings.IndentSequences = false;
                    });

                    using var fsRawYaml =
                        new FileStream(Path.Combine(SurrogatesPathRaw, $"{cultureInfo.Name.ToLowerInvariant()}.yml"),
                            FileMode.Create);
                    fsRawYaml.Write(ms.ToByteArray(o => o.LeaveOpen = true), 0, (int)ms.Length);
                    fsRawYaml.Flush();

                    using var cms = ms.CompressGZip();
                    using var fs =
                        new FileStream(Path.Combine(SurrogatesPath, $"{cultureInfo.Name.ToLowerInvariant()}.bin"),
                            FileMode.Create);
                    fs.Write(cms.ToByteArray(o => o.LeaveOpen = true), 0, (int)cms.Length);
                    fs.Flush();
                }
            }

            WriteIcuNamedNlsAlternatives();
        }

        private static void WriteIcuNamedNlsAlternatives()
        {
            // Maps ICU-only culture names (absent from Windows NLS) to their nearest NLS equivalent.
            // When a surrogate is written for the NLS name, a copy is also written under the ICU name
            // so that ICU-based environments can resolve it from the same embedded resource set.
            var icuToNls = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // Script subtag added in NLS
                { "bgc-IN",       "bgc-Deva-IN"  },
                { "bho-IN",       "bho-Deva-IN"  },
                { "bm-ML",        "bm-Latn-ML"   },
                { "ccp-BD",       "ccp-Cakm-BD"  },
                { "ccp-IN",       "ccp-Cakm-IN"  },
                { "ceb-PH",       "ceb-Latn-PH"  },
                { "chr-US",       "chr-Cher-US"  },
                { "cv-RU",        "cv-Cyrl-RU"   },
                { "doi-IN",       "doi-Deva-IN"  },
                { "ha-GH",        "ha-Latn-GH"   },
                { "ha-NE",        "ha-Latn-NE"   },
                { "ha-NG",        "ha-Latn-NG"   },
                { "iu-CA",        "iu-Cans-CA"   }, // ICU defaults to Unified Canadian Aboriginal Syllabics
                { "jv-ID",        "jv-Latn-ID"   }, // ICU defaults to Latin script
                { "kgp-BR",       "kgp-Latn-BR"  },
                { "pcm-NG",       "pcm-Latn-NG"  },
                { "quc-GT",       "quc-Latn-GT"  },
                { "raj-IN",       "raj-Deva-IN"  },
                { "sc-IT",        "sc-Latn-IT"   },
                { "sn-ZW",        "sn-Latn-ZW"   },
                { "tg-TJ",        "tg-Cyrl-TJ"   },
                { "tzm-DZ",       "tzm-Latn-DZ"  },
                { "tzm-MA",       "tzm-Tfng-MA"  }, // ICU defaults to Tifinagh script
                { "yrl-BR",       "yrl-Latn-BR"  },
                { "yrl-CO",       "yrl-Latn-CO"  },
                { "yrl-VE",       "yrl-Latn-VE"  },
                { "zgh-MA",       "zgh-Tfng-MA"  },
                // Script subtag dropped in NLS
                { "mni-Beng-IN",  "mni-IN"       },
                { "pa-Guru-IN",   "pa-IN"        },
                { "zh-Hans-CN",   "zh-CN"        },
                { "zh-Hans-SG",   "zh-SG"        },
                { "zh-Hant-HK",   "zh-HK"        },
                { "zh-Hant-MO",   "zh-MO"        },
                { "zh-Hant-TW",   "zh-TW"        },
                // Language code differs between ICU (CLDR) and NLS
                { "ckb-IQ",       "ku-Arab-IQ"   }, // Sorani Kurdish → ku in NLS
                { "ckb-IR",       "ku-Arab-IR"   },
                { "qu-BO",        "quz-BO"       }, // Quechua → quz in NLS
                { "qu-EC",        "quz-EC"       },
                { "qu-PE",        "quz-PE"       },
                // POSIX variant not present in NLS
                { "en-US-POSIX",  "en-US"        },
            };

            foreach (var (icuName, nlsName) in icuToNls)
            {
                var sourceBin = Path.Combine(SurrogatesPath, $"{nlsName.ToLowerInvariant()}.bin");
                var sourceYml = Path.Combine(SurrogatesPathRaw, $"{nlsName.ToLowerInvariant()}.yml");

                if (File.Exists(sourceBin))
                {
                    File.Copy(sourceBin, Path.Combine(SurrogatesPath, $"{icuName.ToLowerInvariant()}.bin"), overwrite: true);
                }

                if (File.Exists(sourceYml))
                {
                    File.Copy(sourceYml, Path.Combine(SurrogatesPathRaw, $"{icuName.ToLowerInvariant()}.yml"), overwrite: true);
                }
            }
        }
    }
}
