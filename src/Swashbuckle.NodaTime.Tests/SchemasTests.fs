namespace Swashbuckle.NodaTime.Tests

open System.Text.Json
open Microsoft.OpenApi.Models
open Xunit
open NodaTime
open Swashbuckle.NodaTime

module SchemasTests =

    let private serializerSettings =
        JsonSerializerOptions()
            .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)

    let private deserialize<'T> json =
        JsonSerializer.Deserialize<'T>(json, serializerSettings)

    let private tryDeserializeExample<'T> (schema: OpenApiSchema) =
        schema.Example
        |> string
        |> JsonSerializer.Serialize
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
