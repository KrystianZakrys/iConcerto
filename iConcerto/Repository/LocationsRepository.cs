using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iConcerto.Models;

namespace iConcerto.Repository
{
    public interface ILocationsRepository : IDisposable
    {
        List<Locations> GetLocations();
        Locations GetLocation(int id);
    }
    public class LocationsRepository : ILocationsRepository
    {
       

        public Locations GetLocation(int id)
        {
            Locations location = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var query = db.Locations.Where(l => l.ID == id);
                    if(query.Any())
                        location = query.First();
            }
            return location;
        }

        List<Locations> ILocationsRepository.GetLocations()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Locations> locations = db.Locations.ToList();
                return locations != null ? locations :  new List<Locations>();
            }
        }


        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}