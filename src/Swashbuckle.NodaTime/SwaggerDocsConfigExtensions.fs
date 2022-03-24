namespace Swashbuckle.NodaTime

open System.Runtime.CompilerServices
open System.Text.Json
open Microsoft.Extensions.DependencyInjection
open NodaTime
open Schemas
open Swashbuckle.AspNetCore.SwaggerGen

[<AutoOpen>]
[<Extension>]
module SwaggerDocsConfigExtensions =

    [<Extension>]
    let ConfigureForNodaTime (config: SwaggerGenOptions, serializerSettings: JsonSerializerOptions) =
        let schemas =
            serializerSettings
            |> Create

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

        config.MapType<System.Nullable<Instant>>(fun () -> schemas.Instant)
        config.MapType<System.Nullable<LocalDate>>(fun () -> schemas.LocalDate)
        config.MapType<System.Nullable<LocalTime>>(fun () -> schemas.LocalTime)
        config.MapType<System.Nullable<LocalDateTime>>(fun () -> schemas.LocalDateTime)
        config.MapType<System.Nullable<OffsetDateTime>>(fun () -> schemas.OffsetDateTime)
        config.MapType<System.Nullable<ZonedDateTime>>(fun () -> schemas.ZonedDateTime)
        config.MapType<System.Nullable<Interval>>(fun () -> schemas.Interval)
        config.MapType<System.Nullable<Offset>>(fun () -> schemas.Offset)
        config.MapType<System.Nullable<Duration>>(fun () -> schemas.Duration)

    type SwaggerGenOptions with
        member this.ConfigureForNodaTime(serializerSettings: JsonSerializerOptions) =
            ConfigureForNodaTime(this, serializerSettings)
