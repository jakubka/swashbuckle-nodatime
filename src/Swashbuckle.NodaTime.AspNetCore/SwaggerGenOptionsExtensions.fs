namespace Microsoft.Extensions.DependencyInjection

open System
open System.Runtime.CompilerServices
open Microsoft.Extensions.DependencyInjection
open Newtonsoft.Json
open NodaTime
open Swashbuckle.AspNetCore.SwaggerGen
open Swashbuckle.NodaTime.AspNetCore.Schemas

[<AutoOpen; Extension>]
type SwaggerGenOptionsExtensions =

  [<Extension>]
  static member ConfigureForNodaTime(config : SwaggerGenOptions,
                                     serializerSettings : JsonSerializerSettings) =
    let schemas = SchemaCreator(serializerSettings).Create()
    config.MapType<Nullable<Instant>>(fun () -> schemas.NullableInstant)
    config.MapType<Nullable<LocalDate>>(fun () -> schemas.NullableLocalDate)
    config.MapType<Nullable<LocalTime>>(fun () -> schemas.NullableLocalTime)
    config.MapType<Nullable<LocalDateTime>>(fun () -> schemas.NullableLocalDateTime)
    config.MapType<Nullable<OffsetDateTime>>(fun () -> schemas.NullableOffsetDateTime)
    config.MapType<Nullable<ZonedDateTime>>(fun () -> schemas.NullableZonedDateTime)
    config.MapType<Nullable<Interval>>(fun () -> schemas.NullableInterval)
    config.MapType<Nullable<Offset>>(fun () -> schemas.NullableOffset)
    config.MapType<Nullable<Duration>>(fun () -> schemas.NullableDuration)
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

  [<Extension>]
  static member ConfigureForNodaTime(config : SwaggerGenOptions) =
    SwaggerGenOptionsExtensions.ConfigureForNodaTime
      (config, JsonConvert.DefaultSettings.Invoke())
