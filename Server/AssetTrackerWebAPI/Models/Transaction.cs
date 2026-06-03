using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Transactions")]
public class Transaction
{
    // --- Keys ---
    [DynamoDBHashKey]
    public required string accountId { get; set; }

    [DynamoDBRangeKey]
    public required string transactionId { get; set; }

    // --- Core ---
    public double? amount { get; set; }
    public string? name { get; set; }                   // Raw bank description, fallback when no merchantName
    public string? ISOCurrencyCode { get; set; }        // "USD"
    public string? merchantName { get; set; }           // Enriched: "Walmart"
    public string? merchantEntityId { get; set; }       // Stable ID — useful for grouping spend by merchant
    public string? logoUrl { get; set; }                

    // --- Dates ---
    public DateTime? date { get; set; }                 // Posted date
    public DateTime? authorizedDate { get; set; }       // When card was swiped

    // --- Status ---
    public bool? pending { get; set; }
    public string? pendingTransactionId { get; set; }   // Links to pending version before settlement

    // --- Category ---
    public string? categoryPrimary { get; set; }        // "FOOD_AND_DRINK"
    public string? categoryDetailed { get; set; }       // "FOOD_AND_DRINK_FAST_FOOD"
    public string? categoryConfidence { get; set; }     // "VERY_HIGH" | "HIGH" | "MEDIUM" | "LOW"
    public string? categoryIconUrl { get; set; }        // Fallback when no merchant logo

    // --- Payment ---
    public string? paymentChannel { get; set; }         // "in store" | "online" | "other"

}