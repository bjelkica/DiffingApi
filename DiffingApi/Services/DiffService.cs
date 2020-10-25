using DiffingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DiffingApi.Services
{
    public class DiffService
    {
        // Function for getting left and right endpoint data from a database by id 
        public DiffData GetDiffDataById(int id)
        {
            using (var context = new DiffContext())
            {
                return context.Diffs.Where(b => b.Id == id).FirstOrDefault();
            }
        }

        // Function for putting a left endpoint data in a database
        public void AddLeftData (int id, InputData left)
        {
            using (var context = new DiffContext())
            {
                try
                {
                    var diff = context.Diffs.Where(b => b.Id == id).FirstOrDefault();
                    // If there doesn't exist any diff with given id then we create a new diff in a database
                    if (diff == null)
                    {
                        DiffData diffData = new DiffData();
                        diffData.Id = id;
                        diffData.LeftData = left.Data;
                        context.Diffs.Add(diffData);
                        context.SaveChanges();
                    }
                    // Otherwise we just update left data of an existing diff
                    else
                    {
                        diff.LeftData = left.Data;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        // Function for putting a right endpoint data in a database
        public void AddRightData(int id, InputData right)
        {
            using (var context = new DiffContext())
            {
                try
                {
                    var diff = context.Diffs.Where(b => b.Id == id).FirstOrDefault();
                    // If there doesn't exist any diff with given id then we create a new diff in a database
                    if (diff == null)
                    {
                        DiffData diffData = new DiffData();
                        diffData.Id = id;
                        diffData.RightData = right.Data;
                        context.Diffs.Add(diffData);
                        context.SaveChanges();
                    }
                    // Otherwise we just update right data of an existing diff
                    else
                    {
                        diff.RightData = right.Data;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
