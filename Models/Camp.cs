namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    public class Camp
    {
        public int Id { get; set; }
        public string CampName { get; set; }
        public string CampLocation { get; set; }

        public CampMaster CampMaster { get; set; }
    }
}