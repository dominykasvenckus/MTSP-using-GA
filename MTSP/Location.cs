using static System.Math;

namespace MTSP
{
    class Location
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Location(string name, string id, double x, double y)
        {
            Name = name;
            Id = id;
            X = x;
            Y = y;
        }

        public double CalculateDistance(Location location)
        {
            const double p = PI / 180;
            var a = 0.5 - Cos((location.Y - Y) * p) / 2 + Cos(Y * p) * Cos(location.Y * p) * (1 - Cos((location.X - X) * p)) / 2;
            return 12742 * Asin(Sqrt(a));
        }
    }
}
