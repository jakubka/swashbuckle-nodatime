namespace Swashbuckle.NodaTime

open System.Runtime.CompilerServices

open Swashbuckle.Application

open NodaTime

open Schemas

[<AutoOpen>]
[<Extension>]
module SwaggerDocsConfigExtensions =

    [<Extension>]
    let ConfigureForNodaTime (config: SwaggerDocsConfig) =
        let schemas =
            Schemas.Create ()

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

    type SwaggerDocsConfig with
        member this.ConfigureForNodaTime() = this |> ConfigureForNodaTime
