namespace Swashbuckle.NodaTime.AspNetCore

open System
open Newtonsoft.Json
open NodaTime
open Swashbuckle.AspNetCore.Swagger

module Schemas = 
    type Container = 
        {Instant : Schema;
         LocalDate : Schema;
         LocalTime : Schema;
         LocalDateTime : Schema;
         OffsetDateTime : Schema;
         ZonedDateTime : Schema;
         Interval : Schema;
         Offset : Schema;
         Period : Schema;
         Duration : Schema;
         DateTimeZone : Schema}
    
    let Create(serializerSettings : JsonSerializerSettings) : Container = 
        let stringRepresentation value = 
            // this produces value including quotes, for example: "13:45:13.784"
            // deserializing into string will remove quotes
            JsonConvert.DeserializeObject<string>
                (JsonConvert.SerializeObject(value, serializerSettings))
        let stringSchema value = 
            Schema(Type = "string", Example = stringRepresentation value)
        let timeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault()
        let instant = Instant.FromDateTimeUtc DateTime.UtcNow
        let zonedDateTime = instant.InZone timeZone
        let interval = 
            Interval
                (instant, 
                 instant.PlusTicks(TimeSpan.TicksPerDay)
                        .PlusTicks(TimeSpan.TicksPerHour)
                        .PlusTicks(TimeSpan.TicksPerMinute)
                        .PlusTicks(TimeSpan.TicksPerSecond)
                        .PlusTicks(TimeSpan.TicksPerMillisecond))
        {Container.Instant = stringSchema instant;
         Container.LocalDate = stringSchema zonedDateTime.Date;
         Container.LocalTime = stringSchema zonedDateTime.TimeOfDay;
         Container.LocalDateTime = stringSchema zonedDateTime.LocalDateTime;
         Container.OffsetDateTime = 
             stringSchema(instant.WithOffset zonedDateTime.Offset);
         Container.ZonedDateTime = stringSchema zonedDateTime;
         Container.Interval = 
             Schema(Type = "object", 
                    Properties = dict [("Start", stringSchema interval.Start);
                                       ("End", stringSchema interval.End)]);
         Container.Offset = stringSchema zonedDateTime.Offset;
         Container.Period = 
             stringSchema
                 (Period.Between
                      (zonedDateTime.LocalDateTime, 
                       interval.End.InZone(timeZone).LocalDateTime, 
                       PeriodUnits.AllUnits));
         Container.Duration = stringSchema interval.Duration;
         Container.DateTimeZone = stringSchema timeZone}