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
        Request[] FindByUserId(string userId);
        Request Find(int id);
    }

    public class RequestsFinder : IRequestsFinder
    {

        private readonly ApplicationDbContext _context;

        public RequestsFinder(ApplicationDbContext context)
        {
            _context = context;

        }


        public Request[] FindByUserId(string userId)
        {
            return _context.Requests.Where(r => r.UserId == userId).ToArray();
        }

        public Request Find(int id)
        {
            return _context.Requests.Find(id);
        }
    }
}
