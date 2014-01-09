﻿using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace GraphLabs.DomainModel
{
    /// <summary> Вариант задания </summary>
    public partial class TaskVariant : AbstractEntity
    {
        /// <summary> Валидация </summary>
        public override IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry)
        {
            if (string.IsNullOrWhiteSpace(Number))
                yield return new DbValidationError("Number", ValidationErrors.TaskVariant_OnValidating_Необходимо_указать_номер_варианта_);
            if (string.IsNullOrWhiteSpace(GeneratorVersion))
                yield return new DbValidationError("GeneratorVersion", ValidationErrors.TaskVariant_OnValidating_Необходимо_указать_версию_генератора_варианта_);
            if (Data == null || !Data.Any())
                yield return new DbValidationError("Data", ValidationErrors.TaskVariant_OnValidating_Должны_быть_указаны_данные_варианта_);
            if (Version <= 0)
                yield return new DbValidationError("Version", ValidationErrors.TaskVariant_OnValidating_Версия_должна_быть_больше_0_);
        }
    }
}
