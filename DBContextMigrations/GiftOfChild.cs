using RequestsForSanta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContextMigrations
{
    public class GiftOfChild
    {
        public long id { get; set; }
        public Gift gift { get; set; }
        public string color { get; set; }
        public ChildGiftRequest request { get; set; }
        public long requestID { get; set; }
    }
}
