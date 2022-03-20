namespace Swashbuckle.NodaTime

open System.Text.Json
open Microsoft.OpenApi.Any
open Microsoft.OpenApi.Models
open NodaTime

module Schemas =

    type Container =
        {
            Instant: OpenApiSchema
            LocalDate: OpenApiSchema
            LocalTime: OpenApiSchema
            LocalDateTime: OpenApiSchema
            OffsetDateTime: OpenApiSchema
            ZonedDateTime: OpenApiSchema
            Interval: OpenApiSchema
            Offset: OpenApiSchema
            Period: OpenApiSchema
            Duration: OpenApiSchema
            DateTimeZone: OpenApiSchema
        }

    let Create (serializerSettings: JsonSerializerOptions): Container =
        let stringRepresentation value =
            // this produces value including quotes, for example: "13:45:13.784"
            let jsonString = JsonSerializer.Serialize(value, serializerSettings)

            // deserializing into string will remove quotes
            JsonSerializer.Deserialize<string>(jsonString)

        let stringSchema value =
            OpenApiSchema(
                Type = "object",
                Example = OpenApiString(stringRepresentation value))

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
        let period = Period.Between(localDateTime, localDateTime.PlusTicks(duration.BclCompatibleTicks))

        let intervalSchema =
            let properties = System.Collections.Generic.Dictionary<string, OpenApiSchema>()
            properties.Add("Start", stringSchema interval.Start)
            properties.Add("End", stringSchema interval.End)

            OpenApiSchema(
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
