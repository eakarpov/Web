﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel.Utils;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Контроллер для заданий </summary>
    public class TaskController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        //
        // GET: /Tasks/
        /// <summary> Начальная отрисовка списка </summary>
        public ActionResult Index(string message)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            var tasks = (from task in _ctx.Tasks
                         select task).ToArray()
                        .Select(t => new TaskModel(t, true))
                        .ToArray();

            ViewBag.Message = message;

            return View(tasks);
        }

        #region Upload

        /// <summary> Начальная отрисовка формы загрузки </summary>
        public ActionResult Upload(string message)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            ViewBag.Message = message;

            return View();
        }

        /// <summary> Загружаем задание </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase xap)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            // Verify that the user selected a file
            if (xap != null && xap.ContentLength > 0)
            {
                var newTask = _ctx.Tasks.CreateFromXap(xap.InputStream);
                if (newTask == null)
                    return RedirectToAction("Upload", "Task", new { Message = UserMessages.UPLOAD_ERROR });

                var sameTasks = _ctx.Tasks.Where(t => t.Name == newTask.Name && t.Version == newTask.Version);
                if (sameTasks.Any())
                {
                    return RedirectToAction("Upload", "Task", new { Message = UserMessages.TASK_EXISTS });
                }

                try
                {
                    _ctx.Tasks.Add(newTask);
                    _ctx.SaveChanges();
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Task", new { Message = UserMessages.UPLOAD_ERROR });
                }
                

                return RedirectToAction("Edit", "Task", new { Id = newTask.Id });
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Upload", "Task", new { Message = UserMessages.UPLOAD_FILE_NOT_SPECIFIED }); 
        }

        #endregion


        #region Edit
        
        //
        // GET: /Tasks/Edit
        /// <summary> Начальная отрисовка формы редактирования </summary>
        public ActionResult Edit(long id, string message)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            var task = _ctx.Tasks.Find(id);
            if (task == null)
                return RedirectToAction("Index");
            
            ViewBag.Message = message;

            return View(new TaskModel(task));
        }

        //
        // POST: /Tasks/Edit
        /// <summary> Начальная отрисовка формы редактирования </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskModel model)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            model.SaveToDb(_ctx);
            return RedirectToAction("Edit", new { Id = model.Id, Message = UserMessages.EDIT_COMPLETE });
        }

        //
        // POST: /Tasks/EditVariantGenerator
        /// <summary> Начальная отрисовка формы редактирования </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVariantGenerator(HttpPostedFileBase newGenerator, TaskModel model,
            string upload, string delete)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            if (!string.IsNullOrEmpty(upload))
            {
                // Проверим, что вообще есть, что загружать
                if (newGenerator != null && newGenerator.ContentLength > 0)
                {
                    // Убедимся, что хотя бы формат правильный (xap, и нам удалось вытащить информацию о нём)
                    if (XapProcessor.Parse(newGenerator.InputStream) != null)
                    {
                        // Наконец, сохраним.
                        var task = _ctx.Tasks.Find(model.Id);
                        task.VariantGenerator = newGenerator.InputStream.ReadToEnd();
                        _ctx.SaveChanges();

                        return RedirectToAction("Edit", new {Id = model.Id, Message = UserMessages.EDIT_COMPLETE});
                    }

                    return RedirectToAction("Edit", new { Id = model.Id, Message = UserMessages.UPLOAD_ERROR });
                }

                return RedirectToAction("Edit", new { Id = model.Id, Message = UserMessages.UPLOAD_FILE_NOT_SPECIFIED });
            }

            if (!string.IsNullOrEmpty(delete))
            {
                var task = _ctx.Tasks.Find(model.Id);
                if (task.VariantGenerator != null)
                {
                    task.VariantGenerator = null;
                    _ctx.SaveChanges();
                }

                return RedirectToAction("Edit", new { Id = model.Id, Message = UserMessages.EDIT_COMPLETE });
            }

            throw new ArgumentException("Ошибка при обработке запроса на редактирования генератора вариантов - неверный набор входных аргументов.");
        }

        #endregion
    }
}