namespace Com.Zegtank.MapFileOperation.Model
{
    public class PointModel
    {
        private double _x;
        private double _y;

        public PointModel()
        {
        }

        public PointModel(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double x { get => _x; set => _x = value; }
        public double y { get => _y; set => _y = value; }

      
    }
}
