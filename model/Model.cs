using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace WebApplication1.Models
{
    public class Model
    {
        DAO database;
        public Model() { database = new MySqlData(); }
        //使用者
        public List<UserInfo> searchAllUsers()
        {
            return database.searchAllUsers();
        }
        public UserInfo searchUser(string name)
        {
            return database.searchUser(name);
        }
        public void addUser(UserInfo newUser)
        {
            database.AddUser(newUser);
        }

        //看板管理
        public Board searchBoard(string name)
        {
            return database.searchBoard(name);
        }
        public void addBoard(Board newBoard)
        {
            database.addBoard(newBoard);
        }
        public void deleteBoard(Board board)
        {
            database.deleteBoard(board);
        }
        public void setBoardManager(ref Board board, string userName)
        {
            board.manager = userName;
            database.setBoardManager(board, userName);
        }

        //文章管理
        public void addArticle(ref Board board, Article article)
        {
            board.article.Add(article);
            database.addArticle(board, article);
        }
        public void addComment(ref Board board, ref Article article, string comment)
        {
            article.comments.Add(comment);
            database.addComment(board, article, comment);
        }
        public void newlikes(ref Board board, ref Article article)
        {
            article.likes++;
            database.newlikes(board, article);
        }


        //好友確認
        public void applyFriend(UserInfo applicant, string friendName)
        {
            database.applyFriend(applicant, friendName);
        }
        public void confirmFriend(ref UserInfo confirmer, string friendName)
        {
            confirmer.friendApply.RemoveAll(item=>item==friendName);
            confirmer.myFriend.Add(friendName);
            database.confirmFriend(confirmer, friendName);
        }

       
    }
    public class Article
    {
        public string author;
        public string title;
        public string content;
        public string time;
        public int likes;
        public List<string> comments;
        public Article(string t, string c,string name)
        {
            author = name;
            title = t;content=c;
            time= DateTime.Now.ToString(("yyyy-MM-dd-hh:mm:ss"));
            likes = 0;
            comments = new List<string>();
            //存到database
        }
        public Article() { }
       
    }
    public class Board
    {
        public string Name;
        public string manager;
        public List<Article> article;
        public Board(string name, string managerName)
        {
            Name = name;
            manager = managerName;
        }
        public Board() { }
    }
    public class UserInfo
    {
        public string Name;
        public string Department;
        public string StudentID;
        public string Password;
        public int coin;
        public bool isManager;
        public List<string> myBoard;
        public List<string> myFriend;
        public List<string> friendApply;
        public UserInfo(string name, string department, string id, string password)
        {
            Name = name; Department = department; StudentID = id; Password = password;
            coin = 0;
            isManager = false;
            myBoard = new List<string>();
            myFriend = new List<string>();
            friendApply = new List<string>();
        }
        public UserInfo() { }
    }
    public class Manager
    {
        UserInfo user;
        public List<string> BoardApply;
        public List<int> adPrice;
        public Manager()
        {
            BoardApply = new List<string>();
            adPrice = new List<int>();
        }

    }
    public abstract class DAO
    {
        abstract public List<string> getUserFriend(string name);
        abstract public List<string> getUserBoard(string name);
        abstract public List<string> getUserApplicant(string name);
        abstract public List<string> getArticleComment(string name);
        abstract public List<Article> getBoardArticle(string name);
        abstract public List<UserInfo> searchAllUsers();
        abstract public UserInfo searchUser(string name);
        abstract public void AddUser(UserInfo user);
        abstract public Board searchBoard(string name);
        abstract public void applyFriend(UserInfo applicant, string friendName);
        abstract public void confirmFriend(UserInfo confirmer, string friendName);
        abstract public void addBoard(Board newBoard);
        abstract public void deleteBoard(Board board);
        abstract public void addArticle(Board board, Article article);
        abstract public void addComment(Board board, Article article, string comment);
        abstract public void newlikes(Board board, Article article);
        abstract public void setBoardManager(Board board, string userName);
    }
    public class H2Data : DAO
    {
        override public List<string> getUserFriend(string name) { return new List<string>(); }
        override public List<string> getUserBoard(string name) { return new List<string>(); }
        override public List<string> getUserApplicant(string name) { return new List<string>(); }
        override public List<string> getArticleComment(string name) { return new List<string>(); }
        override public List<Article> getBoardArticle(string name) { return new List<Article>(); }
        override public List<UserInfo> searchAllUsers() { return new List<UserInfo>(); }
        override public UserInfo searchUser(string name)
        {
            return new UserInfo();
        }
        override public void  AddUser(UserInfo user) { }
        override public Board searchBoard(string name)
        {
            return new Board();
        }
        override public void applyFriend(UserInfo applicant, string friendName) { }
        override public void confirmFriend(UserInfo confirmer, string friendName) { }
        override public void addBoard(Board newBoard) { }
        override public void deleteBoard(Board board) { }
        override public void addArticle(Board board, Article article) { }
        override public void addComment(Board board, Article article, string comment) { }
        override public void newlikes(Board board, Article article) { }
        override public void setBoardManager(Board board, string userName) { }
    }
    public class MySqlData : DAO
    {
        /*string dbHost = "";//資料庫位址
        string dbUser = "";//資料庫使用者帳號
        string dbPass = "";//資料庫使用者密碼
        string dbName = "";//資料庫名稱*/
        MySqlConnection Connection;
        MySqlCommand command;
        MySqlDataReader reader;
        public MySqlData()
        {
            string connStr = "server=127.0.0.1;port=3306;user id=root;password=abc1234567;database=mydb;charset=utf8;";
            Connection = new MySqlConnection(connStr);
            command = Connection.CreateCommand();
            Connection.Open();
            command.CommandText = "Create table if not exists userinfo(username longtext, department longtext, ID longtext,userpassword longtext,  coin int,isManager boolean);";
            command.ExecuteNonQuery();
            command.CommandText = "Create table if not exists Board(name longtext,manager longtext)";
            command.ExecuteNonQuery();
            // Console.ReadLine();
        }
        ~MySqlData()
        {
            Connection.Close();
        }
        override public List<string> getUserFriend(string name)
        {
            List<string> Return = new List<string>();
            command.CommandText = "select * from " + name + "Friend";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmp = reader.GetString(0);
                Return.Add(tmp);
            }
            reader.Close();
            return Return;
        }
        override public List<string> getUserBoard(string name)
        {
            List<string> Return = new List<string>();
            command.CommandText = "select * from " + name + "Board";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmp = reader.GetString(0);
                Return.Add(tmp);
            }
            reader.Close();
            return Return;
        }
        override public List<string> getUserApplicant(string name)
        {
            List<string> Return = new List<string>();
            command.CommandText = "select * from " + name + "Applicant";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmp = reader.GetString(0);
                Return.Add(tmp);
            }
            reader.Close();
            return Return;
        }
        override public List<string> getArticleComment(string name)
        {
            List<string> Return = new List<string>();
            command.CommandText = "select * from " + name + "Comment";
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                string tmp = reader.GetString(0);
                Return.Add(tmp);
            }
            return Return;
        }
        override public List<Article> getBoardArticle(string name)
        {
            List<Article> Return = new List<Article>();
            Article tmp = new Article();
            command.CommandText = "select * from "+name+"Article";
            reader = command.ExecuteReader();
            List<string> temp = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                reader.Read();
                temp.Add(reader.GetString(i));
            }
            while (reader.Read())
            {
                tmp = new Article(temp[0], temp[1], temp[2]);
                tmp.time = temp[3];
                reader.Read();
                tmp.likes = reader.GetInt32(4);
                reader.Close();
                tmp.comments = getArticleComment(tmp.title);
                Return.Add(tmp);
            }
            return Return;
        }
        override public List<UserInfo> searchAllUsers()
        {
            List<UserInfo> returnValue=new List<UserInfo>();
            UserInfo tmp = new UserInfo();
            command.CommandText = "select * from userinfo";
            reader= command.ExecuteReader();
            bool end = false;
            while (true)
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < 4; i++)
                {
                    if (!reader.Read()) { end = true;break; }
                    temp.Add(reader.GetString(i));
                }
                if (end) break;
                tmp = new UserInfo(temp[0], temp[1], temp[2], temp[3]);
                reader.Read();
                tmp.coin = reader.GetInt32(4);
                reader.Read();
                tmp.isManager = reader.GetBoolean(5);
                returnValue.Add(tmp);
            }
            reader.Close();
            for (int i = 0; i < returnValue.Count; i++)
            {
                returnValue[i].friendApply = getUserApplicant(returnValue[i].Name);
                returnValue[i].myBoard = getUserBoard(returnValue[i].Name);
                returnValue[i].myFriend = getUserFriend(returnValue[i].Name);
            }
            return returnValue;
        }
        override public UserInfo searchUser(string name)
        {
            UserInfo returnValue;
            command.CommandText= "select * from userinfo where username = '"+name+"'";
            List<string> tmp = new List<string>();
            reader = command.ExecuteReader(); //execure the reader
            for (int i = 0; i < 4; i++)
            {
                reader.Read();
                tmp.Add(reader.GetString(i));
            }
            returnValue=new UserInfo(tmp[0], tmp[1], tmp[2], tmp[3]);
            reader.Read();
            returnValue.coin = reader.GetInt32(4);
            reader.Read();
            returnValue.isManager = reader.GetBoolean(5);
            reader.Close();
            returnValue.friendApply = getUserApplicant(name);
            returnValue.myFriend = getUserFriend(name);
            returnValue.myBoard = getUserBoard(name);
            return returnValue;
        }
        override public void AddUser(UserInfo user)
        {
            command.CommandText= "Insert into userinfo(username,department,ID,userpassword,coin,isManager) values('" + user.Name + "','" + user.Department + "','" + user.StudentID + "','"+user.Password+"',"+user.coin+","+user.isManager + ")";
            command.ExecuteNonQuery();
            command.CommandText = "Create table if not exists " + user.Name + "Friend(friendname longtext)";
            command.ExecuteNonQuery();
            command.CommandText= "Create table if not exists " + user.Name + "Board(board longtext)";
            command.ExecuteNonQuery();
            command.CommandText= "Create table if not exists " + user.Name + "Applicant(applicant longtext)";
            command.ExecuteNonQuery();
            //創建個人動態頁
            command.CommandText = "Insert into Board(name,manager) values ('" + user.Name + "','" + user.Name + "')";
            command.ExecuteNonQuery();
            command.CommandText = "Insert into " + user.Name + "Board(board) values ('" + user.Name + "')";
            command.ExecuteNonQuery();
        }
        override public Board searchBoard(string name)
        {
            command.CommandText = "Select * from Board where name='" + name + "'";
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string Name = reader.GetString(0);
            reader.Read();
            string manager = reader.GetString(1);
            Board Return = new Board(Name, manager);
            Return.article = getBoardArticle(Return.Name);
            return Return;
        }
        override public void applyFriend(UserInfo applicant, string friendName)
        {
            command.CommandText = "Insert into " + friendName + "Applicant(appicant) values ('" + applicant.Name + "')";
            command.ExecuteNonQuery();
        }
        override public void confirmFriend(UserInfo confirmer, string friendName)
        {
            command.CommandText = "Insert into " + confirmer.Name + "Friend(friendname) values ('" + friendName + "')";
            command.ExecuteNonQuery();
            command.CommandText="Insert into "+ friendName + "Friend(friendname) values ('" + confirmer.Name + "')";
            command.ExecuteNonQuery();
            command.CommandText = "Delete from " + confirmer.Name + "Applicant where applicant='" + friendName + "'";
            command.ExecuteNonQuery();
        }
        override public void addBoard(Board newBoard)
        {
            command.CommandText = "Create table if not exists " + newBoard.Name + "Article(title longtext,content longtext,author longtext,time longtext,likes int)";
            command.ExecuteNonQuery();
            command.CommandText = "Insert into Board(name,manager) values ('" + newBoard.Name + "','" + newBoard.manager + "')";
            command.ExecuteNonQuery();
        }
        override public void deleteBoard(Board board)
        {
            command.CommandText = "Delete FROM Board WHERE name='" + board.Name + "'";
            command.ExecuteNonQuery();
            command.CommandText= "Drop table " + board.Name+"Article";
            command.ExecuteNonQuery();
            for (int i = 0; i < board.article.Count; i++)
            {
                command.CommandText = "Drop table " + board.article[i].title + "Comment";
                command.ExecuteNonQuery();
            }
        }
        override public void addArticle(Board board, Article article)
        {
            command.CommandText = "Insert into " + board.Name + "Article(title,content,author,time,likes) values ('" + article.title + "','" + article.content + "','" + article.author + "','" + article.time + "'," + article.likes + ")";
            command.ExecuteNonQuery();
            command.CommandText = "Create table if not exists " + article.title + "Comment(comment longtext)";
            command.ExecuteNonQuery();
        }
        override public void addComment(Board board, Article article, string comment)
        {
            command.CommandText = "Insert into " + article.title + "Comment(comment) values ('" + comment + "')";
            command.ExecuteNonQuery();
        }
        override public void newlikes(Board board, Article article)
        {
            command.CommandText = "Update " + board.Name + "Article SET likes = " + article.likes + " WHERE title = '" + article.title + "'";
            command.ExecuteNonQuery();
        }
        override public void setBoardManager(Board board, string userName)
        {
            command.CommandText = "Update Board SET manager='" + userName + "'";
            command.ExecuteNonQuery();
            command.CommandText = "Insert into " + userName + "Board(board) values ('" + board.Name + ")";
            command.ExecuteNonQuery();
        }
    }





}