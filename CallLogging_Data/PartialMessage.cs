using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallLogging_Data
{
    public partial class Message
    {
        public bool UrgentBool {
            get
            {
                
                if (string.IsNullOrEmpty(this.MES_Urgent))
                {
                    
                    return false;
                }
                else
                {
                    
                    return true;
                }

            }
            set
            {
                if (value == true)
                {
                   
                    this.MES_Urgent = "!";
                }
                else
                {
                   
                    this.MES_Urgent = string.Empty;
                }


            }
        }

    }
}
