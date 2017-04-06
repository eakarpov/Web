﻿using GraphLabs.Dal.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
    public class SurveyCreatingModel : BaseViewModel
    {
        #region Зависимости

        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;

        #endregion


        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать текст вопроса!")]
        [MinLength(3, ErrorMessage = "Текст вопроса слишком короткий!")]
        [MaxLength(3000, ErrorMessage = "Текст вопроса слишком длинный!")]
        public string Question { get; set; }

        public long CategoryId { get; set; }

        public List<SelectListItem> CategoryList
        { 
            get 
            { 
            return _categoryRepository.GetAllCategories().Select(
                cat => new SelectListItem
                {
                    Value = cat.Id.ToString(),
                    Text = cat.Name
                }
                ).ToList();
            }
        }
		static public bool IsValid(string question)
		{

                //длина вопроса от 3 до 3000 символов
		    return !((question.Length < 3) || (question.Length > 3000));

                ////пусть количество ответов от 2 до 20
                //if ((this.QuestionOptions.Count < 2) || (this.QuestionOptions.Count > 20))
                //    return false;

                ////проверка корректности ответов
                //var correctCount = 0;
                //foreach (KeyValuePair<String, bool> answer in this.QuestionOptions)
                //{
                //    //сичтаем корректные ответы
                //    if (answer.Value)
                //        ++correctCount;
                //    //длина ответа от 1 до 300 символов
                //    if ((answer.Key.Length < 1) || (answer.Key.Length > 3000))
                //        return false;
                //}

                ////не выбрано ни одного верного ответа
                //if (correctCount == 0)
                //    return false;
		}

		public SurveyCreatingModel(
            ISurveyRepository surveyRepository, 
            ICategoryRepository categoryRepository)
		{
		    _surveyRepository = surveyRepository;
		    _categoryRepository = categoryRepository;
		}

        public void Save()
		{
            _surveyRepository.SaveQuestion(
                this.Question,
                new Dictionary<string, bool>(), 
                CategoryId);
		}
        //репозиторий если есть такой вопрос, то обновить
        //если нет, то сохранить
        //также может быть левое
    }
}