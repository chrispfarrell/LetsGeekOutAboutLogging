namespace SynergisticLogging.Web.Data
{
    public class DadJoke
    {
        public int Id { get; set; }
        public string Setup { get; set; } = string.Empty;
        public string Punchline { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        
    }
}
