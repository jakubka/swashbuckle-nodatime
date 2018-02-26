namespace Swashbuckle.NodaTime.AspNetCore.Tests

open Newtonsoft.Json
open NodaTime
open NodaTime.Serialization.JsonNet
open Swashbuckle.AspNetCore.Swagger
open Swashbuckle.NodaTime.AspNetCore.Schemas
open Xunit

module SchemasTests =
    // Initialize default settings
    do JsonConvert.DefaultSettings <- fun () ->
        JsonSerializerSettings().ConfigureForNodaTime DateTimeZoneProviders.Tzdb
    
    // Helper function for example deserialization
    let private tryDeserializeExample<'T>(schema : Schema) =
        schema.Example
        |> JsonConvert.SerializeObject
        |> JsonConvert.DeserializeObject<'T>
        |> ignore
    
    // Container gets re-used across all test methods
    let private schemas = SchemaCreator().Create()
    
    [<Fact>]
    let private ``Instant schema example should be deserializable``() =
        tryDeserializeExample<Instant> schemas.Instant
    
    [<Fact>]
    let private ``Instant schema format should be date-time``() =
        Assert.Equal("date-time", schemas.Instant.Format)
    
    [<Fact>]
    let private ``DateTimeZone schema example should be deserializable``() =
        tryDeserializeExample<DateTimeZone> schemas.DateTimeZone
    
    [<Fact>]
    let private ``DateTimeZone schema format should be null``() =
        Assert.Null schemas.DateTimeZone.Format
    
    [<Fact>]
    let private ``Duration schema example should be deserializable``() =
        tryDeserializeExample<Duration> schemas.Duration
    
    [<Fact>]
    let private ``Duration schema format should be null``() = 
        Assert.Null schemas.Duration.Format
    
    [<Fact>]
    let private ``LocalDate schema example should be deserializable``() =
        tryDeserializeExample<LocalDate> schemas.LocalDate
    
    [<Fact>]
    let private ``LocalDate schema format should be full-date``() =
        Assert.Equal("full-date", schemas.LocalDate.Format)
    
    [<Fact>]
    let private ``LocalDateTime schema example should be deserializable``() =
        tryDeserializeExample<LocalDateTime> schemas.LocalDateTime
    
    [<Fact>]
    let private ``LocalDateTime schema format should be date-time``() =
        Assert.Null schemas.LocalDateTime.Format
    
    [<Fact>]
    let private ``LocalTime schema example should be deserializable``() =
        tryDeserializeExample<LocalTime> schemas.LocalTime
    
    [<Fact>]
    let private ``LocalTime schema format should be full-time``() =
        Assert.Equal("partial-time", schemas.LocalTime.Format)
    
    [<Fact>]
    let private ``Offset schema should example be deserializable``() =
        tryDeserializeExample<Offset> schemas.Offset
    
    [<Fact>]
    let private ``Offset schema format should be time-numoffset``() =
        Assert.Equal("time-numoffset", schemas.Offset.Format)
    
    [<Fact>]
    let private ``OffsetDateTime schema example should be deserializable``() =
        tryDeserializeExample<OffsetDateTime> schemas.OffsetDateTime
    
    [<Fact>]
    let private ``OffsetDateTime schema format should be date-time``() =
        Assert.Equal("date-time", schemas.OffsetDateTime.Format)
    
    [<Fact>]
    let private ``Period schema example should be deserializable``() =
        tryDeserializeExample<Period> schemas.Period
    
    [<Fact>]
    let private ``Period schema format should be null``() =
        Assert.Null schemas.Period.Format
    
    [<Fact>]
    let private ``ZonedDateTime schema example should be deserializable``() =
        tryDeserializeExample<ZonedDateTime> schemas.ZonedDateTime
    
    [<Fact>]
    let private ``ZonedDateTime schema format should be null``() =
        Assert.Null schemas.ZonedDateTime.Format
    
    [<Fact>]
    let private ``Interval schema example should be null``() =
        Assert.Null schemas.Interval.Example
    
    [<Fact>]
    let private ``Interval schema start & end formats should be date-time``() =
        Assert.Equal("date-time", schemas.Interval.Properties.["Start"].Format)
        Assert.Equal("date-time", schemas.Interval.Properties.["End"].Format)
