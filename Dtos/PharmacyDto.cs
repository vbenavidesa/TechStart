namespace TechStart.Dtos
{
    public class PharmacyDto
    {
        public long Id { get; set; }
        public long HospitalId { get; set; }
        public HospitalDto Hospital { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}