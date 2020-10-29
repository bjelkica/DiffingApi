using DiffingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiffingApi.DAL
{
    public class DiffDAL
    {
        public DiffData GetDiffDataById (int id)
        {
            using (var diffContext = new DiffContext())
            {
                try
                {
                    return diffContext.Diffs.Where(b => b.Id == id).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void AddData (DiffData diff)
        {
            using (var diffContext = new DiffContext())
            {
                try
                {
                    diffContext.Add(diff);
                    diffContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void UpdateDiff (DiffData newDiff)
        {
            using (var diffContext = new DiffContext())
            {
                try
                {
                    diffContext.Update(newDiff);
                    diffContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
