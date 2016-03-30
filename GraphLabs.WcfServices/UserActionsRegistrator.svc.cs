﻿using System;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UserActionsRegistrator : IUserActionsRegistrator
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;
        private readonly ISystemDateService _systemDate;

        /// <summary> Начальный балл </summary>
        private const int StartingScore = 100;

        /// <summary> Сервис предоставления данных модулям заданий </summary>
        public UserActionsRegistrator(IOperationContextFactory<IGraphLabsContext> operationFactory,
            ISystemDateService systemDate)
        {
            _operationFactory = operationFactory;
            _systemDate = systemDate;
        }

        /// <summary> Регистрирует действия студента </summary>
        /// <param name="taskId"> Идентификатор модуля-задания </param>
        /// <param name="sessionGuid"> Идентификатор сессии </param>
        /// <param name="actions"> Действия для регистрации </param>
        /// <param name="isTaskFinished"> Задание завершено? </param>
        /// <returns> Количество баллов студента </returns>
        /// <remarks> От этой штуки зависит GraphLabs.Components </remarks>
        public int RegisterUserActions(long taskId, Guid sessionGuid, ActionDescription[] actions, bool isTaskFinished = false)
        {
            using (var op = _operationFactory.Create())
            {
                var task = op.DataContext.Query.Get<Task>(taskId);
                var session = GetSessionWithChecks(op.DataContext.Query, sessionGuid);
                var resultLog = GetCurrentResultLog(op.DataContext.Query, session);

                foreach (var actionDescription in actions)
                {
                    var newAction = op.DataContext.Factory.Create<StudentAction>();
                    newAction.Description = actionDescription.Description;
                    newAction.Penalty = actionDescription.Penalty;
                    newAction.Result = resultLog;
                    newAction.Time = actionDescription.TimeStamp;
                    newAction.Task = task;
                    resultLog.Actions.Add(newAction);
                }

                if (isTaskFinished)
                {
                    var newAction = op.DataContext.Factory.Create<StudentAction>();
                    newAction.Description = $"Задание {task.Name} выполнено.";
                    newAction.Penalty = 0;
                    newAction.Result = resultLog;
                    newAction.Time = _systemDate.Now();
                    newAction.Task = task;
                    resultLog.Actions.Add(newAction);
                }

                var currentScore = CalculateCurrentScore(resultLog);

                op.Complete();

                return currentScore;
            }
        }

        private int CalculateCurrentScore(Result result)
        {
            return StartingScore - result.Actions.Sum(a => a.Penalty);
        }

        private Result GetCurrentResultLog(IEntityQuery query, Session session)
        {
            var activeResults = query.OfEntities<Result>()
                .Where(result => result.Student.Id == session.User.Id && result.Grade == null)
                .ToArray();

            foreach (var activeResult in activeResults.OrderByDescending(r => r.StartDateTime).Skip(1))
            {
                activeResult.Grade = Grade.Interrupted;
            }

            return activeResults.First();
        }

        private Session GetSessionWithChecks(IEntityQuery query, Guid sessionGuid)
        {
            var session = query.Get<Session>(sessionGuid);

            //TODO +проверка контрольной суммы и тп - всё надо куда-то в Security вытащить
            if (session.IP != HttpContext.Current.Request.UserHostAddress)
            {
                throw new EntityNotFoundException(typeof(Session), new object[] { sessionGuid });
            }

            return session;
        }
    }
}