namespace PMS.Models
{
    public class Medicine
    {
        public int Id{ get; set; }
        public string Name{ get; set; }
        public int Manufacture_ID { get; set; }
        public string Manufacture { get; set; }
        public float Unit { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

    }
}
