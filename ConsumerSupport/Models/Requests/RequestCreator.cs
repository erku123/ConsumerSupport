using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using ConsumerSupport.Data;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Infrastructure.PrincipalExtensions;

namespace ConsumerSupport.Models.Requests
{
    public interface IRequestCreator
    {
        void Create(AddRequestViewModel model, IPrincipal user);
    }

    public class RequestCreator : IRequestCreator
    {
        private readonly ApplicationDbContext _context;

        public RequestCreator(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(AddRequestViewModel model, IPrincipal user)
        {
            var deadline = model.DeadlineDate.Add(model.DeadlineTime.TimeOfDay);
            var request = new Request(model.Title, model.Description, deadline, user.GetUserId());

            _context.Requests.Add(request);
            _context.SaveChanges();
        }
    }
    
}
