using System.Collections.Generic;

namespace Com.Zegtank.MapFileOperation.Model
{
    public class MapCoordinateModel
    {
        private int _id = 0;

        /// <summary>
        /// 列序号
        /// </summary>
        public int id { get => _id; set => _id = value; }
        public List<MapAreaModel> areas { get; set; } = new List<MapAreaModel>();
    }
}
