﻿using System;
using System.IO;
using System.ServiceModel;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Utils;

namespace GraphLabs.WcfServices.DebugTaskUploader
{
    /// <summary> Вспомогательный сервис для отладки заданий на сайте </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)] 
    public class DebugTaskUploader : IDebugTaskUploader
    {
        private readonly IGraphLabsContext _context;
        private readonly ITaskManager _taskManager;

        public DebugTaskUploader(
            IGraphLabsContext graphLabsContext,
            ITaskManager taskManager)
        {
            _context = graphLabsContext;
            _taskManager = taskManager;
        }

        /// <summary> Загрузить задание для отладки </summary>
        public DebugTaskData UploadDebugTask(byte[] taskData, byte[] variantData)
        {
            if (!WorkingMode.IsDebug())
            {
                throw new InvalidOperationException(
                    "Создание тестовых вариантов возможно только при работе в тестовом режиме.");
            }

            // Загружаем задание
            Task task;
            using (var stream = new MemoryStream(taskData))
            {
                task = _taskManager.UploadTaskWithTimestamp(stream);
            }
            if (task == null)
                throw new InvalidOperationException("Провал. Задание с таким именем и версией уже существует.");

            task.Note = "Загружено автоматически сервисом отладки.";

            // Загружаем вариант задания
            var taskVariant = _context.Create<TaskVariant>();
            taskVariant.Data = variantData;
            taskVariant.GeneratorVersion = "1";
            taskVariant.Number = "Debug";
            taskVariant.Task = task;

            // Создаём лабу
            var now = DateTime.Now;
            var lab = _context.Create<LabWork>();
            lab.Name = $"Отладка модуля \"{task.Name}\"";
            lab.AcquaintanceFrom = now.Date;
            lab.AcquaintanceTill = now.Date.AddDays(7);

            // Добавляем задание в лабу
            var labEntry = _context.Create<LabEntry>();
            labEntry.LabWork = lab;
            labEntry.Order = 0;
            labEntry.Task = task;

            // Создаём вариант
            var labVariant = _context.Create<LabVariant>();
            labVariant.IntroducingVariant = true;
            labVariant.LabWork = lab;
            labVariant.Number = "Debug";
            labVariant.Version = 1;
            labVariant.TaskVariants = new[] {taskVariant};

            return new DebugTaskData
            {
                LabWorkId = lab.Id,
                LabVariantId = labVariant.Id
            };
        }
    }
}