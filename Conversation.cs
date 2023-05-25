using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;

namespace DialogueSystem;

public class Conversation
{
    /// <summary>
    /// Number of positive questions asked by the player. The starting value is 0.
    /// </summary>
    public int numberOfPositive { get; private set; }
    /// <summary>
    /// Number of negative questions asked by the player. The starting value is 0.
    /// </summary>
    public int numberOfNegative { get; private set; }
    /// <summary>
    /// Number of neutral questions asked by the player. The starting value is 0.
    /// </summary>
    public int numberOfNeutral { get; private set; }
    /// <summary>
    /// List of question pools. This will be looped through in the Run() method.
    /// </summary>
    public List<QuestionPool> dialouge { get; set; }
    
    private static Keys[] validKeys = new[]
    {
        Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9
    };
    
    private static Keys[] numpadKeys = new[]
    {
        Keys.NumPad0,Keys.NumPad1,Keys.NumPad2,Keys.NumPad3,Keys.NumPad4,Keys.NumPad5,Keys.NumPad6,Keys.NumPad7,Keys.NumPad8,Keys.NumPad9
    };
    
    /// <summary>
    /// Initializes an instance of the Conversation object 
    /// </summary>
    /// <param name="dialouge">This is the dialogue that you want to take place. It has to be a <code>List<QuestionPool></code>.</param>
    /// <param name="useNumpadKeys">This is a boolean to either use numpad keys or not. This could be part of an ini setting.</param>
    public Conversation(List<QuestionPool> dialouge,bool useNumpadKeys)
    {
        this.dialouge = dialouge;
        numberOfNegative = 0;
        numberOfNeutral = 0;
        numberOfPositive = 0;
        if (useNumpadKeys)
        {
            validKeys = numpadKeys;
        }
    }
    
    private void UpdateNumbers(QuestionEffect effect)
    {
        switch (effect)
        {
            case QuestionEffect.POSITIVE:
                numberOfPositive++;
                break;
            case QuestionEffect.NEUTRAL:
                numberOfNeutral++;
                break;
            case QuestionEffect.NEGATIVE:
                numberOfNegative++;
                break;
        }
    }
    
    private int WaitForValidKeyPress(QuestionPool q)
    {
        bool isValidKeyPressed = false;
        int indexPressed = 0;
        while (!isValidKeyPressed)
        {
            GameFiber.Yield();
            for (int i = 0; i < validKeys.Length; i++)
            {
                Keys key = validKeys[i];
                if (Game.IsKeyDown(key) && q.IsValidIndex(i))
                {
                    isValidKeyPressed = true;
                    indexPressed = i;
                }
            }
        }
        return indexPressed;
    }
    
    /// <summary>
    /// This method runs the dialogue in a new GameFiber. This method will iterate through all your question pools and updates the number of question integers for each category.
    /// </summary>
    public void Run()
    {
        GameFiber.StartNew(delegate
        {
            foreach (QuestionPool q in dialouge)
            {
                Game.DisplayHelp(q.DisplayQuestions(),10000);
                int indexPressed = WaitForValidKeyPress(q);
                Game.HideHelp();
                Game.DisplaySubtitle(q.GetAnswer(indexPressed));
                UpdateNumbers(q.GetEffect(indexPressed));
            }
        });
    }
    
    
    /// <summary>
    /// This method will add all your questions from a question pool to the menu specified.
    /// </summary>
    /// <param name="menu">Menu you want to add the question to</param>
    /// <param name="q">Question pool the questions will be grabbed from</param>
    public void AddQuestionsToMenu(UIMenu menu, QuestionPool q)
    {
        foreach (QuestionAndAnswer qanda in q.pool)
        {
            menu.AddItem(new UIMenuItem(qanda.question));
        }
    }
    
    /// <summary>
    /// This method can be ran during the OnItemSelect event in RageNativeUI. This will display the answer to the question clicked and update the number of question integers accordingly.
    /// </summary>
    /// <param name="index">Index of menu button selected</param>
    /// <param name="q">Question pool that the question was asked from</param>
    public void OnItemSelect(int index, QuestionPool q)
    {
        Game.DisplaySubtitle(q.GetAnswer(index));
        UpdateNumbers(q.GetEffect(index));
    }
}