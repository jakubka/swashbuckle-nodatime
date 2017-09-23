namespace Swashbuckle.NodaTime

open System

open Swashbuckle.AspNetCore.Swagger

open Newtonsoft.Json

open NodaTime

module Schemas =

    type Container =
        {
            Instant: Schema
            LocalDate: Schema
            LocalTime: Schema
            LocalDateTime: Schema
            OffsetDateTime: Schema
            ZonedDateTime: Schema
            Interval: Schema
            Offset: Schema
            Period: Schema
            Duration: Schema
            DateTimeZone: Schema
        }

    let Create (serializerSettings: JsonSerializerSettings): Container =
        let stringRepresentation value =
            // this produces value including quotes, for example: "13:45:13.784"
            let jsonString = JsonConvert.SerializeObject(value, serializerSettings)

            // deserializing into string will remove quotes
            JsonConvert.DeserializeObject<string>(jsonString)

        let stringSchema value =
            Schema(
                Type = "string",
                Example = stringRepresentation value)

        let instant = Instant.FromDateTimeOffset(DateTimeOffset.Now)
        let duration = Duration.FromMilliseconds(49513784L)
        let timeZone = DateTimeZoneProviders.Tzdb.["America/New_York"]

        let zonedDateTime = instant.InZone(timeZone)
        let localDateTime = zonedDateTime.LocalDateTime
        let localDate = localDateTime.Date
        let localTime = localDateTime.TimeOfDay
        let offsetDateTime = zonedDateTime.ToOffsetDateTime()
        let interval = Interval(instant, instant.Plus(duration))
        let offset = timeZone.MaxOffset
        let period = Period.Between(localDateTime, localDateTime.PlusTicks(duration.BclCompatibleTicks))

        let intervalSchema =
            let properties = System.Collections.Generic.Dictionary<string, Schema>()
            properties.Add("Start", stringSchema interval.Start)
            properties.Add("End", stringSchema interval.End)

            Schema(
                Type = "object",
                Properties = properties)

        {
            
            Container.Instant = stringSchema instant
            Container.LocalDate = stringSchema localDate
            Container.LocalTime = stringSchema localTime
            Container.LocalDateTime = stringSchema localDateTime
            Container.OffsetDateTime = stringSchema offsetDateTime
            Container.ZonedDateTime = stringSchema zonedDateTime
            Container.Interval = intervalSchema
            Container.Offset = stringSchema offset
            Container.Period = stringSchema period
            Container.Duration = stringSchema duration
            Container.DateTimeZone = stringSchema timeZone
        }
