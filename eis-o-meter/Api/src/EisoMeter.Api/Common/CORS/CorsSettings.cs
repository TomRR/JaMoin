using System.ComponentModel.DataAnnotations;

namespace EisoMeter.Api.Common.CORS;

public sealed class CorsSettings
{
    public static string SectionName => "CorsSettings";

    [Required]
    public required string OriginUri { get; set; }
}