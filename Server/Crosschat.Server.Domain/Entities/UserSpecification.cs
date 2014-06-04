using System;
using Crosschat.Server.Domain.Seedwork.Specifications;

namespace Crosschat.Server.Domain.Entities
{
    public static class UserSpecification
    {
        public static Specification<User> Id(int id)
        {
            return new DirectSpecification<User>(i => i.Id == id);
        }

        public static Specification<User> NameAndPassword(string name, string psw)
        {
            return new DirectSpecification<User>(i => i.Name == name && i.Password == psw && !i.IsDeleted);
        }

        public static Specification<User> Name(string name)
        {
            return new DirectSpecification<User>(i => name.Equals(i.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        public static Specification<User> Huid(string huid)
        {
            return new DirectSpecification<User>(i => i.Huid == huid);
        }

        public static Specification<User> NameLike(string name)
        {
            return new DirectSpecification<User>(i => !i.IsDeleted && i.Name.StartsWith(name));
        }
    }
}
