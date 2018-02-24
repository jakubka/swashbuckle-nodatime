using System;
using NodaTime;

namespace Swashbuckle.NodaTime.AspNetCore.Web.Models
{
	public class NodaValue
	{
		public NodaValue()
		{
			DateTimeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
			Instant = Instant.FromDateTimeUtc(DateTime.UtcNow);
			ZonedDateTime = Instant.InZone(DateTimeZone);
			OffsetDateTime = Instant.WithOffset(ZonedDateTime.Offset);
			Interval = new Interval(Instant,
				Instant.PlusTicks(TimeSpan.TicksPerDay).PlusTicks(TimeSpan.TicksPerHour).PlusTicks(TimeSpan.TicksPerMinute)
					.PlusTicks(TimeSpan.TicksPerSecond).PlusTicks(TimeSpan.TicksPerMillisecond));
			Period = Period.Between(LocalDateTime, Interval.End.InZone(DateTimeZone).LocalDateTime, PeriodUnits.AllUnits);
		}

		public DateTimeZone DateTimeZone { get; }
		public Instant Instant { get; }
		public Interval Interval { get; }
		public Period Period { get; }
		public ZonedDateTime ZonedDateTime { get; }
		public OffsetDateTime OffsetDateTime { get; }
		public LocalDate LocalDate => ZonedDateTime.Date;
		public LocalTime LocalTime => ZonedDateTime.TimeOfDay;
		public LocalDateTime LocalDateTime => ZonedDateTime.LocalDateTime;
		public Offset Offset => ZonedDateTime.Offset;
		public Duration Duraton => Interval.Duration;
	}
}
