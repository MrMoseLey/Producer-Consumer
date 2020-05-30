namespace Producer_Consumer
{
    public class Tray : MaxItemBase
    {
        const int maxCount = 5;
        public int TrayId { get; set; }
        public Food FoodType { get; set; }
        public int ActiveCookieCount { get; set; }
        public int ActiveCakeCount { get; set; }
        public int ActiveDrinkCount { get; set; }
        public override int ItemCount()
        {
            return ActiveCakeCount + ActiveDrinkCount + ActiveCakeCount;
        }
        public Tray()
        {
            this.ActiveCakeCount = this.ActiveDrinkCount = this.ActiveCookieCount = maxCount;
            this.MaxCakeCount = this.MaxDrinkCount = this.MaxCookieCount = maxCount;
        }
    }
}