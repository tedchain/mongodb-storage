# MongoDB storage provider for Tedchain Server

This project implements a storage provider for Tedchain Server using MongoDb as a persistence media.

It implements Records & Transactions storage and Anchors storage.

## Installation

* Edit the Tedchain webapp project.json
    * Remove dnxcore5 in the frameworks section (mongodb driver currently lacks support for dnxcore) 
    * Add MongoDB storage provider as a dependency

    ```json
    {
        "version": "0.5.0-rc1",
        "entryPoint": "Tedchain.Server",

        "dependencies": {
            "Tedchain.Server": "0.5.0-rc1-*",
            "Tedchain.Validation.PermissionBased": "0.5.0-rc1-*",
            "Tedchain.Anchoring.Blockchain": "0.5.0-rc1-*",
            "Tedchain.MongoDb": "0.1.0-alpha1"
        },

        "userSecretsId": "Tedchain.Server",

        "commands": {
            "start": "Microsoft.AspNet.Hosting --webroot \"Webroot\" --server Microsoft.AspNet.Server.Kestrel --server.urls http://localhost:8080"
        },

        "frameworks": {
            "dnx451": {
            }
        }
    }
    ```

* Edit the Tedchain webapp config.json
    * Edit the root storage section specifying the following parameters :
        * _provider_ : **MongoDb** 
        * _connection_string_ : MongoDB connection string to your MongoDb instance
        * _database_ : Name of the MongoDb database to use

    ```json
    "storage": {
        "provider": "MongoDb",
        "connection_string": "mongodb://localhost",
        "database": "tedchain"
    },
    ```

    * Edit the anchoring storage section :
        * _provider_ : **MongoDb** 
        * _connection_string_ : MongoDB connection string to your MongoDb instance
        * _database_ : Name of the MongoDb database to use

    ```json
    "anchoring": {
        //...
        "storage": {
            "provider": "MongoDb",
            "connection_string": "mongodb://localhost",
            "database": "tedchain"
        }
    }
    ```
    
## Running

At first startup, needed collections and indexes are created.

3 collections are used :
* _records_ containing records details
* _transactions_ containing transactions details
* _pending_transactions_ containing transactions still being committed to the database (this collection is always nearly empty).

