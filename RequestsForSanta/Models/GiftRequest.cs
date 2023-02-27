namespace RequestsForSanta
{
    public class GiftDetails
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
    public class GiftRequest
    {
        public string ChildName { get; set; }
        public float Age { get; set; }
        public string Address { get; set; }
        public List<GiftDetails> Gifts { get; set; }
    }
}
