using DBContextMigrations;
using Microsoft.EntityFrameworkCore;

namespace RequestsForSanta
{
    public static class ServiceHelper
    {
        public static Gift GetGiftByName(SantaDBContext dbContext,string name)
        {
            try
            {
               return dbContext.Gifts.Where(x => x.Name == name).FirstOrDefault();
            }
            catch(Exception ex)
            {
                //write a log somewhere
                throw ex;
            }
            return null;
        }

        public static GiftOfChild GetSpecificGiftOfChild(SantaDBContext dbContext, string name,float age,string giftName)
        {
            try
            {
                ChildGiftRequest req = dbContext.GiftRequests.Include(c => c.childGifts).ThenInclude(s=>s.gift).Where(x => x.ChildName == name && x.Age == age && x.childGifts.Any(y => y.gift.Name == giftName)).FirstOrDefault();
                if (req != null && req.childGifts!=null)
                {
                    return req.childGifts.FirstOrDefault(y => y.gift.Name == giftName);
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public static ChildGiftRequest GetRequestOfChild(SantaDBContext dbContext, string name, float age)
        {
            try
            {
                return dbContext.GiftRequests.Include(c => c.childGifts).ThenInclude(y=>y.gift).Where(x => x.ChildName == name && x.Age == age).FirstOrDefault();

            }
            catch
            {
                return null;
            }
            return null;
        }
    }
    }
