namespace Producer_Consumer
{
    public class Guest : MaxItemBase
    {
        const int maxCount = 5, maxCakeCount = 2;
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public int ConsumedCookieCount { get; set; }
        public int ConsumedCakeCount { get; set; }
        public int ConsumedDrinkCount { get; set; }
        public string GuestInfo
        {
            get
            {
                return this.GuestName + this.GuestId.ToString();
            }
        }
        public bool HasReachedMaxCounts
        {
            get
            {
                return (this.ConsumedCakeCount + this.ConsumedCookieCount + this.ConsumedDrinkCount) == (this.MaxCakeCount + this.MaxCookieCount + this.MaxDrinkCount);
            }
        }
        public bool HasConsumedAll
        {
            get
            {
                return this.ConsumedCakeCount > 0 && this.ConsumedCookieCount > 0 && this.ConsumedDrinkCount > 0;
            }
        }
        public Guest()
        {
            this.ConsumedCookieCount = this.ConsumedDrinkCount = this.ConsumedCakeCount = 0;
            this.MaxCakeCount = maxCakeCount;
            this.MaxCookieCount = this.MaxDrinkCount = maxCount;
        }
        public override int ItemCount()
        {
            return this.ConsumedCakeCount + this.ConsumedDrinkCount + this.ConsumedCookieCount;
        }
    }
}
