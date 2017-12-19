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
            let jsonString = 
                JsonConvert.SerializeObject(value, serializerSettings)
            // deserializing into string will remove quotes
            JsonConvert.DeserializeObject<string>(jsonString)
        
        let stringSchema value = 
            Schema(Type = "string", Example = stringRepresentation value)
        let instant = Instant.FromDateTimeOffset DateTimeOffset.Now
        let duration = Duration.FromMilliseconds 49513784L
        let timeZone = DateTimeZoneProviders.Tzdb.["America/New_York"]
        let zonedDateTime = instant.InZone timeZone
        let localDateTime = zonedDateTime.LocalDateTime
        let interval = Interval(instant, instant.Plus duration)
        {Container.Instant = stringSchema instant;
         Container.LocalDate = stringSchema localDateTime.Date;
         Container.LocalTime = stringSchema localDateTime.TimeOfDay;
         Container.LocalDateTime = stringSchema localDateTime;
         Container.OffsetDateTime = 
             stringSchema(zonedDateTime.ToOffsetDateTime());
         Container.ZonedDateTime = stringSchema zonedDateTime;
         Container.Interval = 
             Schema(Type = "object", 
                    Properties = dict [("Start", stringSchema interval.Start);
                                       ("End", stringSchema interval.End)]);
         Container.Offset = stringSchema timeZone.MaxOffset;
         Container.Period = 
             stringSchema
                 (Period.Between
                      (localDateTime, 
                       localDateTime.PlusTicks duration.BclCompatibleTicks));
         Container.Duration = stringSchema duration;
         Container.DateTimeZone = stringSchema timeZone}