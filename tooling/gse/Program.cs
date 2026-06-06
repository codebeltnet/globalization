using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (var cultureInfo in cultureInfos)
            {
                WriteSurrogate(cultureInfo);
            }

            WriteIcuNamedNlsAlternatives();
        }

        private static void WriteSurrogate(CultureInfo cultureInfo)
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

            var name = cultureInfo.Name.ToLowerInvariant();

            using var fsRawYaml = new FileStream(Path.Combine(SurrogatesPathRaw, $"{name}.yml"), FileMode.Create);
            fsRawYaml.Write(ms.ToByteArray(o => o.LeaveOpen = true), 0, (int)ms.Length);
            fsRawYaml.Flush();

            using var cms = ms.CompressGZip();
            using var fs = new FileStream(Path.Combine(SurrogatesPath, $"{name}.bin"), FileMode.Create);
            fs.Write(cms.ToByteArray(o => o.LeaveOpen = true), 0, (int)cms.Length);
            fs.Flush();
        }

        private static bool TryWriteNlsSurrogate(string cultureName)
        {
            // Attempt to resolve the culture under the current NLS runtime.
            // Returns true and writes a native surrogate if Windows NLS knows the culture;
            // returns false if the culture is genuinely ICU-only (CultureNotFoundException).
            try
            {
                var ci = new CultureInfo(cultureName);

                // Guard: Windows may silently substitute InvariantCulture (LCID 0x7F) for
                // unknown names instead of throwing — treat that as "not natively supported".
                if (ci.LCID == CultureInfo.InvariantCulture.LCID) return false;

                // Guard: if Windows normalised the name to something else, the surrogate
                // would already be covered by a different entry.
                if (!ci.Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase)) return false;

                if (ci.DisplayName.StartsWith("Unknown", StringComparison.OrdinalIgnoreCase)) return false;

                WriteSurrogate(ci);
                return true;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
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
                // Script/language aliases newly surfaced by macOS 26 ICU
                { "ber-Latn-MA",  "tzm-Latn-MA"  },
                { "ber-Tfng-MA",  "tzm-Tfng-MA"  },
                { "kk-Arab-CN",   "kk-KZ"        },
                { "kk-Cyrl-KZ",   "kk-KZ"        },
                { "kok-Deva-IN",  "kok-IN"       },
                { "kok-Latn-IN",  "kok-IN"       },
                { "ks-Aran-IN",   "ks-Arab-IN"   },
                { "ku-Latn-IQ",   "ku-Arab-IQ"   },
                { "mni-Mtei-IN",  "mni-IN"       },
                { "ms-Arab-BN",   "ms-BN"        },
                { "ms-Arab-MY",   "ms-MY"        },
                { "pa-Aran-PK",   "pa-Arab-PK"   },
                { "sat-Deva-IN",  "sat-Olck-IN"  },
                { "ur-Arab-IN",   "ur-IN"        },
                { "ur-Arab-PK",   "ur-PK"        },
                { "ur-Aran-IN",   "ur-IN"        },
                { "ur-Aran-PK",   "ur-PK"        },
                { "yue-Hant-CN",  "zh-CN"        },
                { "yue-Hant-MO",  "zh-MO"        },
                { "zh-Hans-JP",   "zh-CN"        },
                { "zh-Hans-MY",   "zh-SG"        },
                { "zh-Hant-CN",   "zh-CN"        },
                { "zh-Hant-JP",   "zh-HK"        },
                { "zh-Hant-MY",   "zh-HK"        },
                // English regional fallbacks for macOS 26 ICU-only cultures
                { "en-AL",        "en-001"       },
                { "en-AR",        "en-001"       },
                { "en-BD",        "en-001"       },
                { "en-BG",        "en-001"       },
                { "en-BN",        "en-001"       },
                { "en-BR",        "en-001"       },
                { "en-CL",        "en-001"       },
                { "en-CN",        "en-001"       },
                { "en-CO",        "en-001"       },
                { "en-CV",        "en-001"       },
                { "en-CZ",        "en-001"       },
                { "en-EE",        "en-001"       },
                { "en-ES",        "en-001"       },
                { "en-FR",        "en-001"       },
                { "en-GE",        "en-001"       },
                { "en-GR",        "en-001"       },
                { "en-GS",        "en-001"       },
                { "en-HU",        "en-001"       },
                { "en-IT",        "en-001"       },
                { "en-JP",        "en-001"       },
                { "en-KR",        "en-001"       },
                { "en-LT",        "en-001"       },
                { "en-LV",        "en-001"       },
                { "en-MM",        "en-001"       },
                { "en-MX",        "en-001"       },
                { "en-NO",        "en-001"       },
                { "en-PL",        "en-001"       },
                { "en-PT",        "en-001"       },
                { "en-RO",        "en-001"       },
                { "en-RU",        "en-001"       },
                { "en-SA",        "en-001"       },
                { "en-SK",        "en-001"       },
                { "en-TH",        "en-001"       },
                { "en-TR",        "en-001"       },
                { "en-TW",        "en-001"       },
                { "en-UA",        "en-001"       },
                // Spanish regional fallbacks for macOS 26 ICU-only cultures
                { "es-003",       "es-419"       },
                { "es-AG",        "es-419"       },
                { "es-BB",        "es-419"       },
                { "es-BM",        "es-419"       },
                { "es-BQ",        "es-419"       },
                { "es-BS",        "es-419"       },
                { "es-CA",        "es-419"       },
                { "es-CW",        "es-419"       },
                { "es-DM",        "es-419"       },
                { "es-GD",        "es-419"       },
                { "es-GY",        "es-419"       },
                { "es-HT",        "es-419"       },
                { "es-KN",        "es-419"       },
                { "es-KY",        "es-419"       },
                { "es-LC",        "es-419"       },
                { "es-TC",        "es-419"       },
                { "es-TT",        "es-419"       },
                { "es-VC",        "es-419"       },
                { "es-VG",        "es-419"       },
                { "es-VI",        "es-419"       },
                // Geographic/regional fallbacks for ICU-only cultures (no NLS equivalent - macOS 26)
                { "ain-JP",       "ja-JP"        },
                { "apw-US",       "en-US"        },
                { "bua-RU",       "ru-RU"        },
                { "cho-US",       "en-US"        },
                { "cic-US",       "en-US"        },
                { "gaa-GH",       "en-GH"        },
                { "gez-ER",       "ti-ER"        },
                { "gez-ET",       "am-ET"        },
                { "hmn-CN",       "zh-CN"        },
                { "ht-HT",        "fr-HT"        },
                { "io-001",       "en-001"       },
                { "jbo-001",      "en-001"       },
                { "kaj-NG",       "en-NG"        },
                { "kcg-NG",       "en-NG"        },
                { "kpe-GN",       "fr-GN"        },
                { "kpe-LR",       "en-LR"        },
                { "ku-Latn-SY",   "ar-SY"        },
                { "ku-Latn-TR",   "tr-TR"        },
                { "mic-CA",       "en-CA"        },
                { "mid-IQ",       "ar-IQ"        },
                { "mus-US",       "en-US"        },
                { "myv-RU",       "ru-RU"        },
                { "nnp-IN",       "hi-IN"        },
                { "nv-US",        "en-US"        },
                { "ny-MW",        "en-MW"        },
                { "osa-US",       "en-US"        },
                { "pms-IT",       "it-IT"        },
                { "pqm-CA",       "en-CA"        },
                { "pt-FR",        "pt-PT"        },
                { "rej-ID",       "id-ID"        },
                { "rej-Rjng-ID",  "id-ID"        },
                { "rhg-Rohg-BD",  "bn-BD"        },
                { "rhg-Rohg-MM",  "my-MM"        },
                { "scn-IT",       "it-IT"        },
                { "shn-MM",       "my-MM"        },
                { "shn-TH",       "th-TH"        },
                { "sm-AS",        "en-AS"        },
                { "sm-WS",        "en-WS"        },
                { "trv-TW",       "zh-TW"        },
                { "tyv-RU",       "ru-RU"        },
                { "wa-BE",        "fr-BE"        },
                // Geographic/regional fallbacks for ICU-only cultures (no NLS equivalent - Ubuntu 24.04)
                { "ar-EH",        "ar-MA"        }, // Arabic, Western Sahara → Arabic, Morocco (nearest NLS Arabic)
                { "blo-BJ",       "fr-BJ"        }, // Anii, Benin → French, Benin (official language)
                { "csw-CA",       "en-CA"        }, // Swampy Cree, Canada → English, Canada
                { "en-DG",        "en-001"       }, // English, Diego Garcia → English, World (British territory)
                { "es-EA",        "es-ES"        }, // Spanish, Ceuta & Melilla → Spanish, Spain
                { "es-IC",        "es-ES"        }, // Spanish, Canary Islands → Spanish, Spain
                { "ie-EE",        "et-EE"        }, // Interlingue, Estonia → Estonian, Estonia (no NLS for Interlingue)
                { "ko-CN",        "ko-KR"        }, // Korean, China → Korean, Korea
                { "ku-TR",        "tr-TR"        }, // Kurdish, Turkey → Turkish, Turkey (no NLS for ku-Latn-TR)
                { "kxv-Deva-IN",  "hi-IN"        }, // Kuvi, Devanagari → Hindi, India
                { "kxv-Latn-IN",  "en-IN"        }, // Kuvi, Latin → English, India
                { "kxv-Orya-IN",  "or-IN"        }, // Kuvi, Odia → Odia, India
                { "kxv-Telu-IN",  "te-IN"        }, // Kuvi, Telugu → Telugu, India
                { "lij-IT",       "it-IT"        }, // Ligurian, Italy → Italian, Italy (no NLS for Ligurian)
                { "lmo-IT",       "it-IT"        }, // Lombard, Italy → Italian, Italy (no NLS for Lombard)
                { "prg-PL",       "prg-001"      }, // Prussian, Poland → Prussian, World (only NLS Prussian)
                { "syr-IQ",       "syr-SY"       }, // Syriac, Iraq → Syriac, Syria (only NLS Syriac)
                { "szl-PL",       "pl-PL"        }, // Silesian, Poland → Polish, Poland (no NLS for Silesian)
                { "tok-001",      "en-001"       }, // Toki Pona, World → English, World (constructed language)
                { "vec-IT",       "it-IT"        }, // Venetian, Italy → Italian, Italy (no NLS for Venetian)
                { "vmw-MZ",       "pt-MZ"        }, // Makhuwa, Mozambique → Portuguese, Mozambique (official language)
                { "xnr-IN",       "hi-IN"        }, // Kangri, India → Hindi, India (no NLS for Kangri)
                { "yi-UA",        "yi-001"       }, // Yiddish, Ukraine → Yiddish, World (only NLS Yiddish)
                { "yue-Hans-CN",  "zh-CN"        }, // Cantonese, Simplified, China → Chinese Simplified, China
                { "yue-Hant-HK",  "zh-HK"        }, // Cantonese, Traditional, Hong Kong → Chinese Traditional, Hong Kong
                { "za-CN",        "zh-CN"        }, // Zhuang, China → Chinese Simplified, China (no NLS for Zhuang)
            };

            foreach (var (icuName, nlsName) in icuToNls)
            {
                // Prefer a native NLS surrogate for the ICU name if Windows supports it;
                // only fall back to copying the mapped NLS surrogate if it does not.
                if (TryWriteNlsSurrogate(icuName)) continue;

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
