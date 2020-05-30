namespace Producer_Consumer
{
    public abstract class MaxItemBase
    {
        public int MaxCookieCount { get; set; }
        public int MaxCakeCount { get; set; }
        public int MaxDrinkCount { get; set; }
        public abstract int ItemCount();
    }
}
