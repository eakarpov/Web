//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraphLabs.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class LabEntry
    {
        public long Id { get; private set; }
        public int Order { get; set; }
    
        public virtual LabWork LabWork { get; set; }
        public virtual Task Task { get; set; }
    }
}
