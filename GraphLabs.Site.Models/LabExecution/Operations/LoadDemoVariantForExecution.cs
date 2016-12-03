using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Tasks.Contract;

namespace GraphLabs.Site.Models.LabExecution.Operations
{
    internal class LoadDemoVariantForExecution : LoadVariantForExecutionBase
    {
        public LoadDemoVariantForExecution(
            IOperationContextFactory<IGraphLabsContext> operationFactory,
            IAuthenticationSavingService authService,
            IInitParamsProvider initParamsProvider, 
            TaskExecutionModelLoader taskModelLoader) 
            : base(operationFactory, authService, initParamsProvider, taskModelLoader)
        {
        }

        public VariantExecutionModelBase Load(long labVariantId, int? taskIndex, Uri taskCompleteRedirect)
        {
            var variant = Query.Get<LabVariant>(labVariantId);
            if (!variant.IntroducingVariant)
            {
                throw new Exception("Запрошенный вариант не предназначен для ознакомительного решения.");
            }

            var model = LoadImpl(variant, taskIndex, taskCompleteRedirect);

            Complete();

            return model;
        }
    }
}