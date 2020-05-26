namespace Producer_Consumer
{
    public class Tray : MaxItemBase
    {
        const int maxCount = 5;
        public int TrayId { get; set; }
        public int? ActiveCookieCount { get; set; }
        public int? ActiveCakeCount { get; set; }
        public int? ActiveDrinkCount { get; set; }
        public override int ItemCount()
        {
            return ActiveCakeCount.GetValueOrDefault(0) + ActiveDrinkCount.GetValueOrDefault(0) + ActiveCakeCount.GetValueOrDefault(0);
        }
        public Tray()
        {
            this.ActiveCakeCount = this.ActiveDrinkCount = this.ActiveCookieCount = 0;
            this.MaxCakeCount = this.MaxDrinkCount = this.MaxCookieCount = maxCount;
        }
    }
}