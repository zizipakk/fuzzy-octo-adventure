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
                    int i = 0;
                    StringBuilder str = new StringBuilder(searchString);
                    foreach (char c in searchString.ToLower())
                    {
                        switch (c)
                        {
                            case 'a':
                                str[i] = 'á';
                                searchStringList.Add(str.ToString());
                                break;
                            case 'e':
                                str[i] = 'é';
                                searchStringList.Add(str.ToString());
                                break;
                            case 'i':
                                str[i] = 'í';
                                searchStringList.Add(str.ToString());
                                break;
                            case 'o':
                                str[i] = 'ó';
                                searchStringList.Add(str.ToString());
                                str[i] = 'ö';
                                searchStringList.Add(str.ToString());
                                str[i] = 'ő';
                                searchStringList.Add(str.ToString());
                                break;
                            case 'u':
                                str[i] = 'ú';
                                searchStringList.Add(str.ToString());
                                str[i] = 'ü';
                                searchStringList.Add(str.ToString());
                                str[i] = 'ű';
                                searchStringList.Add(str.ToString());
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