using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelectedHotelsModel;
using NGeoNames;
using NGeoNames.Entities;
using GeoName = SelectedHotelsModel.GeoName;

namespace ImportGeoNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SelectedHotelsEntities db = new SelectedHotelsEntities())
            {
                var data = GeoFileReader.ReadExtendedGeoNames(@"C:\Users\Alex\Downloads\GB\GB.txt").ToArray();
                int count = data.Count();
                int i = 0;
                foreach (ExtendedGeoName extendedGeoName in data)
                {
                    Console.WriteLine("{0:##.###}", 1.0 * i++ / count * 100.0);
                    GeoName geoName = db.GeoNames.Find(extendedGeoName.Id);
                    if (geoName == null)
                    {
                        geoName = new GeoName
                        {
                            Id = extendedGeoName.Id,
                            Name = extendedGeoName.Name,
                            CountryCode = extendedGeoName.CountryCode,
                            FeatureClass = extendedGeoName.FeatureClass,
                            FeatureCode = extendedGeoName.FeatureCode,
                            ModificationDate = extendedGeoName.ModificationDate,
                            Population = extendedGeoName.Population,
                            Location = DbGeography.FromText(String.Format("POINT({0} {1})", extendedGeoName.Longitude, extendedGeoName.Latitude))
                        };
                        db.GeoNames.Add(geoName);
                    }
                    else
                    {
                        geoName.Name = extendedGeoName.Name;
                        geoName.CountryCode = extendedGeoName.CountryCode;
                        geoName.FeatureClass = extendedGeoName.FeatureClass;
                        geoName.FeatureCode = extendedGeoName.FeatureCode;
                        geoName.ModificationDate = extendedGeoName.ModificationDate;
                        geoName.Population = extendedGeoName.Population;
                        geoName.Location =
                            DbGeography.FromText(String.Format("POINT({0} {1})", extendedGeoName.Longitude,
                                extendedGeoName.Latitude));
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
