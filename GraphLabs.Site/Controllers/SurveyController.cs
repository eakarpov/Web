using GraphLabs.Dal.Ef;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Controllers.LabWorks;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Web.Helpers;
using System.Web.Routing;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Models.AnswerVariant;
using GraphLabs.Site.Models.Category;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPool;
using GraphLabs.Site.Models.TestPoolEntry;
using GraphLabs.Site.Models.TestQuestion;
using WebGrease.Css.Extensions;

namespace GraphLabs.Site.Controllers
{
    public class QuestionLookForModel
    {
        public string Question { get; set; }

        public long TestPool { get; set; }
    }


	[GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class SurveyController : GraphLabsController
	{
	    private readonly ISurveyRepository _surveyRepository;
	    private readonly ICategoryRepository _categoryRepository;
        private readonly IEntityBasedModelLoader<TestPoolModel, TestPool> _modelPoolLoader;
	    private readonly IEntityBasedModelRemover<EditTestQuestionModel, TestQuestion> _modelRemover;
	    private readonly IEntityBasedModelSaver<EditTestQuestionModel, TestQuestion> _modelSaver; 

        public SurveyController(
            ISurveyRepository surveyRepository,
            ICategoryRepository categoryRepository,
            IEntityBasedModelLoader<TestPoolModel, TestPool> modelPoolLoader,
            IEntityBasedModelRemover<EditTestQuestionModel, TestQuestion> modelRemover,
            IEntityBasedModelSaver<EditTestQuestionModel, TestQuestion> modelSaver)
        {
            _modelPoolLoader = modelPoolLoader;
            _modelRemover = modelRemover;
	        _surveyRepository = surveyRepository;
	        _categoryRepository = categoryRepository;
            _modelSaver = modelSaver;
        }

	    #region Просмотр списка

		[HttpGet]
		public ActionResult Index(long categoryId = 0)
		{
            var model = new SurveyIndexViewModel(_surveyRepository, _categoryRepository);
            model.Load(categoryId);

			return View("~/Views/Survey/Index.cshtml", model);
		}

	    [HttpGet]
	    public ActionResult Edit(long questionId = 0)
	    {
            var entity = _surveyRepository.GetAllQuestions().Single(t => t.Id == questionId);
            var modelAnswer = new AnswerVariantModel[entity.AnswerVariants.Count];
	        for (int i = 0; i < entity.AnswerVariants.Count; i++)
	        {
	            modelAnswer[i] = new AnswerVariantModel
                {
                    Id = entity.AnswerVariants.ElementAt(i).Id,
                    Answer = entity.AnswerVariants.ElementAt(i).Answer,
                    IsCorrect = entity.AnswerVariants.ElementAt(i).IsCorrect,
                    TestQuestion = entity.AnswerVariants.ElementAt(i).TestQuestion.Id
                };
            }
            var modelCategory = new CategoryModel
            {
                Id = entity.Category.Id,
                Name = entity.Category.Name
            };
            var model = new EditTestQuestionModel
	        {
                Id = entity.Id,
                Question = entity.Question,
                AnswerVariants = modelAnswer,
                Category = modelCategory,
            };
	        return View("~/Views/Survey/Edit.cshtml", model);
	    }

	    [HttpPost]
	    public ActionResult Edit(EditTestQuestionModel editTestQuestion)
	    {
	        try
	        {
	            _modelSaver.CreateOrUpdate(editTestQuestion);
                return Json(true);
            }
	        catch (Exception e)
	        {
	            return Json(false);
	        }
	    }

		[HttpGet]
		public ActionResult TestQuestionList(long CategoryId)
		{
			var model = new TestQuestionListViewModel(_surveyRepository);
            model.Load(CategoryId);

			return new JsonResult
			{
				Data = RenderHelper.PartialView(this, "~/Views/Survey/TestQuestionListPartial.cshtml", model),
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

	    [HttpPost]
	    public ActionResult Load(QuestionLookForModel input)
	    {
	        TestQuestion[] questions = _surveyRepository.GetQuestionsSimilarToString(input.Question);
            var questionArray = questions.Select(q => new Tuple<string, long>(
            q.Question,q.Id))
                .ToArray();
            var json = Json(questionArray);
	        return json;
	    }

	    [HttpPost]
	    public ActionResult LoadUnique(QuestionLookForModel input)
	    {
	        // Новый код подгружает только те вопросы, которых ещё нет в данном тестпуле
	        var entity = _modelPoolLoader.Load(input.TestPool);
	        TestQuestion[] questions = _surveyRepository.GetQuestionsSimilarToString(input.Question);
	        var questionArray = questions
                .Where(q => entity.TestPoolEntries.All(t => t.TestQuestion.Question != q.Question))
                .Select(q => new Tuple<string, long>(q.Question, q.Id))
                .ToArray();
            var json = Json(questionArray);
            return json;
        }

		#endregion

		#region Создание вопроса

		[HttpGet]
		public ActionResult Create()
        {
            var emptyQuestion = new SurveyCreatingModel(_surveyRepository, _categoryRepository);
            emptyQuestion.Question = "";
            emptyQuestion.QuestionOptions.Add(new KeyValuePair<string, bool>("", true));

            return View("~/Views/Survey/Create.cshtml", emptyQuestion);
        }

        [HttpPost]
        public ActionResult Create(string question, Dictionary<string, bool> questionOptions, long categoryId)
        {
            //Question, QuestionOptions, CategoryId
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                CategoryId = categoryId,
                QuestionOptions = questionOptions.ToList(),
                Question = question
            };

            if (model.IsValid)
            {
                model.Save();
				return RedirectToAction("Index", new RouteValueDictionary { { "CategoryId", categoryId } });
            }

			return View("~/Views/Survey/Create.cshtml", model);
		}

	    [HttpPost]
	    public ActionResult Delete(TestQuestionModel testQuestionModel)
	    {
	        try
	        {
	            _modelRemover.Remove(testQuestionModel.Id);
	            return RedirectToAction("Index");
	        }
	        catch (Exception e)
	        {
	            return Json(false);
	        }
	    }

		#endregion
	}
}
