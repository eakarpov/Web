﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Logic.Labs
{
    public sealed class DemoLabEngine : IDemoLabEngine
    {
        private readonly ISystemDateService _systemDateService;
        private readonly ILabRepository _labRepository;

        public DemoLabEngine(
            ISystemDateService systemDateService,
            ILabRepository labRepository)
        {
            Contract.Requires(systemDateService != null);

            _systemDateService = systemDateService;
            _labRepository = labRepository;
        }

        /// <summary> Получить лабораторные работы, у которых сейчас ознакомительный период </summary>
        public LabWork[] GetDemoLabs()
        {
            var currentDate = _systemDateService.Now();
            return _labRepository.GetLabWorks()
                .Where(l => l.AcquaintanceFrom.HasValue && l.AcquaintanceTill.HasValue)
                .Where(l => currentDate.CompareTo(l.AcquaintanceFrom) >= 0 && currentDate.CompareTo(l.AcquaintanceTill) <= 0)
                .ToArray();
        }

        /// <summary> Получить варианты лабораторной работы, доступные для ознакомления </summary>
        public LabVariant[] GetDemoLabVariantsByLabWorkId(long id)
        {
            return _labRepository.GetLabVariantsByLabWorkId(id)
                .Where(lv => lv.IntroducingVariant == true)
                .ToArray();
        }
    }
}
