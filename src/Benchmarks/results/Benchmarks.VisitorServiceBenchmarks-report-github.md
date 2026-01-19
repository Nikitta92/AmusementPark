```

BenchmarkDotNet v0.13.12, macOS 15.7.3 (24G419) [Darwin 24.6.0]
Apple M4, 1 CPU, 10 logical and 10 physical cores
.NET SDK 9.0.305
  [Host]   : .NET 9.0.9 (9.0.925.41916), Arm64 RyuJIT AdvSIMD
  .NET 9.0 : .NET 9.0.9 (9.0.925.41916), Arm64 RyuJIT AdvSIMD

Job=.NET 9.0  Runtime=.NET 9.0  

```
| Method                      | Mean     | Error    | StdDev   | Median   | Min      | Max      | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
|---------------------------- |---------:|---------:|---------:|---------:|---------:|---------:|------:|--------:|--------:|-------:|----------:|------------:|
| TryGetAsync_Dapper          | 137.8 μs |  2.72 μs |  5.43 μs | 135.5 μs | 131.1 μs | 154.1 μs |  1.00 |    0.00 |  0.2441 |      - |   3.69 KB |        1.00 |
| TryGetAsync_EFCore          | 175.3 μs |  2.11 μs |  1.65 μs | 175.3 μs | 173.1 μs | 177.9 μs |  1.25 |    0.06 |  6.3477 |      - |  52.42 KB |       14.22 |
| CreateAsync_Dapper          | 295.1 μs |  5.66 μs |  5.56 μs | 296.4 μs | 279.9 μs | 300.3 μs |  2.10 |    0.11 |  0.4883 |      - |   5.84 KB |        1.58 |
| CreateAsync_EFCore          | 536.7 μs | 10.66 μs | 13.48 μs | 539.8 μs | 499.0 μs | 562.7 μs |  3.80 |    0.18 |  6.8359 |      - |  63.55 KB |       17.23 |
| UpdateAsync_Dapper          | 299.4 μs |  2.82 μs |  2.35 μs | 299.2 μs | 293.8 μs | 302.5 μs |  2.14 |    0.10 |  0.4883 |      - |   5.91 KB |        1.60 |
| UpdateAsync_EFCore          | 547.6 μs |  9.55 μs |  8.46 μs | 546.9 μs | 531.2 μs | 566.0 μs |  3.91 |    0.19 |  6.8359 |      - |  63.46 KB |       17.21 |
| CreateAndDeleteAsync_Dapper | 602.5 μs |  5.02 μs |  3.92 μs | 601.9 μs | 598.2 μs | 613.3 μs |  4.31 |    0.21 |  0.9766 |      - |    8.8 KB |        2.39 |
| CreateAndDeleteAsync_EFCore | 914.0 μs |  5.96 μs |  4.98 μs | 912.7 μs | 905.3 μs | 921.5 μs |  6.53 |    0.30 | 13.6719 |      - | 115.78 KB |       31.40 |
| GetAll_Dapper               | 227.9 μs |  3.67 μs |  5.60 μs | 225.8 μs | 223.7 μs | 245.4 μs |  1.63 |    0.07 |  3.9063 |      - |  33.02 KB |        8.95 |
| GetAll_EFCore               | 347.5 μs |  5.65 μs |  5.01 μs | 345.5 μs | 342.2 μs | 357.5 μs |  2.48 |    0.09 | 11.7188 | 0.9766 |  95.99 KB |       26.03 |
| GetAllSinceId_Dapper        | 192.7 μs |  3.84 μs | 10.24 μs | 193.1 μs | 171.0 μs | 215.7 μs |  1.38 |    0.11 |  2.1973 |      - |  18.33 KB |        4.97 |
| GetAllSinceId_EFCore        | 278.4 μs |  5.44 μs |  9.81 μs | 276.7 μs | 260.6 μs | 306.0 μs |  2.01 |    0.12 |  8.7891 |      - |  76.85 KB |       20.84 |
