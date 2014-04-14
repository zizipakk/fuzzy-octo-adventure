using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tax.Data.Models
{
    public class SurveyAnswerCode
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public String Survey { get; set; }
        [Required]
        public String QuestionCode { get; set; }
        [Required]
        public string AnswerCode { get; set; }
        [Required]
        public string AnswerText { get; set; }

    }

    public class SurveyQuestionCode
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public String Survey { get; set; }
        [Required]
        public String QuestionCode { get; set; }
        [Required]
        public string QuestionText { get; set; }

    }
}
