﻿using RecipeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeManager.Controllers
{
    public class RecipesController : Controller
    {
        // GET: Recipes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Recipe (int RecipeId)
        {
            Recipe model = RecipeDb.SelectRecipe(RecipeId);

            return View(model);
        }

        public ActionResult Search(string SearchTerm)
        {
            SearchViewModel model = new SearchViewModel(SearchTerm);
            return View(model);
        }
        
        public ActionResult AddRecipe()
        {
            AddRecipeViewModel model = new AddRecipeViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddRecipe(AddRecipeViewModel model)

        {
          
    
            RecipeDb.Insert(model);
            return RedirectToAction("AllRecipes");
            

           
        }

        public ActionResult AllRecipes()
        {
            AllRecipesViewModel model =new AllRecipesViewModel();
            return View(model);
        }
        public ActionResult Viewerprofile()
        {
            return View();
        }
        public ActionResult ShoppingList()

        {
            int UserId = AccountController.currentUser.UserId;

            ShoppingListViewModel model = new ShoppingListViewModel(UserId);
            return View(model);
        }
        public ActionResult UserRecipes(int? Remove = null)
        {
            if (Remove.HasValue)
            {
                RecipeDb.RemoveFromMyRecipes(Remove.Value);
            }

            RecipeListViewModel model;
            model = new RecipeListViewModel();
            return View(model);
        }

        public ActionResult ViewUsers()
        {
            AllUsersViewModel users = new AllUsersViewModel();
            return View(users);

        }
    
        public ActionResult AccountInfo(int UserId, string UserNme)
        {
            ProfileViewModel model = new ProfileViewModel(UserId, UserNme);
            return View(model);
        }
        public ActionResult AddToMyRecipes(int RecipeId)

        {
            RecipeDb.AddToMyRecipes(RecipeId);
            return RedirectToAction("UserRecipes");
        }

        public ActionResult AddToMyShoppingList(string IngName)
        {
           // string UserName = AccountController.currentUser.Username;

           // System.Diagnostics.Debug.WriteLine("Printing UserId");
            //System.Diagnostics.Debug.WriteLine(UserId);
            RecipeDb.AddToMyShoppingList(IngName);
            return RedirectToAction("ShoppingList");
        }

        public ActionResult Delete(string IngName)
        {
            return RedirectToAction("ShoppingList");
        }
    }
}