﻿using System;

namespace GraphLabs.Dal.Ef
{
    /// <summary> Менеджер изменение </summary>
    [Obsolete("Заменить на TransactionScope")]
    public interface IChangesTracker
    {
        /// <summary> Сохранить все изменения </summary>
        void SaveChanges();

        /// <summary> Проверяет, что изменений нет. В противном случае бросает исключение </summary>
        void CheckHasNoChanges();

        /// <summary> Откатить имеющиеся изменения </summary>
        void DiscardChanges();
    }
}