using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tax.WebAPI.Helpers
{
    public class SearchHelpers
    {
        public static List<string> GetSaerchList(string searchString, string lang)
        {
            var searchStringList = new List<string>();
            searchStringList.Add(searchString);
            if (Regex.Match(lang, "hu", RegexOptions.IgnoreCase).Success)
            {
                var regex = new Regex("[aAeEiIoOuU]");
                if (regex.IsMatch(searchString))
                {
                    StringBuilder strb;
                    List<string> tempL;
                    int i = 0;                    
                    foreach (char c in searchString.ToLower())
                    {
                        switch (c)
                        {
                            case 'a':
                                tempL = new List<string>();
                                foreach (string str in searchStringList)
                                {
                                    strb = new StringBuilder(str);
                                    strb[i] = 'á';
                                    tempL.Add(strb.ToString());
                                }
                                searchStringList.AddRange(tempL);
                                break;
                            case 'e':
                                tempL = new List<string>();
                                foreach (string str in searchStringList)
                                {
                                    strb = new StringBuilder(str);
                                    strb[i] = 'é';
                                    tempL.Add(strb.ToString());
                                }
                                searchStringList.AddRange(tempL);
                                break;
                            case 'i':
                                tempL = new List<string>();
                                foreach (string str in searchStringList)
                                {
                                    strb = new StringBuilder(str);
                                    tempL = new List<string>();
                                    strb[i] = 'í';
                                    tempL.Add(strb.ToString());
                                }
                                searchStringList.AddRange(tempL);
                                break;
                            case 'o':
                                tempL = new List<string>();
                                foreach (string str in searchStringList)
                                {
                                    strb = new StringBuilder(str);
                                    strb[i] = 'ó';
                                    tempL.Add(strb.ToString());
                                    strb[i] = 'ö';
                                    tempL.Add(strb.ToString());
                                    strb[i] = 'ő';
                                    tempL.Add(strb.ToString());
                                }
                                searchStringList.AddRange(tempL);
                                break;
                            case 'u':
                                tempL = new List<string>();
                                foreach (string str in searchStringList)
                                {
                                    strb = new StringBuilder(str);
                                    strb[i] = 'ú';
                                    tempL.Add(strb.ToString());
                                    strb[i] = 'ü';
                                    tempL.Add(strb.ToString());
                                    strb[i] = 'ű';
                                    tempL.Add(strb.ToString());
                                }
                                searchStringList.AddRange(tempL);
                                break;
                            default:
                                break;
                        }
                        i += 1;
                    }
                }
            }
            return searchStringList;
        }
    }
}