using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;


namespace AssetTrackerWebAPI.Services
{
    public class DBinitService: BackgroundService
    {
        private readonly IAmazonDynamoDB _dynamoDBClient;

        public DBinitService(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
        }
        async Task CreatePlaidItemsTable()
        {
        var tableName = "PlaidItems";
        var request = new CreateTableRequest
        {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition { AttributeName = "userId", AttributeType = "S" },
            },
            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement { AttributeName = "userId", KeyType = "HASH" }
            },
            BillingMode = BillingMode.PAY_PER_REQUEST
        };
        try
        {
            await _dynamoDBClient.CreateTableAsync(request);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Table '{tableName}' created successfully.");
            Console.ResetColor();
        }
        catch (ResourceInUseException)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Table '{tableName}' already exists.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error creating table: {ex.Message}");
            Console.ResetColor();
        }
        }
        async Task CreateUsersTable()
        {
        var tableName = "Users";
        var request = new CreateTableRequest
        {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition { AttributeName = "userId", AttributeType = "S" },
                new AttributeDefinition { AttributeName = "username", AttributeType = "S" } // ← add this
            },
            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement { AttributeName = "userId", KeyType = "HASH" }
            },
            GlobalSecondaryIndexes = new List<GlobalSecondaryIndex> // ← add this
            {
                new GlobalSecondaryIndex
                {
                    IndexName = "username-index",
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement { AttributeName = "username", KeyType = "HASH" }
                    },
                    Projection = new Projection
                    {
                        ProjectionType = ProjectionType.ALL // ← return all attributes
                    }
                }
            },
            BillingMode = BillingMode.PAY_PER_REQUEST
        };

        try
        {
            await _dynamoDBClient.CreateTableAsync(request);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Table '{tableName}' created successfully.");
            Console.ResetColor();
        }
        catch (ResourceInUseException)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Table '{tableName}' already exists.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error creating table: {ex.Message}");
            Console.ResetColor();
        }
    }

        async Task CreateProfilesTable()
        {
        var tableName = "Profiles";
        var request = new CreateTableRequest
            {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
                {
                new AttributeDefinition { AttributeName = "profileId", AttributeType = "S" }
                },
            KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement { AttributeName = "profileId", KeyType = "HASH" }
                },
            BillingMode = BillingMode.PAY_PER_REQUEST
            };
            try
            {
                await _dynamoDBClient.CreateTableAsync(request);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Table '{tableName}' created successfully.");
                Console.ResetColor(); 
            }
            catch (ResourceInUseException)
            {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Table '{tableName}' already exists.");
                    Console.ResetColor();
            }
            catch (Exception ex)
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error creating table: {ex.Message}");
                    Console.ResetColor();
            }
        }

        async Task CreateAccountsTable()
        {
        var tableName = "Accounts";
        var request = new CreateTableRequest
            {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
                {
                new AttributeDefinition { AttributeName = "accountId", AttributeType = "S" },
                new AttributeDefinition { AttributeName = "profileId", AttributeType = "S" }
                },
            KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement { AttributeName = "profileId", KeyType = "HASH" },
                new KeySchemaElement { AttributeName = "accountId", KeyType = "RANGE" }
                },
            BillingMode = BillingMode.PAY_PER_REQUEST
            };
            try
            {
                await _dynamoDBClient.CreateTableAsync(request);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Table 'Accounts' created successfully.");
                Console.ResetColor(); 
            }
            catch (ResourceInUseException)
            {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Table '{tableName}' already exists.");
                    Console.ResetColor();
            }
            catch (Exception ex)
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error creating table: {ex.Message}");
                    Console.ResetColor();
            }
        }

        async Task CreateTransactionsTable()
        {
        var tableName = "Transactions";
        var request = new CreateTableRequest
            {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
                {
                new AttributeDefinition { AttributeName = "accountId", AttributeType = "S" },   
                new AttributeDefinition { AttributeName = "transactionId", AttributeType = "S" }
                },
            KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement { AttributeName = "accountId", KeyType = "HASH" },    
                new KeySchemaElement { AttributeName = "transactionId", KeyType = "RANGE" }
                },
            BillingMode = BillingMode.PAY_PER_REQUEST
            };
            try
            {
                await _dynamoDBClient.CreateTableAsync(request);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Table '{tableName}' created successfully.");
                Console.ResetColor(); 
            }
            catch (ResourceInUseException)
            {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Table '{tableName}' already exists.");
                    Console.ResetColor();
            }
            catch (Exception ex)
            {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error creating table: {ex.Message}");
                    Console.ResetColor();
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
            await Task.WhenAll(
            CreateAccountsTable(),
            CreateProfilesTable(),
            CreateTransactionsTable(),
            CreateUsersTable(),
            CreatePlaidItemsTable()
                );
            }
    }

}