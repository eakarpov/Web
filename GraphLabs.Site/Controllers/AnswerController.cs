using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using GraphLabs.Site.Models.AnswerVariant;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class AnswerController : GraphLabsController
    {
        private readonly IEntityBasedModelRemover<AnswerVariantModel, AnswerVariant> _modelRemover;
        private readonly IEntityBasedModelSaver<AnswerVariantModel, AnswerVariant> _modelSaver;


        public AnswerController(
            IEntityBasedModelRemover<AnswerVariantModel, AnswerVariant> modelRemover,
            IEntityBasedModelSaver<AnswerVariantModel, AnswerVariant> modelSaver)
        {
            _modelRemover = modelRemover;
            _modelSaver = modelSaver;
        }


        [HttpPost]
        public ActionResult Create(AnswerVariantModel model)
        {
            try
            {
                var entity = _modelSaver.CreateOrUpdate(model);
                return Json(entity.Id);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult Delete(AnswerVariantModel model)
        {
            try
            {
                _modelRemover.Remove(model.Id);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
    }
}
