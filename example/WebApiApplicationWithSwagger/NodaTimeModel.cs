using NodaTime;

namespace WebApiApplicationWithSwagger
{
    public class NodaTimeModel
    {
        public Instant Instant { get; set; }

        public LocalDate LocalDate { get; set; }

        public LocalTime LocalTime { get; set; }

        public LocalDateTime LocalDateTime { get; set; }

        public OffsetDateTime OffsetDateTime { get; set; }

        public ZonedDateTime ZonedDateTime { get; set; }

        public Interval Interval { get; set; }

        public Offset Offset { get; set; }

        public Period Period { get; set; }

        public Duration Duration { get; set; }

        public DateTimeZone DateTimeZone { get; set; }
    }
}