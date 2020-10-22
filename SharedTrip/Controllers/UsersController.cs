﻿using SharedTrip.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        public HttpResponse Login()
        {
            if (this.IsUserSignedIn())
            {
                return this.Error("You are already logged in.");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username,string password)
        {
            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(CreateUserInputModel model)
        {

        }
    }
}
