namespace Producer_Consumer
{
    public class Guest : MaxItemBase
    {
        const int maxCount = 5, maxCakeCount = 2;
        public int GuestId { get; set; }
        public int? ConsumedCookieCount { get; set; }
        public int? ConsumedCakeCount { get; set; }
        public int? ConsumedDrinkCount { get; set; }
        public bool HasReachedMaxCounts
        {
            get
            {
                return (this.ConsumedCakeCount + this.ConsumedCookieCount + this.ConsumedDrinkCount) == (this.MaxCakeCount + this.MaxCookieCount + this.MaxDrinkCount);
            }
        }
        public override int ItemCount()
        {
            return ConsumedCakeCount.GetValueOrDefault(0) + ConsumedDrinkCount.GetValueOrDefault(0) + ConsumedCookieCount.GetValueOrDefault(0);
        }
        public Guest()
        {
            this.ConsumedCookieCount = this.ConsumedDrinkCount = this.ConsumedCakeCount = 0;
            this.MaxCakeCount = maxCakeCount;
            this.MaxCookieCount = this.MaxDrinkCount = maxCount;
        }
    }
}
