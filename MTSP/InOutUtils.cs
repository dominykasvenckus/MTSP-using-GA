using OfficeOpenXml;
using System.Text;

namespace MTSP
{
    static class InOutUtils
    {
        public static List<Location> ReadData(string fileName)
        {
            List<Location> locations = new List<Location>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(fileName)))
            {
                var myWorksheet = xlPackage.Workbook.Worksheets.First();
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;
                for (int rowNum = 6; rowNum <= totalRows; rowNum++)
                {
                    var row = myWorksheet.Cells[rowNum, 2, rowNum, totalColumns].Select(c => c.Value == null ? "" : c.Value.ToString());
                    string line = string.Join('#', row);
                    string[] values = line.Split('#');
                    string name = values[0];
                    string id = values[1];
                    double x = double.Parse(values[2]);
                    double y = double.Parse(values[3]);
                    Location location = new Location(name, id, x, y);
                    locations.Add(location);
                }
            }
            return locations;
        }

        public static void PrintTravelersPath(string fileName, int locationsPerLine, Chromosome chromosome, int travelersNumber)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                List<Location> travelersLocations = new List<Location>();
                travelersLocations.Add(chromosome.StartLocation);
                for (int i = 0; i < chromosome.Locations.Count; i++)
                {
                    if (chromosome.Travelers[i] == travelersNumber)
                    {
                        travelersLocations.Add(chromosome.Locations[i]);
                    }
                }
                travelersLocations.Add(chromosome.EndLocation);
                for (int i = 0; i < travelersLocations.Count; i++)
                {
                    writer.Write(travelersLocations[i].Id);
                    if (i != travelersLocations.Count - 1)
                    {
                        writer.Write("->");
                    }
                    if (i % locationsPerLine == locationsPerLine - 1)
                    {
                        writer.WriteLine();
                    }
                }
            }
        }
    }
}
