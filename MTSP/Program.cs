using System.Diagnostics;

namespace MTSP
{
    class Program
    {
        static void Main(string[] args)
        {
            string fin = @"..\..\..\data\places_data.xlsx";
            string foutWithoutExtension = @"..\..\..\results\traveler";
            string foutExtension = ".txt";
            Location startLocation = new Location("Kauno m. centras", "start", 495677.140236133, 6084803.83132167);  // Starting location
            Location endLocation = new Location("Kauno m. centras", "end", 495677.140236133, 6084803.83132167);      // Ending location
            int travelersCount = 5;                                                                                  // Different travelers count
            double timePerLocation = 1;                                                                              // Time taken in each location in hours
            double timePerKilometer = 1.0 / 60.0;                                                                    // Travel speed in kilometers per minute
            double timeBeforeBreak = 16;                                                                             // Time before taking a break in hours
            double breakTime = 8;                                                                                    // Break time in hours

            List<Location> locations = InOutUtils.ReadData(fin);
            Population population = new Population(15, startLocation, endLocation, locations, travelersCount, timePerLocation, timePerKilometer, timeBeforeBreak, breakTime);
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(0.01, 5, 3);

            Chromosome fittest = population.GetFittest();
            Console.WriteLine("1 generation: {0:0.00} {1:0.00}", fittest.GetFitnessByTime(), fittest.GetFitnessByDistance());
            int generation = 2;
            int sameIterationCount = 1;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (sameIterationCount <= 100 && stopwatch.Elapsed.TotalSeconds < 20)
            {
                Chromosome lastFittest = fittest;
                population = geneticAlgorithm.EvolvePopulation(population);
                fittest = population.GetFittest();
                if (lastFittest.GetFitnessByTime() != fittest.GetFitnessByTime() || lastFittest.GetFitnessByDistance() != fittest.GetFitnessByDistance())
                {
                    sameIterationCount = 1;
                }
                Console.WriteLine("{0} generation: {1:0.00} {2:0.00}", generation, fittest.GetFitnessByTime(), fittest.GetFitnessByDistance());
                sameIterationCount++;
                generation++;
            }
            stopwatch.Stop();
            Console.WriteLine("\nTime taken: {0:0.00} s", stopwatch.Elapsed.TotalSeconds);

            for (int i = 1; i <= fittest.TravelersCount; i++)
            {
                InOutUtils.PrintTravelersPath(foutWithoutExtension + i + foutExtension, 10, fittest, i);
            }

            Console.ReadKey();
        }
    }
}
