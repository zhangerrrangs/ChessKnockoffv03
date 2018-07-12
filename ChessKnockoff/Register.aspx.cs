﻿using ChessKnockoff.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static ChessKnockoff.Utilities;
using static ChessKnockoff.Validation;

namespace ChessKnockoff
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private void checkUsername(object source, ServerValidateEventArgs args)
        {
            //Pass on validation to the username validation function
            validateUsername(source, args);
        }

        private void checkEmail(object source, ServerValidateEventArgs args)
        {
            //Pass on the values to the email validation function
            validateEmail(source, args);
        }

        private void checkPassword(object source, ServerValidateEventArgs args)
        {
            //Pass on validation to the password validation function
            validatePassword(source, args, inpPasswordRegister.Value, inpRePasswordRegister.Value);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //If user is already logged in
            if (User.Identity.IsAuthenticated)
            {
                //Redirect them to the play page
                Response.Redirect("~/Play");
            }

            //Make the current link in the navbar active
            activateNav(this, "likRegister");

            //Hide errors as the viewstate is not saved
            altPassword.Visible = false;
            altEmailTaken.Visible = false;
            altUsernameTaken.Visible = false;
            altError.Visible = false;
        }

        protected void RegisterClick(object sender, EventArgs e)
        {
            //Check if controls in the group are all valid
            if (IsValid)
            {
                //Create manager
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                //Check if password is valid
                var resultPassword = manager.PasswordValidator.ValidateAsync(inpPasswordRegister.Value).Result;

                //Check if the password can be used
                if (!resultPassword.Succeeded)
                {
                    //Show the error
                    altPassword.Visible = true;
                    //Also show the specific issue
                    altPassword.InnerText = resultPassword.Errors.FirstOrDefault<string>();
                }

                //Check if username is not taken
                var resultUsername = manager.FindByName(inpUsernameRegister.Value);

                //Check if the user is not null
                if (resultUsername != null)
                {
                    //Show the error
                    altUsernameTaken.Visible = true;
                }

                //Check if email is valid
                var resultEmail = manager.FindByEmail(inpEmailRegister.Value);

                if (resultEmail != null)
                {
                    //Show the error
                    altEmailTaken.Visible = true;
                }

                //If there are no errors
                if (resultEmail == null && resultPassword.Succeeded && resultUsername == null)
                {
                    //Create sign in manager
                    var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();

                    var user = new ApplicationUser() { UserName = inpUsernameRegister.Value, Email = inpEmailRegister.Value };

                    IdentityResult result = manager.Create(user, inpPasswordRegister.Value);

                    //Check if it succeeded
                    if (result.Succeeded)
                    {
                        //Send email confirmation link
                        string code = manager.GenerateEmailConfirmationToken(user.Id);
                        string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                        manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                        //Redirect them to the login page
                        Response.Redirect("~/Login?Registered=1");
                    }
                    else
                    {
                        //Write to the debug log something has occured
                        System.Diagnostics.Debug.WriteLine(result.Errors.FirstOrDefault<string>());
                    }
                }
            }
        }
    }
}