using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace A1.Helper
{
    public class ProcessText
    {
        public static string Filter(string s)
        {
            string[] subStrings = s.Split(',');//split the string
            for (int i = 0; i < subStrings.Length; i++)// loop through the elements
            {
                subStrings[i] = subStrings[i].Trim();// trip each elemetn so that there is no leading or trailing empty spaces
            }
            StringBuilder newString = new StringBuilder();//create a new string 
            foreach (string x in subStrings)
            {
                newString.Append(x).Append(",");//append each element onto this new string, with a comma 
            }
            newString.Remove(newString.Length - 1, 1);// but there is an extra comma at the end, so remove
                                                      // it using the remove method with the index to removve
                                                      // and the number of elements to remove is 1
            return newString.ToString();// return the new string
        }
    }
}
