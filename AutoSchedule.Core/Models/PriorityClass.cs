namespace AutoSchedule.Core.Models
{
    public struct PriorityClass
    {
        [System.Text.Json.Serialization.JsonInclude]
        [Newtonsoft.Json.JsonRequired]
        public string Name { get; set; }

        [System.Text.Json.Serialization.JsonInclude]
        [Newtonsoft.Json.JsonRequired]
        public Priority Priority { get; set; }
}
}
