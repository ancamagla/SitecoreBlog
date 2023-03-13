using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using SitecoreBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using static SitecoreBlog.Models.SearchModel;

namespace SitecoreBlog.Controllers
{
    public class ArticlesController : Controller
    {
        // GET: Articles
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetArticles()
        {
            var item = Sitecore.Context.Database.GetItem("{C71E7BF1-F998-482C-BA93-B30E86707A2D}");

            //Sitecore.Data.Fields.MultilistField multilistField = item.Fields["Articles"];

            ////List<object> articles = new List<object>();
            //if (multilistField != null)
            //{
            //    foreach (Sitecore.Data.Items.Item tempItem in multilistField.GetItems())
            //    {
            //        //var article = Sitecore.Context.Database.GetItem(tempItem.ID);
            //        //articles.Add(article);
            //        string name = tempItem.Name;
            //    }
            //}


            return View("~/Views/SitecoreBlog/ArticlesController.cshtml", item);
        }

        public ActionResult DoSearch(string searchText)
        {
            var myResults = new SearchResults
            {
                Results = new List<SearchResult>()
            };
            var searchIndex = ContentSearchManager.GetIndex("sitecore_web_index"); // Get the search index
            var searchPredicate = GetSearchPredicate(searchText); // Build the search predicate
            using (var searchContext = searchIndex.CreateSearchContext()) // Get a context of the search index
            {
                //Select * from Sitecore_web_index Where Author="searchText" OR Description = "searchText" OR Title = "searchText"
                var searchResults =
               searchContext.GetQueryable<SearchModel>().Where(searchPredicate); // Search the index for items which match the predicate

                // This will get all of the results, which is not reccomended
                var fullResults = searchResults.GetResults();
                // This is better and will get paged results - page 1 with 10 results per page
                //var pagedResults = searchResults.Page(1, 10).GetResults();
                foreach (var hit in fullResults.Hits)
                {
                    myResults.Results.Add(new SearchResult
                    {

                        Title = hit.Document.Title,
                        ItemName = hit.Document.ItemName

                    });
                }
                return new JsonResult
                {
                    JsonRequestBehavior =
               JsonRequestBehavior.AllowGet,
                    Data = myResults
                };
            }
        }

        public static Expression<Func<SearchModel, bool>> GetSearchPredicate(string
searchText)
        {
            var predicate = PredicateBuilder.True<SearchModel>(); // Items which meet the predicate
                                                                  // Search the whole phrase - LIKE
                                                                  // predicate = predicate.Or(x => x.DispalyName.Like(searchText)).Boost(1.2f);
                                                                  // predicate = predicate.Or(x => x.Description.Like(searchText)).Boost(1.2f);
                                                                  // predicate = predicate.Or(x => x.Title.Like(searchText)).Boost(1.2f);
                                                                  // Search the whole  phrase - CONTAINS

            predicate = predicate.Or(x => x.Title.Contains(searchText)); // .Boost(2.0f);
                                                                         //Where Author="searchText" OR Description="searchText" OR Title="searchText"
            return predicate;
        }

    }
}