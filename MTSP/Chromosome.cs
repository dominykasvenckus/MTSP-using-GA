using static System.Math;

namespace MTSP
{
    class Chromosome
    {
        public Location StartLocation { get; private set; }
        public Location EndLocation { get; private set; }
        public List<Location> Locations { get; private set; }
        public int TravelersCount { get; private set; }
        public List<int> Travelers { get; private set; }
        public double TimePerLocation { get; private set; }
        public double TimePerKilometer { get; private set; }
        public double TimeBeforeBreak { get; private set; }
        public double BreakTime { get; private set; }

        private static readonly Random _rand = new Random();

        public Chromosome(Location startLocation, Location endLocation, List<Location> locations, int travelersCount,
                          double timePerLocation, double timePerKilometer, double timeBeforeBreak, double breakTime)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
            Shuffle(locations);
            Locations = locations;
            TravelersCount = travelersCount;
            Travelers = new List<int>();
            for (int i = 0; i < locations.Count; i++)
            {
                Travelers.Add(_rand.Next(1, travelersCount + 1));
            }
            TimePerLocation = timePerLocation;
            TimePerKilometer = timePerKilometer;
            TimeBeforeBreak = timeBeforeBreak;
            BreakTime = breakTime;
        }

        public Chromosome(Location startLocation, Location endLocation, List<Location> locations, int travelersCount, List<int> travelers,
                          double timePerLocation, double timePerKilometer, double timeBeforeBreak, double breakTime)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
            Locations = locations;
            TravelersCount = travelersCount;
            Travelers = travelers;
            TimePerLocation = timePerLocation;
            TimePerKilometer = timePerKilometer;
            TimeBeforeBreak = timeBeforeBreak;
            BreakTime = breakTime;
        }

        public override string ToString()
        {
            string line = "";
            for (int i = 1; i <= TravelersCount; i++)
            {
                line += i + " traveler:\n";
                List<Location> travelersLocations = new List<Location>();
                travelersLocations.Add(StartLocation);
                for (int j = 0; j < Locations.Count; j++)
                {
                    if (Travelers[j] == i)
                    {
                        travelersLocations.Add(Locations[j]);
                    }
                }
                travelersLocations.Add(EndLocation);
                for (int j = 0; j < travelersLocations.Count; j++)
                {
                    line += travelersLocations[j].Id;
                    if (j != travelersLocations.Count - 1)
                    {
                        line += "->";
                    }
                }
                if (i != TravelersCount)
                {
                    line += "\n\n";
                }
            }
            return line;
        }

        public double GetFitnessByTime()
        {
            double maxTripTime = 0;
            for (int i = 1; i <= TravelersCount; i++)
            {
                double distance = 0;
                double tripTime = 0;
                List<Location> travelersLocations = new List<Location>();
                travelersLocations.Add(StartLocation);
                for (int j = 0; j < Locations.Count; j++)
                {
                    if (Travelers[j] == i)
                    {
                        travelersLocations.Add(Locations[j]);
                    }
                }
                travelersLocations.Add(EndLocation);
                for (int j = 0; j < travelersLocations.Count - 1; j++)
                {
                    distance += travelersLocations[j].CalculateDistance(travelersLocations[j + 1]);
                    tripTime += distance / 1000 * TimePerKilometer + TimePerLocation;
                }
                int breaksCount = (int)(tripTime / TimeBeforeBreak);
                tripTime += breaksCount * BreakTime;
                if (tripTime > maxTripTime)
                {
                    maxTripTime = tripTime;
                }
            }
            return maxTripTime;
        }

        public double GetFitnessByDistance()
        {
            double distance = 0;
            List<Location> locations = new List<Location>();
            locations.Add(StartLocation);
            locations.AddRange(Locations);
            locations.Add(EndLocation);
            for (int i = 0; i < locations.Count - 1; i++)
            {
                distance += locations[i].CalculateDistance(locations[i + 1]);
            }
            return Cbrt(distance);
        }

        private void Shuffle(List<Location> locations)
        {
            int n = locations.Count;
            while (n > 1)
            {
                int k = _rand.Next(n--);
                Location temp = locations[n];
                locations[n] = locations[k];
                locations[k] = temp;
            }
        }
    }
}
