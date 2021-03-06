﻿using System;
using ConsumerSupport.Models;

namespace ConsumerSupport.Entities.Requests
{
    public class Request
    {

        public Request(
            string title,
            string description,
            DateTime deadline,
            string userId)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
            UserId = userId;

            CreatedOn = DateTime.Now;
        }

        public int Id { get; private set; }
        public string UserId { get; private set; }
        public virtual ApplicationUser User { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime Deadline { get; private set; }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetDeadline(DateTime deadline)
        {
            Deadline = deadline;
        }

        protected Request()
        {
            
        }

        
    }
}
