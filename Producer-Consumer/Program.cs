using System;

namespace Producer_Consumer
{
    class Program
    {
        const int guestCount = 10, trayCount = 3;
        static Tray[] trayArray = new Tray[trayCount];
        static Guest[] guestArray = new Guest[guestCount];
        static int randomGuest, randomTray, randomFood;
        static ServiceQueue serviceQueue;
        static Random random = new Random();
        static Factory factory;
        static void Main(string[] args)
        {
            CreateProducerAndConsumers();
            serviceQueue = new ServiceQueue(trayCount);
            while(serviceQueue.ProduceCount() != 0)
            {
                randomGuest = random.Next(0, 10);
                randomTray = random.Next(0, 3);
                randomFood = random.Next(0, 3);
                if(!guestArray[randomGuest].HasReachedMaxCounts)
                {
                    factory = new Factory()
                    {
                        Food = randomFood,
                        Guest = guestArray[randomGuest],
                        Tray = trayArray[randomTray]
                    };
                    serviceQueue.EnqueueTask(factory);
                }
            }
            Console.ReadKey();
        }
        static void CreateProducerAndConsumers()
        {
            for(int i = 0; i < guestCount; i++)
            {
                guestArray[i] = new Guest()
                {
                    GuestId = i + 1
                };
            }
            for(int i = 0; i < trayCount; i++)
            {
                trayArray[i] = new Tray()
                {
                    TrayId = i + 1
                };
            }
        }
    }
}