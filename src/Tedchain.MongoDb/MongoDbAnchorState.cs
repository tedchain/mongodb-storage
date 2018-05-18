// Copyright (c) 2010-2018 The Bitcoin developers
// Original code was distributed under the MIT software license.
// Copyright (c) 2014-2018 TEDLab Sciences Ltd
// Tedchain code distributed under the GPLv3 license, see COPYING file.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tedchain.Infrastructure;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tedchain.MongoDb
{
    public class MongoDbAnchorStateRecord
    {
        [BsonId]
        public byte[] Position { get; set; }
        public byte[] FullLedgerHash { get; set; }
        public long TransactionCount { get; set; }
        public BsonTimestamp Timestamp { get; set; } = new BsonTimestamp(0);

        [BsonExtraElements]
        public BsonDocument Extra { get; set; }
    }

    /// <summary>
    /// Persists information about the latest known anchor.
    /// </summary>
    public class MongoDbAnchorState : MongoDbBase, IAnchorState
    {
        internal IMongoCollection<MongoDbAnchorStateRecord> AnchorStateCollection
        {
            get;
            set;
        }

        public MongoDbAnchorState(string connectionString, string database)
            : base(connectionString,database)
        {
            AnchorStateCollection = Database.GetCollection<MongoDbAnchorStateRecord>("anchorstates");
        }

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Initialize()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

        /// <summary>
        /// Gets the last known anchor.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<LedgerAnchor> GetLastAnchor()
        {
            var res = await AnchorStateCollection.Find(x => true).SortByDescending(x => x.Timestamp).FirstOrDefaultAsync();
            return res == null ? null : new LedgerAnchor(new ByteString(res.Position),new ByteString(res.FullLedgerHash), res.TransactionCount);
        }

        /// <summary>
        /// Marks the anchor as successfully recorded in the anchoring medium.
        /// </summary>
        /// <param name="anchor">The anchor to commit.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task CommitAnchor(LedgerAnchor anchor)
        {
            await AnchorStateCollection.InsertOneAsync(new MongoDbAnchorStateRecord
            {
                Position = anchor.Position.ToByteArray(),
                FullLedgerHash = anchor.FullStoreHash.ToByteArray(),
                TransactionCount = anchor.TransactionCount
            });
        }
    }
}
