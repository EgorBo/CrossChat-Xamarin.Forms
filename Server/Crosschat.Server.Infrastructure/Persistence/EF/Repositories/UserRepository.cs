using System;
using System.Linq;
using System.Linq.Expressions;
using Crosschat.Server.Domain.Entities;
using Crosschat.Server.Domain.Repositories;
using Crosschat.Server.Domain.Seedwork.Specifications;

namespace Crosschat.Server.Infrastructure.Persistence.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWork _uow;

        public UserRepository(UnitOfWork uow)
        {
            _uow = uow;
        }

        public User FirstMatching(Specification<User> spec)
        {
            var player = _uow.Users.FirstOrDefault(spec.SatisfiedBy());
            return player;
        }

        public bool AnyMatching(Specification<User> spec)
        {
            return _uow.Users.Any(spec.SatisfiedBy());
        }

        public void Add(User user)
        {
            _uow.Users.Add(user);
        }

        public User[] AllMatching(Specification<User> spec, int limit = 0)
        {
            var players = _uow.Users.Where(spec.SatisfiedBy());
            if (limit > 0)
                players = players.Take(limit);
            return players.ToArray();
        }

        public User[] TakeAllMatching<TOrderBy>(int count, Specification<User> spec, Expression<Func<User, TOrderBy>> orderBy)
        {
            if (spec != null)
                return _uow.Users.Where(spec.SatisfiedBy()).OrderByDescending(orderBy).Take(count).ToArray();
            return _uow.Users.OrderByDescending(orderBy).Take(count).ToArray();
        }
    }
}
