namespace Swashbuckle.NodaTime.AspNetCore

open System
open System.Runtime.CompilerServices
open Newtonsoft.Json
open NodaTime
open Swashbuckle.AspNetCore.SwaggerGen
open Swashbuckle.NodaTime.AspNetCore.Schemas

module internal Config = 
    let Configure(config : SwaggerGenOptions, 
                  serializerSettings : JsonSerializerSettings) = 
        let schemas = SchemaCreator(serializerSettings).Create()
        config.MapType<Instant>(fun () -> schemas.Instant)
        config.MapType<LocalDate>(fun () -> schemas.LocalDate)
        config.MapType<LocalTime>(fun () -> schemas.LocalTime)
        config.MapType<LocalDateTime>(fun () -> schemas.LocalDateTime)
        config.MapType<OffsetDateTime>(fun () -> schemas.OffsetDateTime)
        config.MapType<ZonedDateTime>(fun () -> schemas.ZonedDateTime)
        config.MapType<Interval>(fun () -> schemas.Interval)
        config.MapType<Offset>(fun () -> schemas.Offset)
        config.MapType<Period>(fun () -> schemas.Period)
        config.MapType<Duration>(fun () -> schemas.Duration)
        config.MapType<DateTimeZone>(fun () -> schemas.DateTimeZone)
        config.MapType<Nullable<Instant>>(fun () -> schemas.Instant)
        config.MapType<Nullable<LocalDate>>(fun () -> schemas.LocalDate)
        config.MapType<Nullable<LocalTime>>(fun () -> schemas.LocalTime)
        config.MapType<Nullable<LocalDateTime>>(fun () -> schemas.LocalDateTime)
        config.MapType<Nullable<OffsetDateTime>>
            (fun () -> schemas.OffsetDateTime)
        config.MapType<Nullable<ZonedDateTime>>(fun () -> schemas.ZonedDateTime)
        config.MapType<Nullable<Interval>>(fun () -> schemas.Interval)
        config.MapType<Nullable<Offset>>(fun () -> schemas.Offset)
        config.MapType<Nullable<Duration>>(fun () -> schemas.Duration)

[<AutoOpen>]
[<Extension>]
type SwaggerGenOptionsExtensions = 
    
    [<Extension>]
    static member ConfigureForNodaTime(config : SwaggerGenOptions) = 
        Config.Configure(config, JsonConvert.DefaultSettings.Invoke())
    
    [<Extension>]
    static member ConfigureForNodaTime(config : SwaggerGenOptions, 
                                       serializerSettings : JsonSerializerSettings) = 
        Config.Configure(config, serializerSettings)
