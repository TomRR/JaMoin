// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Benchmarks;

// BenchmarkRunner.Run<TimeboxedCounterBenchmarks>();
// BenchmarkRunner.Run<TimeboxedCounterBenchmarksLinq>();
BenchmarkRunner.Run<TimeboxedCounterBenchmarksSpan>();