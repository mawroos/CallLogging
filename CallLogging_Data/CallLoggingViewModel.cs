using System;
using System.Collections.Generic;
using CallLogging_Common;
using System.Configuration;
using PagedList.Mvc;
using PagedList;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace CallLogging_Data
{
    public class CallLoggingViewModel : ViewModelBase
    {
        

        public CallLoggingViewModel()
          : base()
        {
            // Initialize other variables
            Messages = new List<Message>();
            
            Customers = new List<Customer>();
            SearchEntity = new Message();
            Entity = new Message();
            MessageCategories = new List<string>();
            MessageCategoriesDropDown = new List<SelectListItem>();
            MessageProblem = new List<string>();
            MessageProblemDropDown = new List<SelectListItem>();
        }

        public List<Message> Messages { get; set; }
        public List<Customer> Customers { get; set; }
        public List<string> MessageCategories { get; set; }
        public List<SelectListItem> MessageCategoriesDropDown { get; set; }
        public List<string> MessageProblem { get; set; }
        public List<SelectListItem> MessageProblemDropDown { get; set; }
        public List<SelectListItem> CustomersDropDown { get; set; }
        public IPagedList<Message> PagedMessages { get; set; }
        public Message SearchEntity { get; set; }
        public Message Entity { get; set; }
        public int CompanyId { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        protected override void Init()
        {
            Messages = new List<Message>();
            Customers = new List<Customer>();
            CustomersDropDown = new List<SelectListItem>();
            PagedMessages = new PagedList<Message>(Messages, 1, 10);
            SearchEntity = new Message();
            Entity = new Message();
            CompanyId = 1;
            
            base.Init();
        }

        public override void HandleRequest(int page,int pageSize)
        {
            //some code to get the correct CompanyId
            CompanyId = 3;
            // This is an example of adding on a new command
            switch (EventCommand.ToLower())
            {
                case "newcommand":
                    break;

            }

            base.HandleRequest(page,pageSize);
        }

        protected override void Add()
        {
            IsValid = true;
            Customers = GetCustomers();
            CustomersDropDown = GetCustomersDropDown(Customers);
            MessageCategories = GetProblemCategory("category", 3,string.Empty);
            MessageCategoriesDropDown = (from p in MessageCategories
                                         select new SelectListItem { Value = p, Text = p }).ToList();

            // Initialize Entity Object
            Entity = new Message();
            Entity.MES_MesDateTime = DateTime.Now;
            Entity.MES_CustCode = GetCustomerCode(Entity.MES_CustomerUID,Customers) ;
            Entity.MES_CompanyId = CompanyId;
            Entity.MES_Category = string.Empty;
            Entity.MES_Problem = string.Empty;
            Entity.MES_Priority = (short)Convert.ToInt32(
                ConfigurationManager.AppSettings["MessagePriority"]);
            Entity.MES_Description = string.Empty;
            Entity.MES_Response = string.Empty;
            Entity.MES_Status = MessageRecord.Status.InProgress.ToString();
            Entity.MES_TakenBy = "me";
            
            Entity.MES_WaitStatus = MessageRecord.WaitStatus.Analysis.ToString();
            base.Add();
        }

        public List<string> GetProblemCategory(string contentType, int CompanyId,string CategoryName)
        {
            List<string> Ret = new List<string>();
            ProblemRecordManager ProMgr =new ProblemRecordManager();
            List<Problem> Problems_List = ProMgr.Get(CompanyId);
            switch (contentType)
            { 
                case "category":
                    Ret = (from p in Problems_List
                           select p.PRO_category).Distinct().ToList();
         
                    
                    break;
                case "problem":
                    Ret = (from p in Problems_List
                           where (p.PRO_category==CategoryName && !string.IsNullOrEmpty(CategoryName))
                           select p.PRO_Problem).Distinct().ToList();
                    break;
            }
            return Ret;
        }

        private string GetCustomerCode(int? mES_CustomerUID, List<Customer> customers)
        {
            string _customer_Code=string.Empty;
            int _customer_index;
            if (mES_CustomerUID != null)
            { 
                _customer_index = customers.FindIndex(c => c.CUS_UID == mES_CustomerUID);
                _customer_Code = customers[_customer_index].CUS_CustCode;
            }
            return _customer_Code;
        }

        protected override void Edit()
        {
            MessageRecordManager mgr =
             new MessageRecordManager();
            Customers = GetCustomers();
            CustomersDropDown=GetCustomersDropDown(Customers);
            MessageCategories = GetProblemCategory("category", 3, string.Empty);
            MessageCategoriesDropDown = (from p in MessageCategories
                                         select new SelectListItem { Value = p, Text = p }).ToList();
            // Get Product Data
            Entity = mgr.Get(
              Convert.ToInt32(EventArgument));

            base.Edit();
        }

        private List<SelectListItem> GetCustomersDropDown(List<Customer> CustomersList)
        {
            List<SelectListItem> CustomersDropDownList;
            CustomersDropDownList = new List<SelectListItem>();
            foreach (var customer in CustomersList)
                CustomersDropDownList.Add(new SelectListItem { Value = customer.CUS_UID.ToString(), Text = customer.CUS_Name });
            return CustomersDropDownList;
        }

        private List<Customer> GetCustomers()
        {
            CustomerRecordManager CusMgr =
                         new CustomerRecordManager();
            var Customers_List = CusMgr.Get();
            return Customers_List;
        }

        protected override void Display()
        {
            MessageCategories = GetProblemCategory("category", 3, string.Empty);
            MessageCategoriesDropDown = (from p in MessageCategories
                                         select new SelectListItem { Value = p, Text = p }).ToList();
            Customers = GetCustomers();
            CustomersDropDown = GetCustomersDropDown(Customers);
            MessageRecordManager mgr =
             new MessageRecordManager();
            
            // Get Product Data
            Entity = mgr.Get(
              Convert.ToInt32(EventArgument));

            base.Display();
        }
        protected override void Save()
        {
            MessageRecordManager mgr =
              new MessageRecordManager();

            if (Mode == "Add")
            {
                mgr.Insert(Entity);
            }
            else
            {
                mgr.Update(Entity);
            }

            // Set any validation errors
            ValidationErrors = mgr.ValidationErrors;

            // Set mode based on validation errors
            base.Save();
        }

        protected override void Delete()
        {
            MessageRecordManager mgr =
              new MessageRecordManager();

            // Create new entity
            Entity = new Message();

            // Get primary key from EventArgument
            Entity.MES_UID =
              Convert.ToInt32(EventArgument);

            // Call data layer to delete record
            mgr.Delete(Entity);

            // Reload the Data
            Get();

            base.Delete();
        }

        protected override void ResetSearch()
        {
            SearchEntity = new Message();

            base.ResetSearch();
        }

        protected override void Get()
        {
            MessageRecordManager mgr = new MessageRecordManager();

            Messages = mgr.Get(SearchEntity);
            // check for _page and _pagesize /Not Required
            //if (_page == 0)
            //    _page = _firstPageNumber;
            //if (_pageSize == 0)
            //    _pageSize = _ItemsInaPage;
            PagedMessages = Messages.ToPagedList(_page, _pageSize);
            base.Get();
        }

        
    }
}
