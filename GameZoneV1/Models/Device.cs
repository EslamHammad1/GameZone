namespace GameZoneV1.Models
{
    public class Device :BaseEntity
    {
        [MaxLength(100)]
        public string Icon { get; set; } = string.Empty;
    }
}
