// Copyright (c) 2010-2018 The Bitcoin developers
// Original code was distributed under the MIT software license.
// Copyright (c) 2014-2018 TEDLab Sciences Ltd
// Tedchain code distributed under the GPLv3 license, see COPYING file.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Tedchain.Infrastructure;
using MongoDB.Driver;

namespace Tedchain.MongoDb
{
    public class MongoDbAnchorStateBuilder : IComponentBuilder<MongoDbAnchorState>
    {
        public string Name { get; } = "MongoDb";

        string connectionString
        {
            get;
            set;
        }

        string database
        {
            get;
            set;
        }


        public MongoDbAnchorState Build(IServiceProvider serviceProvider)
        {
            return new MongoDbAnchorState(connectionString,database);
        }

        public async Task Initialize(IServiceProvider serviceProvider, IConfigurationSection configuration)
        {
            connectionString = configuration["connection_string"];
            database = configuration["database"] ?? "tedchain";
            using (var m = new MongoDbAnchorState(connectionString, database))
            {
                await m.AnchorStateCollection.Indexes.DropAllAsync();
                await m.AnchorStateCollection.Indexes.CreateOneAsync(Builders<MongoDbAnchorStateRecord>.IndexKeys.Ascending(x => x.Timestamp), new CreateIndexOptions { Background = true, Unique = true });
            }
        }

    }
}
