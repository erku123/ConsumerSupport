using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Networking;
using Remotion.Linq.Utilities;

namespace ConsumerSupport.Models.Requests
{
    public class AddRequestViewModel
    {
        [MaxLength(10, ErrorMessage = "max length is 10")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DeadlineDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime DeadlineTime { get; set; }

    }
}
