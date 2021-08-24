namespace Swashbuckle.NodaTime.AspNetCore

open System
open Microsoft.OpenApi.Any
open Microsoft.OpenApi.Models
open Newtonsoft.Json
open NodaTime
open NodaTime.TimeZones

module internal Schemas =
  type Container =
    {Instant : OpenApiSchema;
     LocalDate : OpenApiSchema;
     LocalTime : OpenApiSchema;
     LocalDateTime : OpenApiSchema;
     OffsetDateTime : OpenApiSchema;
     ZonedDateTime : OpenApiSchema;
     Interval : OpenApiSchema;
     Offset : OpenApiSchema;
     Period : OpenApiSchema;
     Duration : OpenApiSchema;
     DateTimeZone : OpenApiSchema}

  type SchemaCreator(?serializerSettings : JsonSerializerSettings) =

    let defaultSerializer =
      match JsonConvert.DefaultSettings with
      | null -> null
      | _ -> JsonConvert.DefaultSettings.Invoke()

    let settings = defaultArg serializerSettings defaultSerializer

    // F# won't allow optional parameters on a let binding
    // So made a type to have a private method declared
    member private __.StringSchema(value, ?format) =
      OpenApiSchema(Type = "string",
             Example = OpenApiString(JsonConvert.SerializeObject(value, settings) |> JsonConvert.DeserializeObject<string>),
             Format = match format with
                      | Some x -> x
                      | None -> null)

    member __.Create() =
      let dateTimeZone =
        try
          DateTimeZoneProviders.Tzdb.GetSystemDefault()
        with :? DateTimeZoneNotFoundException ->
          DateTimeZoneProviders.Tzdb.["America/New_York"]

      let instant = Instant.FromUnixTimeTicks(15759724104663180L)
      let zonedDateTime = instant.InZone dateTimeZone
      let interval =
        Interval
          (instant,
           instant.PlusTicks(TimeSpan.TicksPerDay)
                  .PlusTicks(TimeSpan.TicksPerHour)
                  .PlusTicks(TimeSpan.TicksPerMinute)
                  .PlusTicks(TimeSpan.TicksPerSecond)
                  .PlusTicks(TimeSpan.TicksPerMillisecond))
      {Container.Instant = __.StringSchema(instant, "date-time");
       Container.LocalDate = __.StringSchema(zonedDateTime.Date, "full-date");
       Container.LocalTime =
         __.StringSchema(zonedDateTime.TimeOfDay, "partial-time");
       Container.LocalDateTime = __.StringSchema zonedDateTime.LocalDateTime;
       Container.OffsetDateTime =
         __.StringSchema(instant.WithOffset zonedDateTime.Offset, "date-time");
       Container.ZonedDateTime = __.StringSchema zonedDateTime;
       Container.Interval =
         OpenApiSchema
           (Type = "object",
            Properties = dict
                           [("Start",
                             __.StringSchema(interval.Start, "date-time"));
                            ("End", __.StringSchema(interval.End, "date-time"))]);
       Container.Offset =
         __.StringSchema(zonedDateTime.Offset, "time-numoffset");
       Container.Period =
         __.StringSchema
           (Period.Between
              (zonedDateTime.LocalDateTime,
               interval.End.InZone(dateTimeZone).LocalDateTime,
               PeriodUnits.AllUnits));
       Container.Duration = __.StringSchema interval.Duration;
       Container.DateTimeZone = __.StringSchema dateTimeZone}
