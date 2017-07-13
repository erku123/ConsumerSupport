using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerSupport.Data;
using ConsumerSupport.Entities.Requests;
using Microsoft.EntityFrameworkCore;

namespace ConsumerSupport.Models.Requests
{
    public interface IRequestChanger
    {
        void DeleteRequest(int Id);
        Request EditRequest(ChangeRequestViewModel changeRequest);
        ChangeRequestViewModel GetChangeRequest(int Id);
    }

    public class RequestChanger : IRequestChanger
    {
        private readonly ApplicationDbContext _context;

        public RequestChanger(ApplicationDbContext context)
        {
            _context = context;
        }

        protected virtual Request Find(int Id)
        {
            return _context.Requests.Find(Id);
        }

        public ChangeRequestViewModel GetChangeRequest(int Id)
        {
            var request = Find(Id);

            var changeRequest = new ChangeRequestViewModel();
            changeRequest.Id = request.Id;
            changeRequest.Title = request.Title;
            changeRequest.Description = request.Description;
            changeRequest.DeadlineDate = request.Deadline.Subtract(request.Deadline.TimeOfDay);
            changeRequest.DeadlineTime = (new DateTime()).Add(request.Deadline.TimeOfDay);

            return changeRequest;
        }

        public void DeleteRequest(int Id)
        {
            var request = _context.Requests.Find(Id);
            _context.Requests.Remove(request);

            _context.SaveChanges();
        }

        public Request EditRequest(ChangeRequestViewModel changedRequest)
        {
            var request = Find(changedRequest.Id);
            var deadline = changedRequest.DeadlineDate.Add(changedRequest.DeadlineTime.TimeOfDay);

            request.SetTitle(changedRequest.Title);
            request.SetDescription(changedRequest.Description);
            request.SetDeadline(deadline);

            _context.SaveChanges();

            return request;
        }

    }
}
