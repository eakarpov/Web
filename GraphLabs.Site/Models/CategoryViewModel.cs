﻿using GraphLabs.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
    public class CategoryViewModel : BaseViewModel
	{
        #region Зависимости

        private ICategoryRepository _categoriesRepository
        {
            get { return DependencyResolver.GetService<ICategoryRepository>(); }
        }

        #endregion

		public long Id { get; set; }

        [Required(ErrorMessage = "Укажите категорию")]
		public string Name { get; set; }

		public CategoryViewModel()
		{
		}

		public CategoryViewModel(long id)
		{
			var category = _categoriesRepository.GetById(id);
			Id = category.Id;
			Name = category.Name;
		}

        public void Save()
        {
			if (Id == default(int))
			{
				var category = new Category
				{
					Name = this.Name
				};
				_categoriesRepository.SaveCategory(category);
			}
			else
			{
				var category = _categoriesRepository.GetById(Id);
				category.Name = Name;
				_categoriesRepository.EditCategory(category);
			}
        }
	}
}