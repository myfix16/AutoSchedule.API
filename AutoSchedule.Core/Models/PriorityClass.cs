using System.Text.Json.Serialization;

namespace AutoSchedule.Core.Models
{
    public struct PriorityClass
    {
        [JsonInclude] 
        public string Name;

        [JsonInclude] 
        public Priority Priority;
    }
}
