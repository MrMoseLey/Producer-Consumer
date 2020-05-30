using System;
using System.Collections.Generic;
using System.Threading;

namespace Producer_Consumer
{
    public class ServiceQueue : IDisposable
    {
        static int cookieCount = 30, cakeCount = 15, drinkCount = 30;
        private object lockObject = new object();
        private Thread[] threads;
        private Queue<Factory> tasks = new Queue<Factory>();
        public ServiceQueue(int workerCount)
        {
            threads = new Thread[workerCount];
            for(int i = 0; i < workerCount; i++)
                (threads[i] = new Thread(Consume)).Start();
        }
        public void Dispose()
        {
            foreach(Thread worker in threads)
                EnqueueTask(null);
            foreach(Thread worker in threads)
                worker.Join();
        }
        public void EnqueueTask(Factory factory)
        {
            lock(lockObject)
            {
                tasks.Enqueue(factory);
                Monitor.PulseAll(lockObject);
            }
        }
        private void Consume()
        {
            while(ProduceCount() != 0)
            {
                Factory task;
                lock(lockObject)
                {
                    while(tasks.Count == 0)
                        Monitor.Wait(lockObject);
                    task = tasks.Dequeue();
                }
                if(task == null)
                    return;
                CalculateFoodCounts(task.Tray, task.Guest, task.Food);
                Console.WriteLine();
                Thread.Sleep(1000);
            }
            Thread.Sleep(250);
            lock(lockObject)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Tüm ürünler tüketilmiştir.");
                this.Dispose();
            }
        }
        private void CalculateFoodCounts(Tray tray, Guest guest, Food food)
        {
            lock(lockObject)
            {
                switch(food)
                {
                    case Food.Cake:
                        ConsumeCake(tray, guest);
                        break;
                    case Food.Drink:
                        ConsumeDrink(tray, guest);
                        break;
                    case Food.Cookie:
                        ConsumeCookie(tray, guest);
                        break;
                    default:
                        break;
                }
            }
        }
        private void TrayDoesntExistItem(string guestName, string foodName, string trayId)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} numaralı müşterinin almak istediği {1} tüketim ürünü {2} numaralı tepside tükenmiştir.", guestName, foodName, trayId);
        }
        private void ConsumedItem(string guestName, string foodName, string trayId)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} numaralı müşteri {1} numaralı tepsinden {2} ürününü tüketmiştir.", guestName, trayId, foodName);
        }
        private void ReachedMaxConsumeCount(string guestName, string foodName)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("{0} numaralı müşterinin almak istediği {1} tüketim ürünü için tüketim hakkı dolmuştur.", guestName, foodName);
        }
        private void ProduceInfo(string trayId, string foodName)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("{0} numaralı tepsi için maksimum kapasitesi kadar {1} üretimi gerçekleştirildi.", trayId, foodName);
        }
        private void ConsumeCake(Tray tray, Guest guest)
        {
            if(guest.ConsumedCakeCount < guest.MaxCakeCount)
            {
                if(tray.ActiveCakeCount == 0)
                    TrayDoesntExistItem(guest.GuestInfo, "kek", tray.TrayId.ToString());
                else
                {
                    if(guest.HasConsumedAll || (!guest.HasConsumedAll && guest.ConsumedCakeCount == 0))
                    {
                        ProduceCake(tray);
                        ++guest.ConsumedCakeCount;
                        --tray.ActiveCakeCount;
                        ConsumedItem(guest.GuestInfo, "kek", tray.TrayId.ToString());
                    }
                }
            }
            else
                ReachedMaxConsumeCount(guest.GuestInfo, "kek");
        }
        private void ConsumeDrink(Tray tray, Guest guest)
        {
            if(guest.ConsumedDrinkCount < guest.MaxDrinkCount)
            {
                if(tray.ActiveDrinkCount == 0)
                    TrayDoesntExistItem(guest.GuestInfo, "içecek", tray.TrayId.ToString());
                else
                {
                    if(guest.HasConsumedAll || (!guest.HasConsumedAll && guest.ConsumedDrinkCount == 0))
                    {
                        ProduceDrink(tray);
                        ++guest.ConsumedDrinkCount;
                        --tray.ActiveDrinkCount;
                        ConsumedItem(guest.GuestInfo, "içecek", tray.TrayId.ToString());
                    }
                }
            }
            else
                ReachedMaxConsumeCount(guest.GuestInfo, "içecek");
        }
        private void ConsumeCookie(Tray tray, Guest guest)
        {
            if(guest.ConsumedCookieCount < guest.MaxCookieCount)
            {
                if(tray.ActiveCookieCount == 0)
                    TrayDoesntExistItem(guest.GuestInfo, "kurabiye", tray.TrayId.ToString());
                else
                {
                    if(guest.HasConsumedAll || (!guest.HasConsumedAll && guest.ConsumedCookieCount == 0))
                    {
                        ProduceCookie(tray);
                        ++guest.ConsumedCookieCount;
                        --tray.ActiveCookieCount;
                        ConsumedItem(guest.GuestInfo, "kurabiye", tray.TrayId.ToString());
                    }
                }
            }
            else
                ReachedMaxConsumeCount(guest.GuestInfo, "kurabiye");
        }
        private void ProduceCake(Tray tray)
        {
            if(cakeCount > 0 && tray.MaxCakeCount != tray.ActiveCakeCount)
            {
                if(tray.ActiveCakeCount == 0 || tray.ActiveCakeCount == 1)
                {
                    if(cakeCount == tray.MaxCakeCount)
                    {
                        tray.ActiveCakeCount += cakeCount;
                        cakeCount = 0;
                    }
                    else
                    {
                        tray.ActiveCakeCount += tray.MaxCakeCount;
                        cakeCount -= tray.MaxCakeCount;
                    }
                    ProduceInfo(tray.TrayId.ToString(), "kek");
                }
            }
        }
        private void ProduceDrink(Tray tray)
        {
            if(drinkCount > 0 && tray.MaxDrinkCount != tray.ActiveDrinkCount)
            {
                if(tray.ActiveDrinkCount == 0 || tray.ActiveDrinkCount == 1)
                {
                    if(drinkCount == tray.MaxDrinkCount)
                    {
                        tray.ActiveDrinkCount += drinkCount;
                        drinkCount = 0;
                    }
                    else
                    {
                        tray.ActiveDrinkCount += tray.MaxDrinkCount;
                        drinkCount -= tray.MaxDrinkCount;
                    }
                    ProduceInfo(tray.TrayId.ToString(), "içecek");
                }
            }
        }
        private void ProduceCookie(Tray tray)
        {
            if(cookieCount > 0 && tray.MaxCookieCount != tray.ActiveCookieCount)
            {
                if(tray.ActiveCookieCount == 0 || tray.ActiveCookieCount == 1)
                {
                    if(cookieCount == tray.ActiveCookieCount)
                    {
                        tray.ActiveCookieCount += cookieCount;
                        cookieCount = 0;
                    }
                    else
                    {
                        tray.ActiveCookieCount += tray.MaxCookieCount;
                        cookieCount -= tray.MaxCookieCount;
                    }
                    ProduceInfo(tray.TrayId.ToString(), "kurabiye");
                }
            }
        }
        public int ProduceCount()
        {
            return cookieCount + cakeCount + drinkCount;
        }
    }
}
