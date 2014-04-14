using Tax.Data.Models;
using Tax.WebAPI.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tax.WebAPI.Service
{
    public class SurveyService
    {
        ApplicationDbContext context;

        public SurveyService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void SaveSurvey(Survey survey, string username)
        {
            var user = UserQueries.GetUserByName(context, username);

            survey.Timestamp = DateTime.Now;
            survey.ApplicationUser = user;

            context.Surveys.Add(survey);
        }
    }
}