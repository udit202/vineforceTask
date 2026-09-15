namespace vineforceTask.DTO
{
    public class UpdateCountryDto
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}