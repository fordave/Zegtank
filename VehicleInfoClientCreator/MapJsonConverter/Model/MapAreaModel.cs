using System.Drawing;

namespace Com.Zegtank.MapFileOperation.Model
{
    public class MapAreaModel
    {
        private int _id = 0;
        private string _type;
        private int _count = 1;
        private PointModel[] _points = new PointModel[4];
        /// <summary>
        /// 行序号
        /// </summary>
        public int id { get => _id; set => _id = value; }
        public string type { get => _type; set => _type = value; }
        /// <summary>
        /// 均分行数
        /// </summary>
        public int count { get => _count; set => _count = value; }
        public PointModel[] points { get => _points; set => _points = value; }
    }
}
