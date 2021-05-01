using System;
using System.Collections.Generic;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Application : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("applicant")] public int ApplicantId { get; protected set; }
        [Column("dateposted")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("datereviewed")] public DateTime DateReviewed { get; protected set; }
        [Column("reviewer")] public int ReviewerId { get; protected set; }
        [Column("note")] public string Note { get; protected set; }
        [Column("state")] public int State { get; protected set; }
        [Column("question1")] public string Question1 { get; protected set; }
        [Column("question2")] public string Question2 { get; protected set; }
        [Column("question3")] public string Question3 { get; protected set; }
        [Column("question4")] public string Question4 { get; protected set; }
        [Column("answer1")] public string Answer1 { get; protected set; }
        [Column("answer2")] public string Answer2 { get; protected set; }
        [Column("answer3")] public string Answer3 { get; protected set; }
        [Column("answer4")] public string Answer4 { get; protected set; }

        public User Applicant { get; protected set; }

        public static Application Create(int applicantId) => new Application {ApplicantId = applicantId};

        public void SetQuestionsAndAnswers(List<(string, string)> questionsAnswers)
        {
            (Question1, Answer1) = questionsAnswers[0];
            (Question2, Answer2) = questionsAnswers[1];
            (Question3, Answer3) = questionsAnswers[2];
            (Question4, Answer4) = questionsAnswers[3];
        }

        public void SetReviewDetails(int reviewerId, string note, ApplicationStateType stateType) =>
            (ReviewerId, Note, State, DateReviewed) = (reviewerId, note, (int) stateType, DateTime.Now);

        public void SetApplicant(User applicant) => Applicant = applicant;
    }
}