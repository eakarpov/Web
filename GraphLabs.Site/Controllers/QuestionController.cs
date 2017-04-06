using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.Site.Models.TestQuestion;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class QuestionController : GraphLabsController
    {
        private readonly IListModelLoader _listModelLoader;

        public QuestionController(IListModelLoader listModelLoader)
        {
            _listModelLoader = listModelLoader;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectQuestions()
        {
            var model = _listModelLoader.LoadListModel<TestQuestionListModel, TestQuestionModel>();
            return View(model);
        }

    }
}
