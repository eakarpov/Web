﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Utils;

namespace GraphLabs.Site.Models
{
    /// <summary> Данные о задании </summary>
    public class TaskModel
    {
        /// <summary> Id </summary>
        public long Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Темы")]
        public string Sections { get; set; }

        [Display(Name = "Версия")]
        public string Version { get; set; }

        [Display(Name = "Примечание")]
        public string Note { get; set; }

        
        #region VariantGenerator 

        [Display(Name = "Название")]
        public string VariantGeneratorName { get; set; }

        [Display(Name = "Версия")]
        public string VariantGeneratorVersion { get; set; }

        /// <summary> Есть ли генератор вариантов? </summary>
        public bool HasVariantGenerator
        {
            get
            {
                return !string.IsNullOrEmpty(VariantGeneratorName) || 
                    !string.IsNullOrEmpty(VariantGeneratorVersion);
            }
        }

        #endregion


        /// <summary> Ctor. </summary>
        public TaskModel()
        {
        }

        /// <summary> Ctor. </summary>
        public TaskModel(Task task, bool loadForTableView = false)
        {
            Contract.Requires(task != null);

            Id = task.Id;
            Name = task.Name;
            Sections = task.Sections;
            Version = task.Version;

            const int NOTE_CUT_LENGTH = 30;
            if (loadForTableView && task.Note != null && task.Note.Length > NOTE_CUT_LENGTH)
            {
                Note = string.Format("{0}...", task.Note.Substring(0, NOTE_CUT_LENGTH).Trim());
            }
            else
            {
                Note = task.Note;
            }

            if (loadForTableView || task.VariantGenerator == null)
            {
                return;
            }

            using (var generator = new MemoryStream(task.VariantGenerator))
            {
                var info = XapProcessor.Parse(generator);
                VariantGeneratorName = info.Name;
                VariantGeneratorVersion = info.Version;
            }
        }

        /// <summary> Вытаскивает из БД соответствующий экземпляр 
        /// задания, вносит изменения и сохраняет. </summary>
        public Task SaveToDb(GraphLabsContext ctx)
        {
            Contract.Requires(ctx != null);
            Contract.Ensures(Contract.Result<Task>() != null);

            var task = ctx.Tasks.Single(t => t.Id == Id);
            
            if (task.Note != Note)
            {
                task.Note = Note;
                ctx.SaveChanges();
            }

            return task;
        }
    }
}