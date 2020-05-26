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
        void Consume()
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
                //Thread.Sleep(1000);
            }
            Thread.Sleep(100);
            lock(lockObject)
            {
                Console.WriteLine("Tüm ürünler tüketilmiştir.");
                this.Dispose();
            }
        }
        void CalculateFoodCounts(Tray tray, Guest guest, int food)
        {
            lock(lockObject)
            {
                switch((Food)food)
                {
                    case Food.Cake:
                        ProduceCake(tray);
                        ConsumeCake(tray, guest);
                        break;
                    case Food.Drink:
                        ProduceDrink(tray);
                        ConsumeDrink(tray, guest);
                        break;
                    case Food.Cookie:
                        ProduceCookie(tray);
                        ConsumeCookie(tray, guest);
                        break;
                    default:
                        break;
                }
            }
        }
        void ConsumeCake(Tray tray, Guest guest)
        {
            if(guest.ConsumedCakeCount < guest.MaxCakeCount)
            {
                if(tray.ActiveCakeCount == 0)
                    Console.WriteLine("{0} numaralı müşterinin almak istediği kek tüketim ürünü {1} numaralı tepside tükenmiştir.", guest.GuestId, tray.TrayId);
                else
                {
                    ++guest.ConsumedCakeCount;
                    --tray.ActiveCakeCount;
                    Console.WriteLine("{0} numaralı müşteri {1} numaralı tepsinden kek ürününü tüketmiştir.", guest.GuestId, tray.TrayId);
                    Console.WriteLine("Toplam kalan kek sayısı: {0}", cakeCount);
                }
            }
            else
                Console.WriteLine("{0} numaralı müşterinin almak istediği kek tüketim ürünü için tüketim hakkı dolmuştur.", guest.GuestId);
        }
        void ConsumeDrink(Tray tray, Guest guest)
        {
            if(guest.ConsumedDrinkCount < guest.MaxDrinkCount)
            {
                if(tray.ActiveDrinkCount == 0)
                    Console.WriteLine("{0} numaralı müşterinin almak istediği içecek tüketim ürünü {1} numaralı tepside tükenmiştir.", guest.GuestId, tray.TrayId);
                else
                {
                    ++guest.ConsumedDrinkCount;
                    --tray.ActiveDrinkCount;
                    Console.WriteLine("{0} numaralı müşteri {1} numaralı tepsinden içecek ürününü tüketmiştir.", guest.GuestId, tray.TrayId);
                    Console.WriteLine("Toplam kalan içecek sayısı: {0}", drinkCount);
                }
            }
            else
                Console.WriteLine("{0} numaralı müşterinin almak istediği içecek tüketim ürünü için tüketim hakkı dolmuştur.", guest.GuestId);
        }
        void ConsumeCookie(Tray tray, Guest guest)
        {
            if(guest.ConsumedCookieCount < guest.MaxCookieCount)
            {
                if(tray.ActiveCookieCount == 0)
                    Console.WriteLine("{0} numaralı müşterinin almak istediği kurabiye tüketim ürünü {1} numaralı tepside tükenmiştir.", guest.GuestId, tray.TrayId);
                else
                {
                    ++guest.ConsumedCookieCount;
                    --tray.ActiveCookieCount;
                    Console.WriteLine("{0} numaralı müşteri {1} numaralı tepsinden kurabiye ürününü tüketmiştir.", guest.GuestId, tray.TrayId);
                    Console.WriteLine("Toplam kalan kurabiye sayısı: {0}", cookieCount);
                }
            }
            else
                Console.WriteLine("{0} numaralı müşterinin almak istediği kurabiye tüketim ürünü için tüketim hakkı dolmuştur.", guest.GuestId);
        }
        void ProduceCake(Tray tray)
        {
            if(cakeCount > 0 && tray.MaxCakeCount != tray.ActiveCakeCount)
            {
                --cakeCount;
                ++tray.ActiveCakeCount;
                Console.WriteLine("{0} numaralı tepsi için kek üretimi tamamlandı.", tray.TrayId);
            }
        }
        void ProduceDrink(Tray tray)
        {
            if(drinkCount > 0 && tray.MaxDrinkCount != tray.ActiveDrinkCount)
            {
                --drinkCount;
                ++tray.ActiveDrinkCount;
                Console.WriteLine("{0} numaralı tepsi için içecek üretimi tamamlandı.", tray.TrayId);
            }
        }
        void ProduceCookie(Tray tray)
        {
            if(cookieCount > 0 && tray.MaxCookieCount != tray.ActiveCookieCount)
            {
                --cookieCount;
                ++tray.ActiveCookieCount;
                Console.WriteLine("{0} numaralı tepsi için kurabiye üretimi tamamlandı.", tray.TrayId);
            }
        }
        public int ProduceCount()
        {
            return cookieCount + cakeCount + drinkCount;
        }
    }
}
