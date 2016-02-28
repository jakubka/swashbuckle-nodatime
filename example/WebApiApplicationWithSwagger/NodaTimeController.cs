using System.Web.Http;
using NodaTime;

namespace WebApiApplicationWithSwagger
{
    public class NodaTimeController : ApiController
    {
        [Route("")]
        public NodaTimeModel Get()
        {
            var instant = SystemClock.Instance.Now;
            var dateTimeZone = DateTimeZoneProviders.Tzdb["Australia/Sydney"];
            var zonedDateTime = instant.InZone(dateTimeZone);
            var duration = Duration.FromMilliseconds(123456);

            return new NodaTimeModel
            {
                Instant = instant,
                LocalDateTime = zonedDateTime.LocalDateTime,
                LocalDate = zonedDateTime.LocalDateTime.Date,
                LocalTime = zonedDateTime.LocalDateTime.TimeOfDay,
                OffsetDateTime = zonedDateTime.ToOffsetDateTime(),
                ZonedDateTime = zonedDateTime,
                Interval = new Interval(instant, instant.Plus(duration)),
                Offset = dateTimeZone.MaxOffset,
                Period = Period.FromWeeks(3),
                Duration = duration,
                DateTimeZone = dateTimeZone,
            };
        }
    }
}