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
        let stringSchema(value, format : string option) = 
            Schema(Type = "string", Example = stringRepresentation value, 
                   Format = match format with
                            | Some x -> x
                            | None -> null)
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
        {Container.Instant = stringSchema(instant, Some "date-time");
         Container.LocalDate = 
             stringSchema(zonedDateTime.Date, Some "full-date");
         Container.LocalTime = 
             stringSchema(zonedDateTime.TimeOfDay, Some "partial-time");
         Container.LocalDateTime = 
             stringSchema(zonedDateTime.LocalDateTime, None);
         Container.OffsetDateTime = 
             stringSchema
                 (instant.WithOffset zonedDateTime.Offset, Some "date-time");
         Container.ZonedDateTime = stringSchema(zonedDateTime, None);
         Container.Interval = 
             Schema
                 (Type = "object", 
                  Properties = dict 
                                   [("Start", 
                                     stringSchema
                                         (interval.Start, Some "date-time"));
                                    ("End", 
                                     stringSchema
                                         (interval.End, Some "date-time"))]);
         Container.Offset = 
             stringSchema(zonedDateTime.Offset, Some "time-numoffset");
         Container.Period = 
             stringSchema
                 (Period.Between
                      (zonedDateTime.LocalDateTime, 
                       interval.End.InZone(timeZone).LocalDateTime, 
                       PeriodUnits.AllUnits), None);
         Container.Duration = stringSchema(interval.Duration, None);
         Container.DateTimeZone = stringSchema(timeZone, None)}