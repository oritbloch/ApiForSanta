using DBContextMigrations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForSanta
{
    public class ChildGiftRequest
    {
        public long Id { get; set; }
        public string ChildName { get; set; }
        public float Age { get; set; }
        public string Address { get; set; }

        public List<GiftOfChild> childGifts { get; set; }
    }
}
