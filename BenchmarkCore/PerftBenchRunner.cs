﻿namespace BenchmarkCore
{
    using BenchmarkDotNet.Running;

    /*

// * Summary *

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.379 (1809/October2018Update/Redstone5)
Intel Core i7-8086K CPU 4.00GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=2.2.101
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

Method	N	Mean	Error	StdDev
Result	1	296.4 us	1.496 us	1.326 us
Result	2	300.2 us	1.678 us	1.570 us
Result	3	379.0 us	1.897 us	1.584 us
Result	4	2,408.1 us	12.816 us	11.988 us
Result	5	64,921.2 us	1,310.703 us	1,402.437 us
Result	6	1,912,300.6 us	3,551.167 us	3,148.017 us

 Method |  Job | Runtime | N |           Mean |         Error |        StdDev | Ratio | RatioSD |
 Result | Core |    Core | 1 |       285.1 us |      1.088 us |      1.017 us |     ? |       ? |
 Result | Core |    Core | 2 |       292.6 us |      2.725 us |      2.549 us |     ? |       ? |
 Result | Core |    Core | 3 |       329.5 us |      1.161 us |      1.029 us |     ? |       ? |
 Result | Core |    Core | 4 |     1,762.3 us |     10.213 us |      9.053 us |     ? |       ? |
 Result | Core |    Core | 5 |    50,636.9 us |    998.394 us |  1,262.649 us |     ? |       ? |
 Result | Core |    Core | 6 | 1,537,741.3 us | 11,692.917 us | 10,365.466 us |     ? |       ? |

 Method | N |            Mean |         Error |        StdDev |
------- |-- |----------------:|--------------:|--------------:|
 Result | 1 |        98.25 us |     0.7352 us |     0.6518 us |
 Result | 2 |       103.12 us |     0.3322 us |     0.2774 us |
 Result | 3 |       160.12 us |     0.7573 us |     0.6713 us |
 Result | 4 |     1,588.79 us |     3.5257 us |     3.2979 us |
 Result | 5 |    51,304.89 us | 1,006.0687 us | 1,235.5427 us |
 Result | 6 | 1,547,075.27 us | 2,673.5871 us | 2,500.8751 us |

 Method | N |         Mean |     Error |    StdDev |
------- |-- |-------------:|----------:|----------:|
 Result | 4 |     1.569 ms | 0.0032 ms | 0.0030 ms |
 Result | 5 |    49.873 ms | 0.6022 ms | 0.5029 ms |
 Result | 6 | 1,525.450 ms | 2.8101 ms | 2.3466 ms |

 Method | N |         Mean |     Error |    StdDev |
------- |-- |-------------:|----------:|----------:|
 Result | 4 |     1.411 ms | 0.0045 ms | 0.0042 ms |
 Result | 5 |    48.951 ms | 0.8226 ms | 0.7695 ms |
 Result | 6 | 1,486.312 ms | 1.7112 ms | 1.6006 ms |
*/

    public class PerftBenchRunner
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PerftBench>();
        }
    }
}