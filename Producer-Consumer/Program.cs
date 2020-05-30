using System;
using System.Collections.Generic;
using System.Linq;

namespace Producer_Consumer
{
    class Program
    {
        const int guestCount = 10, trayCount = 3;
        static Tray[] trayArray = new Tray[trayCount];
        static Guest[] guestArray = new Guest[guestCount];
        static int randomGuest, randomFood;
        static ServiceQueue serviceQueue;
        static Random random = new Random();
        static Factory factory;
        static bool isEnqueueable = true;
        static void Main(string[] args)
        {
            CreateProducerAndConsumers();
            serviceQueue = new ServiceQueue(trayCount);
            while(serviceQueue.ProduceCount() != 0)
            {
                randomGuest = random.Next(0, guestCount);
                randomFood = random.Next(0, Enum.GetValues(typeof(Food)).Cast<int>().Max());
                if(!guestArray[randomGuest].HasReachedMaxCounts)
                {
                    if(!guestArray[randomGuest].HasConsumedAll)
                    {
                        isEnqueueable = false;
                        serviceQueue.EnqueueTask(PrepareFactoryForGuest(guestArray[randomGuest], randomFood));
                    }
                    if(isEnqueueable)
                    {
                        factory = new Factory()
                        {
                            Food = (Food)randomFood,
                            Guest = guestArray[randomGuest],
                            Tray = trayArray[randomFood]
                        };
                        serviceQueue.EnqueueTask(factory);
                    }
                }
            }
            Console.ReadKey();
        }
        private static Factory PrepareFactoryForGuest(Guest guest, int randomFood)
        {
            List<int> notConsumedList = new List<int>()
            {
                Food.Cake.GetHashCode(),
                Food.Cookie.GetHashCode(),
                Food.Drink.GetHashCode()
            };
            notConsumedList.Remove(randomFood);
            int selectedFood = random.Next(0, notConsumedList.Count + 1);
            return new Factory()
            {
                Guest = guest,
                Food = (Food)selectedFood,
                Tray = trayArray[selectedFood]
            };
        }

        static void CreateProducerAndConsumers()
        {
            for(int i = 0; i < guestCount; i++)
            {
                guestArray[i] = new Guest()
                {
                    GuestId = i + 1,
                    GuestName = string.Concat(Enumerable.Repeat("*", i + 1))
                };
            }
            for(int i = 0; i < trayCount; i++)
            {
                trayArray[i] = new Tray()
                {
                    TrayId = i + 1,
                    FoodType = (Food)i
                };
            }
        }
    }
}