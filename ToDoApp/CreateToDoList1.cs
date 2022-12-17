﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToDoApp
{
    public class CreateToDoList
    {


        public string ListTitle { get; set; }
        public List<Task> Task { get; set; }
        public string Date { get; set; }
        public bool ThisWeek { get; set; }
        public bool Expired { get; set; }

        public static void CreateNewToDoList(int userIndex)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("\n\nENTER NAME OF LIS OR PRESS 'Q' TO QUIT.\n");
            var listName = Console.ReadLine().ToUpper();
            if (listName == "Q")
            {
                return;
            }
            if (String.IsNullOrWhiteSpace(listName))
            {
                Console.WriteLine("You have to put a name on your list.");
                return;
            }


            var newList = new CreateToDoList()
            {
                ListTitle = listName,
                Task = new List<Task>(),
                Date = DateTime.Now.ToString("G"),
                ThisWeek = false,
                Expired = false,

            };

            json[userIndex].ToDoList.Add(newList);
            CreateUserFile.UpDate(json);

            return;
        }


        public static void ViewAllList(int user)
        {
            var json = CreateUserFile.GetJson();
            Validation.IsThereAnyLists(user);
            int index = -1;
            Console.WriteLine("\n\n\nALL OF YOUR LISTS\n");

            foreach (var title in json[user].ToDoList) { } 
            {
                index++;
                Console.WriteLine("[" + index + "] " + json[user].ToDoList + "\n");
            }
            return;

        }


        public static void DeleteList(int user)
        {

            var json = CreateUserFile.GetJson();
            int num = 0;

            Console.WriteLine("\n\n\nSELECT LIST TO DELETE OR PRESS 'Q' TO QUIT. \n");
            EveryListTitleInJson(user);
            var choosenList = Console.ReadLine().ToLower();
            if (choosenList == "q")
            {
                return;
            }

            bool valid = int.TryParse(choosenList, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }

            bool listExists = Validation.IsThereValidList(num, user);
            if (!listExists)
            {
                return;
            }

            Console.WriteLine("\nDo you want to delete this list? y/n");
            string yesOrNo = Console.ReadLine().ToLower();
            if (yesOrNo == "y")
            {
                Console.WriteLine("LIST DELETED.");
                json.RemoveAt(num);
                CreateUserFile.UpDate(json);
            }
            else if (yesOrNo == "n")
            {
                return;
            }
            else
            {
                Console.WriteLine("Only 'y' or 'n'.");
            }

            return;

        }


        public static void ChangeListName(int user)
        {
            var json = CreateUserFile.GetJson();
            int num = 0;

            Console.WriteLine("\n\n\nSELECT LIST TO RENAME OR PRESS 'Q' TO QUIT. \n");
            EveryListTitleInJson(user);

            var choosenList = Console.ReadLine().ToLower();
            if (choosenList == "q")
            {
                return;
            }
            bool valid = int.TryParse(choosenList, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }
            bool validOrNot = Validation.IsThereValidList(num, user);

            if (!validOrNot)
            {
                return;
            }

            Console.WriteLine("ENTER NEW LISTNAME OR PRESS 'Q' TO QUIT.");
            string newListName = Console.ReadLine().ToUpper();

            if (String.IsNullOrEmpty(newListName))
            {
                Console.WriteLine("You have to put a name on your list.");
                return;
            }
            if (newListName == "Q")
            {
                return;
            }

            json[user].ToDoList[num].ListTitle = newListName;

            CreateUserFile.UpDate(json);
            return;
        }


        
        public static void ViewOneList(int user)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("\n\n\nSELECT LIST TO VIEW PRESS 'Q' TO QUIT.\n");
            EveryListTitleInJson(user);
            var choosenList = Console.ReadLine().ToLower();
            if (choosenList == "q")
            {
                return;
            }

            int num = 0;
            bool valid = int.TryParse(choosenList, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }
            bool validOrNot = Validation.IsThereValidList(num, user);
            if (!validOrNot)
            {
                return;

            }
            Console.WriteLine(json[user].ToDoList[num].ListTitle);


            Validation.IsThereAnyTasks(num, user);
            AddNewTask.EveryTaskInList(num, user);


        }


        public static void RecentList(int user)
        {
            var json = CreateUserFile.GetJson();

            Validation.IsThereAnyLists(user);
            json[user].ToDoList.Reverse();

            Console.WriteLine("\n\n\n" + json[user].ToDoList[0].ListTitle);

            if (json[user].ToDoList[0].Task.Count == 0)
            {
                Console.WriteLine("Ooops.. empty! No to-do's here!");
                return;
            }

            for(int i = 0; i < json[user].ToDoList[0].Task.Count;  i++)
            {
                if (json[user].ToDoList[0].Task[i].Completed == true)
                {

                    Console.ForegroundColor = ConsoleColor.Green;
                }

                if (json[user].ToDoList[0].Task[i].Completed == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine(json[user].ToDoList[0].Task[i].TaskTitle);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return;

        }


        public static void EveryListTitleInJson(int user)
        {
            var json = CreateUserFile.GetJson();
            int index = -1;

            for (int i = 0; i < json[user].ToDoList.Count; i++)
            {
                index++;
                Console.WriteLine(json[user].ToDoList[i].ListTitle + "\nPress: " + "[" + index + "]" + "\n");
            }

        }


        //public static void SortLists(int userIndex)
        //{
        //    Console.WriteLine("HOW DO YOU WANT TO SORT?\n" +
        //        "[N]ewest list\n" +
        //        "[O]ldest list\n" +
        //        "[B]y name\n");
        //    var howToSort = Console.ReadLine().ToLower();
        //    if (string.IsNullOrEmpty(howToSort))
        //    {
        //        Console.WriteLine("Try again.");
        //        return;
        //    }

        //    var json = CreateUserFile.GetJson();


        //    switch (howToSort)
        //    {
        //        case "n":
        //            SortByNewest(userIndex);
        //            break;
        //        //case "o":
        //        //    SortByOldest(userIndex);
        //        //    break;
        //        //case "b":
        //        //    SortByName(userIndex);
        //        //    break;
        //        case "h":
        //            break;
        //        default:
        //            break;

        //    }
        //}

        //public static void SortByNewest(int user)
        //{
        //    var json = CreateUserFile.GetJson();
          
        //    json = json[user].ToDoList.ListTitle.  .OrderBy(x => x.Date).ToString();


        //    CreateUserFile.UpDate(json);
        //    Console.WriteLine("NEW ORDER SAVED.");
        //}


        //public static void SortByOldest(int user)
        //{

        //    var json = CreateUserFile.GetJson();
        //    json = json.OrderByDescending(x => x.Date).ToList();
        //    CreateUserFile.UpDate(json);
        //    Console.WriteLine("NEW ORDER SAVED.");

        //}


        //public static void SortByName(int user)
        //{
        //    var json = CreateUserFile.GetJson();
        //    json = json.OrderBy(x => x.ListTitle).ToList();
        //    CreateUserFile.UpDate(json);
        //    Console.WriteLine("NEW ORDER SAVED.");
        //}


        public static void FinishedLists(int user)
        {
            var json = CreateUserFile.GetJson();

            Console.WriteLine("\n\n\n\nALL OF YOUR FINISED LISTS.\n\n");

            for (int i = 0; i < json[user].ToDoList.Count; i++)
            {
                var allDone = json[user].ToDoList[i].Task.All(x => x.Completed == true);
                
                var orNull = json[user].ToDoList[i].Task.Any();
                if (allDone == true && orNull == true)
                {
                    Console.WriteLine(json[user].ToDoList[i].ListTitle);
                    AddNewTask.EveryTaskInList(i, user);

                }

            }

        }

        public static void AddListToCompleteInAWeek(int user)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("\n\n\n\nWHAT LIST TO ADD TO BE COMPLETED WITHIN A WEEK? PRESS 'Q' TO QUIT.\n\n");

            for (int i = 0; i < json[user].ToDoList.Count; i++)
            {
                if (json[user].ToDoList[i].ThisWeek == false && json[user].ToDoList[i].Expired == false)
                {
                    Console.WriteLine("[" + i + "] " + json[user].ToDoList[i].ListTitle);

                }
            }

            var isThereAnyToMove = json[user].ToDoList.All(x => x.Expired == true);
            if (isThereAnyToMove == true)
            {
                Console.WriteLine("No lists to move :-)");
                return;
            }

            var whichList = Console.ReadLine().ToLower();
            int listToMove = 0;
            if (whichList == "q")
            {
                return;
            }
            bool validOrNot = int.TryParse(whichList, out listToMove);
            if (!validOrNot)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }


            json[user].ToDoList[listToMove].ThisWeek = true;
            CreateUserFile.UpDate(json);
        }



        public static void ShowWeeklyLists(int user)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("\n\n\nYOUR LISTS TO BE COMPLETED IN A WEEK :-)\n");

            for (int i = 0; i < json[user].ToDoList.Count; i++)
            {
                if (json[user].ToDoList[i].ThisWeek == true && json[user].ToDoList[i].Expired == false)
                {
                    bool complete = json[user].ToDoList[i].Task.All(x => x.Completed == true);
                    bool empty = json[user].ToDoList[i].Task.Count == 0;
                    if (!complete || empty)
                    {
                        DateTime start = DateTime.Parse(json[user].ToDoList[i].Date);
                        DateTime expiry = start.AddDays(7);
                        TimeSpan span = expiry - DateTime.Now;
                        Console.WriteLine("\n\n" + json[user].ToDoList[i].ListTitle + "\n*" + span.Days + " days left to complete *");
                    }

                }
            }


            var noLists = json[user].ToDoList.All(x => x.ThisWeek == false);
            if (noLists == true)
            {
                Console.WriteLine("No lists added yet!");
                return;
            }
        }


        public static void UnFinishedLists(int user)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("\n\n\n- EXPIRED AND UNFINISED LISTS - \n");

            for (int i = 0; i < json[user].ToDoList.Count; i++)
            {
                DateTime start = DateTime.Parse(json[user].ToDoList[i].Date);
                DateTime expiry = start.AddDays(7);
                TimeSpan span = start - expiry;

                bool allCompleted = json[user].ToDoList[i].Task.All(x => x.Completed == true);

                if (DateTime.Now > expiry)
                {
                    json[user].ToDoList[i].Expired = true;
                    json[user].ToDoList[i].ThisWeek = false;
                    CreateUserFile.UpDate(json);

                    if (!allCompleted || json[user].ToDoList[i].Task.Count == 0)
                    {
                        Console.WriteLine(json[user].ToDoList[i].ListTitle);
                    }
                }

            }

            var noLists = json[user].ToDoList.All(x => x.Expired == false);
            if (noLists == true)
            {
                Console.WriteLine("No expired lists :-)");
                return;
            }

        }














    }
}