﻿using BenchmarkDotNet.Jobs;
using System;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Xunit;

namespace BenchmarkDotNet.IntegrationTests
{
    [Config(typeof(SingleRunFastConfig))]
    public class SetupAttributeTest
    {
        private const string SetupCalled = "// ### Setup called ###";
        private const string BenchmarkCalled = "// ### Benchmark called ###";

        [Fact]
        public void SetupMethodRunsTest()
        {
            var logger = new AccumulationLogger();
            var config = DefaultConfig.Instance.With(logger);
            BenchmarkTestExecutor.CanExecute<SetupAttributeTest>(config);

            string log = logger.GetLog();
            Assert.Contains(SetupCalled + Environment.NewLine, log);
            Assert.True(
                log.IndexOf(SetupCalled + Environment.NewLine) <
                log.IndexOf(BenchmarkCalled + Environment.NewLine));
        }

        [Setup]
        public void Setup()
        {
            Console.WriteLine(SetupCalled);
        }

        [Benchmark]
        public void Benchmark()
        {
            Console.WriteLine(BenchmarkCalled);
            Thread.Sleep(5);
        }
    }
}
