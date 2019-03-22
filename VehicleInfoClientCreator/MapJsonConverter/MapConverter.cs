using Com.Zegtank.MapFileOperation.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace MapJsonConverter
{
    public class XmlToJsonMapConverter : IMapConverter
    {
        public IList< MapCoordinateModel> MapReader(string xmlPath)
        {
            var mapCoordinateModel = new List<MapCoordinateModel>();
            var xmlString = MapReaderToString(xmlPath);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            var map = xmlDocument.DocumentElement;

            foreach (var line in map.ChildNodes)
            {
                MapCoordinateModel model = new MapCoordinateModel();
                mapCoordinateModel.Add(model);
                model.id = int.Parse((line as XmlElement).GetAttribute("Id"));
                var type = (line as XmlElement).GetAttribute("type");
                switch (type)
                {
                    case "aisle":
                        foreach (var child in (line as XmlNode).ChildNodes)
                        {
                            MapAreaModel mapAreaModel = new MapAreaModel();
                            model.areas.Add(mapAreaModel);
                            mapAreaModel.id = int.Parse((child as XmlElement).GetAttribute("Id"));
                            var group = (child as XmlElement).GetAttribute("group");
                            switch (group)
                            {
                                case "1":
                                    mapAreaModel.type = "aisle";
                                    mapAreaModel.count = 1;
                                    break;
                                case "7":
                                    mapAreaModel.type = "item";
                                    mapAreaModel.count = 7;
                                    break;
                            }
                            mapAreaModel.points= GetPointModels((child as XmlElement).InnerText);
                        }
                        break;
                    case "shelve":
                        foreach (var child in (line as XmlNode).ChildNodes)
                        {
                            MapAreaModel mapAreaModel = new MapAreaModel();
                            mapAreaModel.id = int.Parse((child as XmlElement).GetAttribute("Id"));
                            var group = (child as XmlElement).GetAttribute("group");
                            switch (group)
                            {
                                case "1":
                                    mapAreaModel.type = "aisle";
                                    mapAreaModel.count = 1;
                                    break;
                                case "7":
                                    mapAreaModel.type = "shelve";
                                    mapAreaModel.count = 7;
                                    break;
                            }
                            mapAreaModel.points = GetPointModels((child as XmlElement).InnerText);
                        }
                        break;
                }
            }
            return mapCoordinateModel;
        }

        private PointModel[] GetPointModels(string xmlContent)
        {
            PointModel[] models = new PointModel[4];
            Debug.Assert(xmlContent != null);
            if (string.IsNullOrEmpty(xmlContent))
                throw new ArgumentNullException();
           
            string[] modelsString = xmlContent.Split(new char[] { ';' });
            if (modelsString.Length != 4)
            {
                return null;
            }
            for (int i = 0; i < modelsString.Length; i++)
            {
                var childs = modelsString[i].Split(new char[] { ',' });
                if (childs.Length != 2)
                {
                    continue;
                }
                PointModel point = new PointModel(double.Parse(childs[0]), double.Parse(childs[1]));
                models[i] = point;
            }
           

            return models;
        }

        public void MapWriter(IList<MapCoordinateModel> models, string jsonPath)
        {
            Debug.Assert(models != null);
            if (models == null)
                throw new ArgumentNullException();

            Debug.Assert(!string.IsNullOrEmpty(jsonPath));
            if (string.IsNullOrEmpty(jsonPath))
                throw new ArgumentNullException();

            if (!File.Exists(jsonPath))
            {
                using (File.Create(jsonPath))
                {

                }
            }
            using (StreamWriter streamWriter = new StreamWriter(jsonPath))
            {
                var json = JsonConvert.SerializeObject(models);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

        }

        public string MapReaderToString(string path)
        {
            Debug.Assert(!string.IsNullOrEmpty(path));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException();

            if (!File.Exists(path))
                throw new FileNotFoundException();
            using (StreamReader streamReader = new StreamReader(path))
            {
               var xml= streamReader.ReadToEnd();
                return xml;
            }

        }
    }
}
