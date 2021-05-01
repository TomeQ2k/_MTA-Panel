using System;

namespace MTA.Core.Application.Dtos
{
    public class ApplicationDto
    {
        public int Id { get; protected set; }
        public int ApplicantId { get; protected set; }
        public DateTime DateCreated { get; protected set; }
        public DateTime DateReviewed { get; protected set; }
        public int ReviewerId { get; protected set; }
        public string Note { get; protected set; }
        public int State { get; protected set; }
        public string Question1 { get; protected set; }
        public string Question2 { get; protected set; }
        public string Question3 { get; protected set; }
        public string Question4 { get; protected set; }
        public string Answer1 { get; protected set; }
        public string Answer2 { get; protected set; }
        public string Answer3 { get; protected set; }
        public string Answer4 { get; protected set; }
    }
}