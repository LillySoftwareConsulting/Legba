using System.Text.Json.Serialization;

namespace Legba.Engine.Models.OpenAi;

public class Message
{
    [JsonPropertyName("role")]
    public Enums.Role Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsInitialSourceCode { get; set; } = false;

    [JsonIgnore]
    public bool IsSentByUser { get { return Role == Enums.Role.User || Role == Enums.Role.System; } }

    [JsonIgnore]
    public string DisplayText
    {
        get
        {
            if (Role == Enums.Role.System)
            {
                return "Personality sent";
            }
            else if (IsInitialSourceCode)
            {
                return "Soure code sent";
            }
            else
            {
                return Content;
            }
        }
    }

    [JsonIgnore]
    public System.Windows.Media.Brush BackgroundColor
    {
        get
        {
            if (Role == Enums.Role.System || IsInitialSourceCode)
            {
                return System.Windows.Media.Brushes.Gold;
            }

            return IsSentByUser
                ? System.Windows.Media.Brushes.LightBlue
                : System.Windows.Media.Brushes.LightGray;
        }
    }

    [JsonIgnore]
    public System.Windows.TextAlignment Alignment
    {
        get
        {
            if (Role == Enums.Role.System || IsInitialSourceCode)
            {
                return System.Windows.TextAlignment.Center;
            }

            return IsSentByUser
                ? System.Windows.TextAlignment.Right
                : System.Windows.TextAlignment.Left;
        }
    }
}