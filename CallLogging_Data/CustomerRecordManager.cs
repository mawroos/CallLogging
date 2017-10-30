using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallLogging_Data
{
    public class CustomerRecordManager
    {
        public CustomerRecordManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<Customer> Get()
        {
            return GetCustomers();
        }

        
        private static List<Customer> GetCustomers()
        {
            List<Customer> ret;
            using (var context = new CallLoggingEntities())
            {
                var Customers = context.Customers
                               .Where(c => c.CUS_CompanyId == 3)
                               .OrderBy(c => c.CUS_Name);
                               

                               
                ret = Customers.ToList();

            }
            return ret;
        }

        public Customer Get(int CustomerId)
        {

            Customer Customer;
            using (var context = new CallLoggingEntities())
            {
                Customer = (from c in context.Customers
                                where (c.CUS_UID == CustomerId)
                                select c).FirstOrDefault();
                           
                               ;
                
                              
            }
            return Customer;

            
        }

        
        public bool Validate(Customer entity)
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