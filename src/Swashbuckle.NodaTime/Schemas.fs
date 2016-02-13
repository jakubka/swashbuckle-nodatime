namespace Swashbuckle.NodaTime

open Swashbuckle.Swagger

open Newtonsoft.Json

open NodaTime
open NodaTime.Serialization.JsonNet

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

    let Create (): Container =
        let serializerSettings =
            JsonSerializerSettings()
                .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)

        let serialize value =
            JsonConvert.SerializeObject(value, serializerSettings)

        let stringSchema value =
            Schema(
                ``type`` = "string",
                example = serialize value)

        let instant = Instant.FromUtc(2016, 9, 22, 16, 53)
        let duration = Duration.FromMilliseconds(49513784L)
        let timeZone = DateTimeZoneProviders.Tzdb.["America/New_York"]

        let zonedDateTime = instant.InZone(timeZone)
        let localDateTime = zonedDateTime.LocalDateTime
        let localDate = localDateTime.Date
        let localTime = localDateTime.TimeOfDay
        let offsetDateTime = zonedDateTime.ToOffsetDateTime()
        let interval = Interval(instant, instant.Plus(duration))
        let offset = timeZone.MaxOffset
        let period = Period.Between(localDateTime, localDateTime.PlusTicks(duration.Ticks))

        let intervalSchema =
            let properties = System.Collections.Generic.Dictionary<string, Schema>()
            properties.Add("Start", stringSchema interval.Start)
            properties.Add("End", stringSchema interval.End)

            Schema(
                ``type`` = "object",
                properties = properties)

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
