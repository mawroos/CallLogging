using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallLogging_Data
{
    public class MessageRecordManager
    {
        public MessageRecordManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<Message> Get()
        {
            return GetMessages();
        }

        public List<Message> Get(Message entity)
        {
            List<Message> ret = new List<Message>();

            // TODO: Add your own data access method here
            //  ret = CreateMockData();
            ret = GetMessages();
            // Do any searching
            if (!string.IsNullOrEmpty(entity.MES_Description))
            {
                //split text into separate keywords
                string[] searchText = entity.MES_Description.Split(' ');
                foreach (string searchKeyword in searchText)
                {
                    // try to convert the keyword to text, maybe its log number?
                    int MessNo;
                    if(Int32.TryParse(searchKeyword, out MessNo))
                    {
                        var LogNumberFound = ret.FindAll(m => m.MES_UID == MessNo);
                        if (LogNumberFound != null)
                        { 
                            ret = LogNumberFound;
                            continue;
                        }

                    }
                    
                    // do keyword search.
                    ret = ret.FindAll(
                  p => p.MES_Description.ToLower().
                  Contains(searchKeyword.ToLower()));
                }
           
            }



            return ret;
        }

        private static List<Message> GetMessages()
        {
            List<Message> ret;
            using (var context = new CallLoggingEntities())
            {
                var Messages = context.Messages
                               .Where(m => m.MES_CompanyId == 3)
                               .Where(m => m.MES_Status != "Deleted")
                               .OrderBy(m => m.MES_Status)
                               .ThenByDescending(m => m.MES_MesDateTime);

                               
                ret = Messages.Take(300).ToList();

            }
            return ret;
        }

        public Message Get(int MessageId)
        {
            List<Message> ret =
              new List<Message>();
            Message entity = null;
            
            using (var context = new CallLoggingEntities())
            {
                var Messages = from m in context.Messages
                               where (m.MES_UID == MessageId)
                           select m
                           
                               ;
                ret = Messages.ToList();
                              
            }
            // TODO: Add data access method here
            //ret = CreateMockData();

            // Find the specific product
            entity = ret.FirstOrDefault();

            return entity;
        }

        public bool Update(Message entity)
        {
            bool ret = false;

            ret = Validate(entity);
            Message MessageEntity;
            if (ret)
            {
                /// TODO: Create UPDATE code here
                using (var context = new CallLoggingEntities())
                {
                    var Messages = from m in context.Messages

                                   select m

                                   ;
                    MessageEntity = Messages.Where(m => m.MES_UID == entity.MES_UID).SingleOrDefault();

                }
                if (MessageEntity != null)
                {
                    MessageEntity.MES_Description = entity.MES_Description;
                    MessageEntity.MES_MesDateTime = DateTime.Today;
                    MessageEntity.MES_Urgent = entity.MES_Urgent;


                }
                using (var context = new CallLoggingEntities())
                {
                    context.Entry(MessageEntity).State = System.Data.Entity.EntityState.Modified;
                    ret = (context.SaveChanges() > 0 ? true : false);


                }


                
            }
            return ret;
        }

        public bool Delete(Message entity)
        {
            /// TODO: Create DELETE code here
            return true;
        }

        public bool Validate(Message entity)
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

        public bool Insert(Message entity)
        {
            bool ret = false;

            ret = Validate(entity);

            if (ret)
            {
                using (var context = new CallLoggingEntities())
                {
                    //find the latest callLogNumber
                    int LogNo;
                    using (var context2 = new CallLoggingEntities())
                    {
                        LogNo = context2.Messages.OrderByDescending(m => m.MES_UID).First().MES_UID+1;
                    }
                    if (LogNo !=0)
                    { 
                    entity.MES_UID = LogNo;
                
                        var result = context.Messages.Add(entity);
                        
                    ret = (result.MES_UID == entity.MES_UID ? true : false);
                        context.SaveChanges();
                    }
                    
                }
               
            }

            return ret;
        }

        //    protected List<MessageRecord> CreateMockData()
        //    {
        //        List<MessageRecord> ret = new List<MessageRecord>();

        //        ret.Add(new MessageRecord()
        //        {
        //            ProductId = 1,
        //            ProductName = "Extending Bootstrap with CSS, JavaScript and jQuery",
        //            IntroductionDate = Convert.ToDateTime("6/11/2015"),
        //            Url = "http://bit.ly/1SNzc0i",
        //            Price = Convert.ToDecimal(29.00)
        //        });

        //        ret.Add(new MessageRecord()
        //        {
        //            ProductId = 2,
        //            ProductName = "Build your own Bootstrap Business Application Template in MVC",
        //            IntroductionDate = Convert.ToDateTime("1/29/2015"),
        //            Url = "http://bit.ly/1I8ZqZg",
        //            Price = Convert.ToDecimal(29.00)
        //        });

        //        ret.Add(new MessageRecord()
        //        {
        //            ProductId = 3,
        //            ProductName = "Building Mobile Web Sites Using Web Forms, Bootstrap, and HTML5",
        //            IntroductionDate = Convert.ToDateTime("8/28/2014"),
        //            Url = "http://bit.ly/1J2dcrj",
        //            Price = Convert.ToDecimal(29.00)
        //        });

        //        ret.Add(new MessageRecord()
        //        {
        //            ProductId = 4,
        //            ProductName = "How to Start and Run A Consulting Business",
        //            IntroductionDate = Convert.ToDateTime("9/12/2013"),
        //            Url = "http://bit.ly/1L8kOwd",
        //            Price = Convert.ToDecimal(29.00)
        //        });

        //        ret.Add(new MessageRecord()
        //        {
        //            ProductId = 5,
        //            ProductName = "The Many Approaches to XML Processing in .NET Applications",
        //            IntroductionDate = Convert.ToDateTime("7/22/2013"),
        //            Url = "http://bit.ly/1DBfUqd",
        //            Price = Convert.ToDecimal(29.00)
        //        });

        //        ret.Add(new MessageRecord()
        //        {
        //            ProductId = 6,
        //            ProductName = "WPF for the Business Programmer",
        //            IntroductionDate = Convert.ToDateTime("6/12/2009"),
        //            Url = "http://bit.ly/1UF858z",
        //            Price = Convert.ToDecimal(29.00)
        //        });

        //        ret.Add(new MessageRecord()
        //        {
        //            ProductId = 7,
        //            ProductName = "WPF for the Visual Basic Programmer - Part 1",
        //            IntroductionDate = Convert.ToDateTime("12/16/2014"),
        //            Url = "http://bit.ly/1uFxS7C",
        //            Price = Convert.ToDecimal(29.00)
        //        });

        //        ret.Add(new MessageRecord()
        //        {
        //            ProductId = 8,
        //            ProductName = "WPF for the Visual Basic Programmer - Part 2",
        //            IntroductionDate = Convert.ToDateTime("2/18/2014"),
        //            Url = "http://bit.ly/1MjQ9NG",
        //            Price = Convert.ToDecimal(29.00)
        //        });

        //        return ret;
        //    }
        //}
    }
}