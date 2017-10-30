using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallLogging_Data
{
    public class ProblemRecordManager
    {
        public ProblemRecordManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<Problem> Get(int CompanyId)
        {
            return GetProblems(CompanyId);
        }

        
        private static List<Problem> GetProblems(int CompanyId)
        {
            List<Problem> ret;
            
            using (var context = new CallLoggingEntities())
            {
                var Problems = context.Problems
                               .Where(p => p.PRO_CompanyId == CompanyId);
                               
                ret = Problems.ToList();

            }
            return ret;
        }

                    
        

        
        public bool Validate(Problem entity)
        {
            ValidationErrors.Clear();
            // Extra Business Validation if required
            //if (!string.IsNullOrEmpty(entity.ProductName))
            //{
            //    if (entity.ProductName.ToLower() ==
            //        entity.ProductName)
            //    {
            //        ValidationErrors.Add(new
            //          KeyValuePair<string, string>("ProductName",
            //          "Product must not be all lower case."));
            //    }
            //}

            return (ValidationErrors.Count == 0);
        }

        
    }
}