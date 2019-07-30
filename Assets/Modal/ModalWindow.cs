using System;
public static class ModalWindow
{
   public static string answer = null;
   
    public static string GetAnswer()
    {
        string value = answer;
        ModalWindow.answer = null;
        return ModalWindow.answer;
    }
    
    public static void SetAnswer(string str)
    {
        ModalWindow.answer = str;
    }
}
