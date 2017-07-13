using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
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

        public AddRequestViewModel()
        {
            Title = "";
            Description = "";
            DeadlineDate = DateTime.Now.Subtract((DateTime.Now).TimeOfDay);
            DeadlineTime = new DateTime().AddHours(DateTime.Now.Hour).AddHours(2);

        }

    }

    public class MinDate : ValidationAttribute
    {
        private readonly DateTime _minDate;

        public MinDate(DateTime minDate)
        {
            _minDate = minDate;
        }

        public override bool IsValid(object value)
        {
            return (DateTime) value >= _minDate;
        }

    }

    public class MinTimeAttribute : ValidationAttribute
    {
        private readonly DateTime _minTime;

        public MinTimeAttribute(DateTime minTime)
        {
            _minTime = new DateTime().Add(minTime.TimeOfDay);
        }

        public override bool IsValid(object value)
        {
            var valueTime = new DateTime().Add(((DateTime) value).TimeOfDay);

            return valueTime >= _minTime;
        }

    }
}
