namespace BuildingBlocks.Messaging.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime OccurredOnUtc { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = default!; 
        public string Content { get; set; } = default!;
        public DateTime? ProcessedOnUtc { get; set; } 
        public string? Error { get; set; }

    }
}
