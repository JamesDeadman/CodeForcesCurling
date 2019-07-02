namespace CurlingSim
{
    /// <summary>
    /// Represents a disk in the curling game (otherwise known as a rock)
    /// </summary>
    public class Disk
    {
        public string Name { get; private set; }
        public int XLocation { get; private set; }
        public double YLocation { get; set; }
        public int Radius { get; private set; }

        public Disk(string name, int xLocation, double yLocation, int radius)
        {
            Name = name;
            XLocation = xLocation;
            YLocation = yLocation;
            Radius = radius;
        }

        public override string ToString()
        {
            return $"{Name} x:{XLocation}, y:{YLocation}, r:{Radius}";
        }
    }
}
