| Method          | InvocationCount | UnrollFactor | RequestCount | Mean         | Error     | StdDev    | Median        | Allocated |
|---------------- |---------------- |------------- |------------- |-------------:|----------:|----------:|--------------:|----------:|
| AddRequests     | Default         | 16           | 10000        |     21.86 us |  0.110 us |  0.098 us |     21.812 us |         - |
| SumOfLastMinute | 1               | 1            | 10000        |     32.32 us |  0.633 us |  0.967 us |     32.541 us |     144 B |
| SumOfLastHour   | 1               | 1            | 10000        |     30.46 us |  0.595 us |  1.588 us |     29.958 us |     144 B |
| AddRequests     | Default         | 16           | 100000       |    211.71 us |  2.152 us |  1.907 us |    211.883 us |         - |
| SumOfLastMinute | 1               | 1            | 100000       |     29.94 us |  0.592 us |  1.529 us |     29.396 us |     144 B |
| SumOfLastHour   | 1               | 1            | 100000       |     39.46 us |  2.358 us |  6.690 us |     38.375 us |     144 B |
| AddRequests     | Default         | 16           | 1000000      |  2,127.92 us | 14.512 us | 12.865 us |  2,126.506 us |         - |
| SumOfLastMinute | 1               | 1            | 1000000      |     32.55 us |  1.535 us |  4.354 us |     31.958 us |     144 B |
| SumOfLastHour   | 1               | 1            | 1000000      |     32.06 us |  1.430 us |  4.034 us |     31.729 us |     144 B |
| AddRequests     | Default         | 16           | 10000000     | 21,056.76 us | 72.028 us | 60.146 us | 21,070.882 us |         - |
| SumOfLastMinute | 1               | 1            | 10000000     |     22.36 us |  7.852 us | 22.905 us |      8.063 us |     144 B |
| SumOfLastHour   | 1               | 1            | 10000000     |     22.62 us |  7.795 us | 22.740 us |      8.479 us |     144 B |



| Method               | InvocationCount | UnrollFactor | RequestCount | Mean         | Error     | StdDev    | Median        | Allocated |
|--------------------- |---------------- |------------- |------------- |-------------:|----------:|----------:|--------------:|----------:|
| AddRequests_Faster   | Default         | 16           | 10000        |     21.07 us |  0.033 us |  0.030 us |     21.069 us |         - |
| SumOfLastMinute_Linq | 1               | 1            | 10000        |     28.63 us |  0.735 us |  2.049 us |     28.104 us |     144 B |
| SumOfLastHour_Linq   | 1               | 1            | 10000        |     28.48 us |  0.540 us |  1.487 us |     28.145 us |     144 B |
| AddRequests_Faster   | Default         | 16           | 100000       |    212.06 us |  0.411 us |  0.384 us |    211.913 us |         - |
| SumOfLastMinute_Linq | 1               | 1            | 100000       |     28.04 us |  0.602 us |  1.728 us |     27.709 us |     144 B |
| SumOfLastHour_Linq   | 1               | 1            | 100000       |     28.37 us |  0.564 us |  1.516 us |     28.124 us |     144 B |
| AddRequests_Faster   | Default         | 16           | 1000000      |  2,109.20 us |  8.243 us |  7.711 us |  2,105.743 us |         - |
| SumOfLastMinute_Linq | 1               | 1            | 1000000      |     30.40 us |  1.378 us |  3.955 us |     29.667 us |     144 B |
| SumOfLastHour_Linq   | 1               | 1            | 1000000      |     30.78 us |  1.586 us |  4.526 us |     30.084 us |     144 B |
| AddRequests_Faster   | Default         | 16           | 10000000     | 21,030.84 us | 35.340 us | 27.591 us | 21,029.289 us |         - |
| SumOfLastMinute_Linq | 1               | 1            | 10000000     |     22.99 us |  7.914 us | 23.087 us |      8.000 us |     144 B |
| SumOfLastHour_Linq   | 1               | 1            | 10000000     |     22.85 us |  8.353 us | 24.365 us |      7.937 us |     144 B |


| Method                    | InvocationCount | UnrollFactor | RequestCount | Mean          | Error      | StdDev     | Median        | Allocated |
|-------------------------- |---------------- |------------- |------------- |--------------:|-----------:|-----------:|--------------:|----------:|
| AddRequests_Fastest       | Default         | 16           | 10000        |     21.039 us |  0.0980 us |  0.0818 us |     21.011 us |         - |
| SumOfLastMinute_Span      | 1               | 1            | 10000        |     10.420 us |  0.5555 us |  1.5578 us |     10.166 us |         - |
| SumOfLastHour_Span        | 1               | 1            | 10000        |     10.136 us |  0.4584 us |  1.2627 us |      9.729 us |         - |
| SumOfLastHour_Span_Faster | 1               | 1            | 10000        |     10.201 us |  0.4666 us |  1.2930 us |      9.916 us |         - |
| AddRequests_Fastest       | Default         | 16           | 100000       |    209.472 us |  0.8601 us |  0.8046 us |    209.261 us |         - |
| SumOfLastMinute_Span      | 1               | 1            | 100000       |     10.103 us |  0.5729 us |  1.6065 us |      9.833 us |         - |
| SumOfLastHour_Span        | 1               | 1            | 100000       |     10.115 us |  0.5998 us |  1.7209 us |      9.666 us |         - |
| SumOfLastHour_Span_Faster | 1               | 1            | 100000       |      9.832 us |  0.4675 us |  1.3033 us |      9.521 us |         - |
| AddRequests_Fastest       | Default         | 16           | 1000000      |  2,101.942 us |  7.7843 us |  6.5003 us |  2,099.051 us |         - |
| SumOfLastMinute_Span      | 1               | 1            | 1000000      |     13.354 us |  1.2091 us |  3.4691 us |     11.875 us |         - |
| SumOfLastHour_Span        | 1               | 1            | 1000000      |     12.650 us |  0.9829 us |  2.8200 us |     12.000 us |         - |
| SumOfLastHour_Span_Faster | 1               | 1            | 1000000      |     11.613 us |  0.8468 us |  2.3885 us |     10.874 us |         - |
| AddRequests_Fastest       | Default         | 16           | 10000000     | 21,112.912 us | 80.5272 us | 67.2439 us | 21,093.534 us |         - |
| SumOfLastMinute_Span      | 1               | 1            | 10000000     |     15.580 us |  3.6361 us | 10.6067 us |     17.708 us |         - |
| SumOfLastHour_Span        | 1               | 1            | 10000000     |     14.813 us |  3.5715 us | 10.4181 us |     17.771 us |         - |
| SumOfLastHour_Span_Faster | 1               | 1            | 10000000     |     14.961 us |  3.2043 us |  9.3472 us |     17.500 us |         - |
