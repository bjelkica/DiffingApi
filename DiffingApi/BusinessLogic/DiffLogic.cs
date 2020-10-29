using DiffingApi.BusinessLogic;
using DiffingApi.DAL;
using DiffingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DiffingApi.BusinessLogic
{
    public class DiffLogic
    {
        #region Database Logic
        private readonly DiffDAL diffDAL = new DiffDAL();
        // Function for getting left and right endpoint data from a database by id 
        public DiffData GetDiffDataById(int id)
        {
            return diffDAL.GetDiffDataById(id);
        }

        // Function for putting a data into a database. If the parameter side equals left, then we put left endpoint data
        // otherwise we put right endpoint data
        public void AddData(int id, string side, InputData data)
        {
            {
                var diff = diffDAL.GetDiffDataById(id);
                string decodedData = Base64String.DecodeBase64String(data.Data);

                // If there doesn't exist any diff with given id then we create a new diff in a database
                if (diff == null)
                {
                    DiffData newDiff = new DiffData();
                    newDiff.Id = id;
                    newDiff = UpdateDiffData(side, decodedData, newDiff);
                    diffDAL.AddData(newDiff);
                }
                // Otherwise we just update data of an existing diff
                else
                {
                    diff = UpdateDiffData(side, decodedData, diff);
                    diffDAL.UpdateDiff(diff);
                }
            }
        }
        #endregion

        #region Other Logic
        // Function for updating a diff
        public DiffData UpdateDiffData(string side, string data,  DiffData diff)
        {
            if (side == "left")
            {
                diff.LeftData = data;
            }
            else
            {
                diff.RightData = data;
            }
            return diff;
        }

        // Function that checks if diff is empty or there is left/right data missing
        public bool IsDiffMissingData (DiffData diff)
        {
            if (diff == null || diff.LeftData == null || diff.RightData == null)
            {
                return true;
            }
            return false;
        }

        // Function for diff-ing data
        public DiffResponse Diffing(DiffData diff)
        {
            DiffResponse response = new DiffResponse();
            List<Diff> diffs = new List<Diff>();
            string leftDiff = diff.LeftData;
            string rightDiff = diff.RightData;
            int leftLen = leftDiff.Length;
            int rightLen = rightDiff.Length;

            // If the strings aren't of the same length we aren't interested in differences
            // so we first compare their lengths
            if (leftLen != rightLen)
            {
                response.DiffResultType = "SizeDoNotMatch";
            }
            else
            {
                // If the strings are of the same length we compare them. 
                // If they are not equal we locate and return differences.
                int prevDiff = -1; // Index of the last diff object in the list diffs
                int lastDiff = -2; // Index of the last string chars that were recognized as different 

                for (int i = 0; i < leftLen; i++)
                {
                    if (leftDiff[i] != rightDiff[i])
                    {
                        // If previous chars of both strings were equal this diff represents a start of a new different substring
                        if (lastDiff != i - 1)
                        {
                            prevDiff += 1;
                            Diff d = new Diff
                            {
                                Offset = i,
                                Length = 1
                            };
                            diffs.Add(d);
                        }
                        // Otherwise we just increase a length of previous diff
                        else
                        {
                            diffs[prevDiff].Length += 1;
                        }
                        lastDiff = i;
                    }
                }
                // We check if we found any differences
                if (prevDiff == -1)
                {
                    response.DiffResultType = "Equals";
                }
                else
                {
                    response.DiffResultType = "ContentDoNotMatch";
                    response.Diffs = diffs;
                }
            }
            return response;
        }
        #endregion
    }
}
