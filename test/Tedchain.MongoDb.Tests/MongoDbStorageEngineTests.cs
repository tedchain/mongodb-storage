// Copyright (c) 2010-2018 The Bitcoin developers
// Original code was distributed under the MIT software license.
// Copyright (c) 2014-2018 TEDLab Sciences Ltd
// Tedchain code distributed under the GPLv3 license, see COPYING file.

using Tedchain.Tests;
using System;
using MongoDB.Driver;
using Xunit.Abstractions;

namespace Tedchain.MongoDb.Tests
{
    public class MongoDbStorageEngineTests : BaseStorageEngineTests
    {
        ITestOutputHelper Output { get; }

        public MongoDbStorageEngineTests(ITestOutputHelper output)
        {
            Output = output;
            var logger = new Logger() { Output = output };
            //var logger = new Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider((x,y)=>true, true).CreateLogger("Test");
            var store = new MongoDbStorageEngine(
                            new MongoDbStorageEngineConfiguration {
                                ConnectionString="mongodb://localhost",
                                Database="tedchaintest",
                                ReadLoopDelay=TimeSpan.FromMilliseconds(50),
                                ReadRetryCount=10,
                                StaleTransactionDelay=TimeSpan.FromMinutes(10),
                                RunRollbackThread=false                              
                            }, logger);
            store.RecordCollection.DeleteMany(x => true);
            store.TransactionCollection.DeleteMany(x => true);
            store.PendingTransactionCollection.DeleteMany(x => true);

            this.Store = store;
        }
    }
}
