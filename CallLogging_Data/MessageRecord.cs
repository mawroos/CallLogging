using System;
using System.ComponentModel.DataAnnotations;

namespace CallLogging_Data
{
    public class MessageRecord
    {
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Problem  must be filled in.")]
        [Display(Description = "Problem")]
        [StringLength(350, MinimumLength = 4,
           ErrorMessage = "Problem  must be greater than {2} characters and less than {1} characters.")]
        public string MessageProblem { get; set; }
        public string MessageResolution { get; set; }
        public int MessagePriority { get; set; }
        public string MessageModule { get; set; }
        public string MessageMenu { get; set; }
        public string MessageCompany { get; set; }
        public DateTime MessageDate { get; set; }
        public Status MessageStatus { get; set; }
        public WaitStatus MessageWaitStatus { get; set; }
        public string MessageTakenUser { get; set; }
        public string MessageUser { get; set; }

        public enum Status
        {
            InProgress,
            Resolved
        }
        public enum WaitStatus
        {
            HelpDesk,
            Customer,
            Analysis
        }
    }
    
}
