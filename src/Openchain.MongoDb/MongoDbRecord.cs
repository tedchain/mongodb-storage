// Copyright (c) 2010-2018 The Bitcoin developers
// Original code was distributed under the MIT software license.
// Copyright (c) 2014-2018 TEDLab Sciences Ltd
// Tedchain code distributed under the GPLv3 license, see COPYING file.

using Tedchain.Infrastructure;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tedchain.MongoDb
{
    public class MongoDbRecord
    {
        [BsonId]
        public byte[] Key
        {
            get;
            set;
        }

        public string KeyS
        {
            get;
            set;
        }

        public byte[] Value
        {
            get;
            set;
        }

        public byte[] Version
        {
            get;
            set;
        }

        public string[] Path
        {
            get;
            set;
        }

        public RecordType Type
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public byte[] TransactionLock { get; set; }

        [BsonExtraElements]
        public BsonDocument Extra { get; set; }

    }
}