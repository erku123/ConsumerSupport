using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerSupport.Data;

namespace ConsumerSupport.Models.Requests
{

    public interface IRequestAuthorizer
    {
        bool CanAccess(int id, string userId);
    }

    public class RequestAuthorizer : IRequestAuthorizer
    {

        private readonly ApplicationDbContext _context;

        public RequestAuthorizer(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CanAccess(int id, string userId)
        {
            return _context.Requests.Find(id).UserId == userId;
        }
    }
}
