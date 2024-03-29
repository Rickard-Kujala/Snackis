﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Areas.Identity.Data;

namespace Snackis.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;

        public IndexModel(
            UserManager<SnackisUser> userManager,
            SignInManager<SnackisUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty(SupportsGet =true)]
        public string ImageURL { get; set; }


        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            // Ny Kod -->
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Smeknamn")]
            public string NickName { get; set; }

            [Required]
            [Display(Name = "Födelseår")]
            public int BirthYear { get; set; }
            // <-- Ny kod
            [BindProperty]
            public byte[] Image { get; set; }

            [Display(Name = "Personlig beskrivning")]
            public string PersonalText { get; set; }

        }

        private async Task LoadAsync(SnackisUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                NickName = user.NickName,
                BirthYear = user.BirthYear,
                Image = user.Image,
                PersonalText = user.PersonalText
                
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (user.Image != null)
            {
                string imageBase64Data = Convert.ToBase64String(user.Image);
                ImageURL = string.Format($"data:image/jpg;base64, {imageBase64Data}");
            }
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            // Ny Kod -->
            if (Input.NickName != user.NickName)
            {
                user.NickName = Input.NickName;
            }
            if (Input.BirthYear != user.BirthYear)
            {
                user.BirthYear = Input.BirthYear;
            }
            if (Request.Form.Files.Count >0 )
            {
                var file=Request.Form.Files[0];
                Byte[] image;
                using (MemoryStream ms = new())
                {
                    await file.CopyToAsync(ms);
                    image = ms.ToArray();
                }
                    user.Image = image;
            }
            if (Input.PersonalText!=null)
            {
                user.PersonalText = Input.PersonalText;
            }
            await _userManager.UpdateAsync(user);
            // <-- Ny kod

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Dina ändringar har sparats!";
            return RedirectToPage();

            
        }
    }
}
