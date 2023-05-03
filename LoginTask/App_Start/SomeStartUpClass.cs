using LoginTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginTask.App_Start
{
    public class SomeStartUpClass
    {
        public static void Init()
        {
            DbUserSignUpLoginEntities1 obj = new DbUserSignUpLoginEntities1();
            bool b = false;
            var res = obj.TblUsers.FirstOrDefault(x => x.UserID == 1);
            if (res != null)
            {
                b = true;
            }
            Console.WriteLine(b);
        }
    }
}