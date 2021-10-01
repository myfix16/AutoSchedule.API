using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AutoSchedule.Core.Models
{
    public struct PriorityClass
    {
        [JsonConstructor]
        public PriorityClass(string name, Priority priority)
        {
            Name = name;
            Priority = priority;
        }

        [JsonInclude]
        [Required]
        public readonly string Name;

        [JsonInclude]
        [Required]
        public Priority Priority;
    }
}
