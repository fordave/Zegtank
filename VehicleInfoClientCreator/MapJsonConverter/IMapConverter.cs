using Com.Zegtank.MapFileOperation.Model;
using System.Collections.Generic;

namespace MapJsonConverter
{
    public interface IMapConverter
    {
        string MapReaderToString(string path);
        IList<MapCoordinateModel> MapReader(string xmlPath);
        void MapWriter(IList<MapCoordinateModel> models, string jsonpath);
    }
}
