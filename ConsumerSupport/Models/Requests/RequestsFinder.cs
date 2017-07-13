using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using ConsumerSupport.Entities.Requests;
using ConsumerSupport.Data;
using ConsumerSupport.Infrastructure.PrincipalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ConsumerSupport.Models.Requests
{   

    public interface IRequestsFinder
    {
        List<Request> FindByUser(IPrincipal user);
    }

    public class RequestsFinder : IRequestsFinder
    {

        private readonly ApplicationDbContext _context;

        public RequestsFinder(ApplicationDbContext context)
        {
            _context = context;

        }

        protected virtual List<Request> GetAll()
        {
            return _context.Requests.ToList();
        }

        public List<Request> FindByUser(IPrincipal user)
        {

            var result = FindByUserId(user.GetUserId());
          
            return result;
        }

        public List<Request> FindByUserId(string userId)
        {
            var all = GetAll();

            var result = new List<Request>();

            foreach (var request in all)
            {
                if (request.UserId == userId)
                    result.Add(request);
            };

            return result;
        }


    }
}
