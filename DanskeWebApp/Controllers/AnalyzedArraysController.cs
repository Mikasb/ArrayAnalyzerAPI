using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DanskeWebApp.Database;
using DanskeWebApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DanskeWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzedArraysController : ControllerBase
    {
        private readonly ArrayAnalysisDbContext _context;

        public AnalyzedArraysController(ArrayAnalysisDbContext context)
        {
            _context = context;
        }

        //GET: api/AnalyzedArrays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnalyzedArrays>>> GetAnalyzedArrays()
        {
            return await _context.AnalyzedArrays.ToListAsync();
        }

        //GET: api/AnalyzedArrays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnalyzedArrays>> GetAnalyzedArrays(int id)
        {
            var analyzedArray =  await _context.AnalyzedArrays.FindAsync(id);
            if (analyzedArray == null)
            {
                return NotFound();
            }

            return analyzedArray;
        }

        //PUT: api/AlyzedArrays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnalyzedArray(int id, AnalyzedArrays analyzedArray)
        {
            if (id != analyzedArray.Id)
            {
                return BadRequest("Id does not exist.");
            }

            analyzedArray = ProcessArray(analyzedArray);

            _context.Entry(analyzedArray).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {

                if (!AnalyzedArraysExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/AnalyzedArrays

        [HttpPost]
        public async Task<ActionResult<AnalyzedArrays>> PostAnalyzedArrays(AnalyzedArrays analyzedArray)
        {

            if (_context.AnalyzedArrays.Any(arr => arr.ArrayComposition.Equals(analyzedArray.ArrayComposition)))
            {
                return Content("Array already exists.");
            }

            analyzedArray = ProcessArray(analyzedArray);

            _context.AnalyzedArrays.Add(analyzedArray);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnalyzedArrays", new {id = analyzedArray.Id}, analyzedArray);
        }
        private bool AnalyzedArraysExists(int id)
        {
            return _context.AnalyzedArrays.Any(e => e.Id == id);
        }

        /**
         * Process array method processes the array to display optimal path,
         * reachable boolean field to false(0) or true(1).
         */
        public AnalyzedArrays ProcessArray(AnalyzedArrays analyzedArray)
        {
            //converts "ArrayComposition" field, which is in a form of e.g. 1,3,2,5 to a List of integers.
            List<int> stringToList = new List<int>(analyzedArray.ArrayComposition.Split(",").Select(x => int.Parse(x)));

            //converting the List to number array to be passed for processing.
            int[] numArray = stringToList.ToArray();

            string optimalPath = "";
            if (IsReachable(numArray))
            {
                foreach (var optimalJump in FindPath(numArray))
                {
                    optimalPath += optimalJump.Value + " -> ";
                }
                optimalPath = optimalPath.Remove(optimalPath.LastIndexOf("-"), 2);


                analyzedArray.IsReachable = 1;
                analyzedArray.OptimalPath = optimalPath.Trim();
            }
            else
            {
                analyzedArray.IsReachable = 0;
                analyzedArray.OptimalPath = "n/a";
            }

            return analyzedArray;
        }




        // The logic to identify if array's end is reachable.
        public static bool IsReachable(int[] numArray)
        {
            int acceptableIndex = numArray.Length - 1;
            for (int i = numArray.Length - 1; i >= 0; i--)
            {
                if ((i + numArray[i]) >= acceptableIndex)
                {
                    acceptableIndex = i;
                }
            }

            return acceptableIndex == 0 ? true : false;
        }

        /**
         * FindPath method is called if the end of an array is reachable
         * to determine the optimal path.
         * @return - Dictionary, where key is the index of a start/move point and
         * value is the max jump capacity held by the index.
         */
        public static Dictionary<int, int> FindPath(int[] numArray)
        {
            var path = new Dictionary<int, int>();

            int maxReachableIndex = 0;

            for (int i = 0; i < numArray.Length - 1; i++)
            {
                if ((i + numArray[i]) > maxReachableIndex)
                {
                    path.Add(i, numArray[i]);
                    maxReachableIndex = i + numArray[i];
                }

                if (maxReachableIndex >= (numArray.Length - 1))
                {
                    path.Add(numArray.Length - 1, numArray[numArray.Length - 1]);
                    return path;
                }
            }
            return path;
        }
    }
}
