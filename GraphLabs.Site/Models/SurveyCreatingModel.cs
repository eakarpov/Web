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
    public class SurveyCreatingModel : BaseViewModel
    {
        #region Зависимости
        private ISurveyRepository _surveyRepository
        {
            get { return DependencyResolver.GetService<ISurveyRepository>(); }
        }       
        private ICategoryRepository _categoryRepository
        {
            get { return DependencyResolver.GetService<ICategoryRepository>(); }
        }
        #endregion

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать текст вопроса!")]
        [MinLength(3, ErrorMessage = "Текст вопроса слишком короткий!")]
        [MaxLength(3000, ErrorMessage = "Текст вопроса слишком длинный!")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Укажите варианты ответа")]
        public List<KeyValuePair<String, bool>> QuestionOptions { get; set; }

        public string CategoryId { get; set; }

        private long _categoryId;

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
		public bool IsValid
		{
			get
			{
                //длина вопроса от 3 до 3000 символов
                if ((this.Question.Length < 3) || (this.Question.Length > 3000))
                    return false;

                //пусть количество ответов от 2 до 20
                if ((this.QuestionOptions.Count < 2) || (this.QuestionOptions.Count > 20))
                    return false;

                //проверка корректности ответов
                var correctCount = 0;
                foreach (KeyValuePair<String, bool> answer in this.QuestionOptions)
                {
                    //сичтаем корректные ответы
                    if (answer.Value)
                        ++correctCount;
                    //длина ответа от 1 до 300 символов
                    if ((answer.Key.Length < 1) || (answer.Key.Length > 3000))
                        return false;
                }

                //не выбрано ни одного верного ответа
                if (correctCount == 0)
                    return false;

                //все проверки пройдены
                return true;
			}
		}

		public SurveyCreatingModel()
		{
			QuestionOptions = new List<KeyValuePair<String, bool>>();
		}

		public SurveyCreatingModel(string question, Dictionary<string, bool> questionOptions, long categoryId)
		{
			Question = question;
			QuestionOptions = questionOptions.ToList();
            _categoryId = categoryId;
		}

		public void Save()
		{
            _surveyRepository.SaveQuestion(
                this.Question,
                this.QuestionOptions.ToDictionary(qo => qo.Key, qo => qo.Value),
                _categoryId);
		}
        //репозиторий если есть такой вопрос, то обновить
        //если нет, то сохранить
        //также может быть левое
    }
}