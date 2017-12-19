namespace Swashbuckle.NodaTime.AspNetCore.Tests

open Newtonsoft.Json
open NodaTime
open NodaTime.Serialization.JsonNet
open Swashbuckle.AspNetCore.Swagger
open Swashbuckle.NodaTime.AspNetCore
open Xunit

module SchemasTests = 
    let private serializerSettings = 
        JsonSerializerSettings()
            .ConfigureForNodaTime DateTimeZoneProviders.Tzdb
    let private deserialize<'T> json = 
        JsonConvert.DeserializeObject<'T>(json, serializerSettings)
    
    let private tryDeserializeExample<'T>(schema : Schema) = 
        schema.Example
        |> string
        |> JsonConvert.SerializeObject
        |> deserialize<'T>
        |> ignore

    let private schemas = serializerSettings |> Schemas.Create
    
    [<Fact>]
    let private ``Instant schema should be deserializable``() = 
        tryDeserializeExample<Instant> schemas.Instant

    [<Fact>]
    let private ``DateTimeZone schema should be deserializable``() = 
        tryDeserializeExample<DateTimeZone> schemas.DateTimeZone

    [<Fact>]
    let private ``Duration schema should be deserializable``() = 
        tryDeserializeExample<Duration> schemas.Duration

    [<Fact>]
    let private ``LocalDate schema should be deserializable``() = 
        tryDeserializeExample<LocalDate> schemas.LocalDate

    [<Fact>]
    let private ``LocalDateTime schema should be deserializable``() = 
        tryDeserializeExample<LocalDateTime> schemas.LocalDateTime

    [<Fact>]
    let private ``LocalTime schema should be deserializable``() = 
        tryDeserializeExample<LocalTime> schemas.LocalTime

    [<Fact>]
    let private ``Offset schema should be deserializable``() = 
        tryDeserializeExample<Offset> schemas.Offset

    [<Fact>]
    let private ``OffsetDateTime schema should be deserializable``() = 
        tryDeserializeExample<OffsetDateTime> schemas.OffsetDateTime

    [<Fact>]
    let private ``Period schema should be deserializable``() = 
        tryDeserializeExample<Period> schemas.Period

    [<Fact>]
    let private ``ZonedDateTime schema should be deserializable``() = 
        tryDeserializeExample<ZonedDateTime> schemas.ZonedDateTime
