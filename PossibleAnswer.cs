using System;
using System.Collections.Generic;
using Rage;

namespace DialogueSystem;

public class PossibleAnswer
{
    public string Answer { get; set; }
    public Predicate<Ped>? Condition { get; set; }
    public Action<Ped>? PerformActionIfChosen{ get; set; }
    public List<string>? RemoveThoseQuestionsIfChosen{ get; set; }
    public Ped? Ped { get; set; }
    public bool EndsConversation { get; set; }

    public PossibleAnswer(string answer, bool EndsConversation)
    {
        Answer = answer;
        this.EndsConversation = EndsConversation;
        Condition = null;
        Ped = null;
        PerformActionIfChosen = null;
        RemoveThoseQuestionsIfChosen = new List<string>();
    }
    public PossibleAnswer(string answer, bool EndsConversation, Predicate<Ped> Condition, Ped Ped)
    {
        Answer = answer;
        this.EndsConversation = EndsConversation;
        this.Condition = Condition;
        this.Ped = Ped;
        PerformActionIfChosen = null;
        RemoveThoseQuestionsIfChosen = null;
    }
    public PossibleAnswer(string answer, bool EndsConversation, Predicate<Ped> Condition, Ped Ped, Action<Ped> PerformActionIfChosen)
    {
        Answer = answer;
        this.EndsConversation = EndsConversation;
        this.Condition = Condition;
        this.Ped = Ped;
        this.PerformActionIfChosen = PerformActionIfChosen;
        RemoveThoseQuestionsIfChosen = new List<string>();
    }
    public PossibleAnswer(string answer, bool EndsConversation, Predicate<Ped> Condition, Ped Ped, Action<Ped> PerformActionIfChosen, List<string> RemoveThoseQuestionsIfChosen)
    {
        Answer = answer;
        this.EndsConversation = EndsConversation;
        this.Condition = Condition;
        this.Ped = Ped;
        this.PerformActionIfChosen = PerformActionIfChosen;
        this.RemoveThoseQuestionsIfChosen = RemoveThoseQuestionsIfChosen;
    }
    
}