namespace Swashbuckle.NodaTime.Tests

open Xunit

open Swashbuckle.AspNetCore.Swagger

open Newtonsoft.Json

open NodaTime
open NodaTime.Serialization.JsonNet

open Swashbuckle.NodaTime

module SchemasTests =

    let private serializerSettings =
        JsonSerializerSettings()
            .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)

    let private deserialize<'T> json =
        JsonConvert.DeserializeObject<'T>(json, serializerSettings)

    let private tryDeserializeExample<'T> (schema: Schema) =
        schema.Example
        |> string
        |> JsonConvert.SerializeObject
        |> deserialize<'T>
        |> ignore

    [<Fact>]
    let ``Examples in schemas should be deserializable`` () =

        let schemas =
            serializerSettings
            |> Schemas.Create

        tryDeserializeExample<Instant>(schemas.Instant)
        tryDeserializeExample<DateTimeZone>(schemas.DateTimeZone)
        tryDeserializeExample<Duration>(schemas.Duration)
        tryDeserializeExample<LocalDate>(schemas.LocalDate)
        tryDeserializeExample<LocalDateTime>(schemas.LocalDateTime)
        tryDeserializeExample<LocalTime>(schemas.LocalTime)
        tryDeserializeExample<Offset>(schemas.Offset)
        tryDeserializeExample<OffsetDateTime>(schemas.OffsetDateTime)
        tryDeserializeExample<Period>(schemas.Period)
        tryDeserializeExample<ZonedDateTime>(schemas.ZonedDateTime)
