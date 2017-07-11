using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerSupport.Models.Requests
{
    public class AddRequestViewModel
    {

        public string Title { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime DeadlineDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime DeadlineTime { get; set; }

    }
}
