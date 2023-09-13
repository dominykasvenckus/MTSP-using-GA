namespace MTSP
{
    class Population
    {
        public int Size { get; private set; }
        public List<Chromosome> Chromosomes { get; private set; }

        public Population(int size, Location startLocation, Location endLocation, List<Location> locations, int travelersCount,
                          double timePerLocation, double timePerKilometer, double timeBeforeBreak, double breakTime)
        {
            Size = size;
            Chromosomes = new List<Chromosome>();
            for (int i = 0; i < size; i++)
            {
                Chromosomes.Add(new Chromosome(startLocation, endLocation, locations, travelersCount,
                                               timePerLocation, timePerKilometer, timeBeforeBreak, breakTime));
            }
        }

        public Population(int size)
        {
            Size = size;
            Chromosomes = new List<Chromosome>();
        }

        public void AddChromosome(Chromosome chromosome)
        {
            Chromosomes.Add(chromosome);
        }

        public Chromosome GetFittest()
        {
            List<Chromosome> sortedList = Chromosomes.OrderBy(x => x.GetFitnessByTime())
                                                     .ThenBy(x => x.GetFitnessByDistance())
                                                     .ToList();
            return sortedList[0];
        }

        public Chromosome GetFittest(int index)
        {
            List<Chromosome> sortedList = Chromosomes.OrderBy(x => x.GetFitnessByTime())
                                                     .ThenBy(x => x.GetFitnessByDistance())
                                                     .ToList();
            return sortedList[index];
        }
    }
}
