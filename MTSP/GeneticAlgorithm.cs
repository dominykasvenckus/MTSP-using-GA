namespace MTSP
{
    class GeneticAlgorithm
    {
        public double MutationRate { get; private set; }
        public int EliteSize { get; private set; }
        public int TournamentSize { get; private set; }

        private static readonly Random _rand = new Random();

        public GeneticAlgorithm(double mutationRate, int eliteSize, int tournamentSize)
        {
            MutationRate = mutationRate;
            EliteSize = eliteSize;
            TournamentSize = tournamentSize;
        }

        public Population EvolvePopulation(Population population)
        {
            Population newPopulation = new Population(population.Size);
            for (int i = 0; i < EliteSize; i++)
            {
                newPopulation.Chromosomes.Add(population.GetFittest(i));
            }
            for (int i = EliteSize; i < population.Size; i++)
            {
                Chromosome chromosome1 = TournamentSelection(population);
                Chromosome chromosome2 = TournamentSelection(population);
                Chromosome newChromosome = CrossoverAndMutate(chromosome1, chromosome2);
                newPopulation.AddChromosome(newChromosome);
            }
            return newPopulation;
        }

        private Chromosome CrossoverAndMutate(Chromosome chromosome1, Chromosome chromosome2)
        {
            int cutPoint = _rand.Next(1, chromosome1.Locations.Count);
            List<Location> locations = chromosome1.Locations.GetRange(0, cutPoint);
            List<int> travelers = chromosome1.Travelers.GetRange(0, cutPoint);
            for (int i = 0; i < chromosome2.Locations.Count; i++)
            {
                for (int j = cutPoint; j < chromosome1.Locations.Count; j++)
                {
                    if (chromosome2.Locations[i].Id.Equals(chromosome1.Locations[j].Id))
                    {
                        locations.Add(chromosome2.Locations[i]);
                        travelers.Add(chromosome2.Travelers[i]);
                    }
                }
            }
            for (int i = 0; i < locations.Count; i++)
            {
                if (_rand.NextDouble() < MutationRate)
                {
                    int index = _rand.Next(0, locations.Count);
                    Location temp1 = locations[i];
                    int temp2 = travelers[i];
                    locations[i] = locations[index];
                    travelers[i] = travelers[index];
                    locations[index] = temp1;
                    travelers[index] = temp2;
                }
            }
            Location startLocation = chromosome1.StartLocation;
            Location endLocation = chromosome1.EndLocation;
            int travelersCount = chromosome1.TravelersCount;
            double timePerLocation = chromosome1.TimePerLocation;
            double timePerKilometer = chromosome1.TimePerKilometer;
            double timeBeforeBreak = chromosome1.TimeBeforeBreak;
            double breakTime = chromosome1.BreakTime;
            Chromosome newChromosome = new Chromosome(startLocation, endLocation, locations, travelersCount, travelers,
                                                      timePerLocation, timePerKilometer, timeBeforeBreak, breakTime);
            return newChromosome;
        }

        private Chromosome TournamentSelection(Population population)
        {
            Population tournament = new Population(TournamentSize);
            for (int i = 0; i < TournamentSize; i++)
            {
                tournament.AddChromosome(population.Chromosomes[_rand.Next(1, population.Size)]);
            }
            return tournament.GetFittest();
        }
    }
}
