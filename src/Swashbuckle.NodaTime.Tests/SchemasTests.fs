namespace Swashbuckle.NodaTime.Tests

open System
open System.Text.Json
open Microsoft.OpenApi.Any
open Microsoft.OpenApi.Interfaces
open Microsoft.OpenApi.Models
open Xunit
open NodaTime
open Swashbuckle.NodaTime
open NodaTime.Serialization.SystemTextJson

module SchemasTests =

    let private serializerSettings =
        JsonSerializerOptions()
            .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb)

    let private deserialize<'T> (json: string) =
        JsonSerializer.Deserialize<'T>(json, serializerSettings)

    let private tryDeserializeExample<'T> (schema: OpenApiSchema) =
        schema.Example
        :?> OpenApiString// Downcast the IOpenApiAny type to OpenApiString so that we can get the value.
        |> fun (x : OpenApiString) -> x.Value
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
