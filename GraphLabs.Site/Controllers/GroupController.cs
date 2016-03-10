﻿using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class GroupController : GraphLabsController
    {
        #region Зависимости

        private readonly IUsersContext _usersContext;
        private readonly IGroupRepository _groupRepository;

        private readonly ISystemDateService _dateService;

        #endregion

        public GroupController(
            IUsersContext usersContext,
            IGroupRepository groupRepository, ISystemDateService dateService)
        {
            _usersContext = usersContext;
            _groupRepository = groupRepository;
            _dateService = dateService;
        }

        #region Формирование списка групп

        public ActionResult Index(string message)
        {
            Group[] groups = _groupRepository.GetAllGroups();
            GroupModel[] groupModel = groups.Select(t => new GroupModel(t, _dateService)).ToArray();

            return View(groupModel);
        }

        #endregion

        #region Создание группы

        public ActionResult Create()
        {
            var group = _usersContext.Groups.CreateNew();
            group.FirstYear = _dateService.GetDate().Year;

            return View(group);
        }
        
        [HttpPost]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                if (_groupRepository.TrySaveGroup(group))
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Message = "Невозможно сохранить группу";
            }

            return View(group);
        }

        #endregion

        #region Редактирование группы

        public ActionResult Edit(long id = 0)
        {
            GroupModel group = new GroupModel( _groupRepository.GetGroupById(id), _dateService );

            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(GroupModel gr)
        {
            if (ModelState.IsValid)
            {
                if (_groupRepository.TryModifyGroup(gr.Id, gr.Number, gr.FirstYear, gr.IsOpen))
                {
                    return RedirectToAction("Index");
                }

                ViewBag.Message = "Невозможно обновить группу";
            }
            return View(gr);
        }

        #endregion
    }
}