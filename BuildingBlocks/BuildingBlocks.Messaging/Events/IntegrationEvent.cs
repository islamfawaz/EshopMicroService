namespace BuildingBlocks.Messaging.Events
{
    public record IntegrationEvent
    {
        public Guid Id { get; set; }
        public DateTime OccurredOn => DateTime.UtcNow;

        public string EventType => GetType().AssemblyQualifiedName!;
    }
}
