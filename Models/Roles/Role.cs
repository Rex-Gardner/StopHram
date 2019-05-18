using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.Identity.Mongo.Model;

namespace Models.Roles
{
    public class Role : MongoRole
    {
        public Role() : base()
        {

        }

        public Role(string name) : base(name)
        {

        }
    }
}
