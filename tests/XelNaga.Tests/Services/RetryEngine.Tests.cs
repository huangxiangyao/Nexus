﻿using Aiursoft.Scanner;
using Aiursoft.XelNaga.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Aiursoft.XelNaga.Tests.Services
{
    /// <summary>
    /// Retry engine tests.
    /// </summary>
    [TestClass]
    public class RetryEngineTests
    {
        private RetryEngine retryEngine;

        /// <summary>
        /// Test initialize.
        /// </summary>
        /// <returns>TestInitialize task.</returns>
        [TestInitialize]
        public void TestInitialize()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddLibraryDependencies()
                .BuildServiceProvider();

            this.retryEngine = serviceProvider.GetRequiredService<RetryEngine>();
        }

        /// <summary>
        /// Retry with success task.
        /// </summary>
        /// <returns>Task</returns>
        [TestMethod]
        public async Task RetrySuccess()
        {
            var result = await this.retryEngine.RunWithTry(
            attempts =>
            {
                if (attempts == 1)
                {
                    throw new InvalidOperationException("Fake Exception.");
                }

                return Task.FromResult(12343 + attempts);
            }, attempts: 2, when: e => e is InvalidOperationException);
            Assert.AreEqual(12345, result);
        }

        /// <summary>
        /// Retry with a failure task.
        /// </summary>
        /// <returns>Task</returns>
        [TestMethod]
        public async Task RetryFailure()
        {
            try
            {
                var result = await this.retryEngine.RunWithTry(
                attempts =>
                {
                    if (attempts == 1)
                    {
                        throw new InvalidOperationException("Fake Exception.");
                    }

                    if (attempts == 2)
                    {
                        throw new NotImplementedException("Fake Exception.");
                    }

                    return Task.FromResult(12345 + attempts);
                }, attempts: 2, when: e => e is InvalidOperationException);

                Assert.AreEqual(result, 12345);
                Assert.Fail("Shouldn't suppress NotImplementedException.");
            }
            catch (NotImplementedException)
            {
            }
        }
    }
}
